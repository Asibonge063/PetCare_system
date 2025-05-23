using Microsoft.AspNet.Identity;
using PetCare_system.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace PetCare_system.Controllers
{
    public class PetController : Controller
    {
        // GET: Pet
        private readonly ApplicationDbContext _context;


        public PetController(ApplicationDbContext context)
        {
            _context = context;
        }


        public PetController() : this(new ApplicationDbContext()) { }

        public ActionResult Index()
        {
            var pets = _context.pets.ToList();
            return View(pets);
        }



        public ActionResult Register()
        {
            return View(new Pet());
        }
       

            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Register(Pet pet, HttpPostedFileBase ImageFile)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        // Get current user ID
                        string userId = User.Identity.GetUserId();

                        if (string.IsNullOrEmpty(userId))
                        {
                            ModelState.AddModelError("", "You must be logged in to register a pet.");
                            return View(pet);
                        }

                        // Assign user to pet
                        pet.UserId = userId;

                        // Handle image upload
                        if (ImageFile != null && ImageFile.ContentLength > 0)
                        {
                            // Validate image
                            if (!IsValidImage(ImageFile))
                            {
                                ModelState.AddModelError("ImageFile", "Please upload a valid image file (JPEG, PNG, GIF).");
                                return View(pet);
                            }

                            // Create upload directory if it doesn't exist
                            string uploadDir = Server.MapPath("~/Uploads/Pets/");
                            if (!Directory.Exists(uploadDir))
                            {
                                Directory.CreateDirectory(uploadDir);
                            }

                            // Generate unique filename
                            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(ImageFile.FileName)}";
                            string filePath = Path.Combine(uploadDir, fileName);

                            // Save file
                            ImageFile.SaveAs(filePath);

                            // Set path in model
                            pet.PicturePath = $"/Uploads/Pets/{fileName}";
                        }

                        // Add to database
                        _context.pets.Add(pet);
                        _context.SaveChanges();

                        // Success message
                        TempData["SuccessMessage"] = "Pet registered successfully!";
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (Exception ex)
                {
                    // Log the error
                    System.Diagnostics.Debug.WriteLine($"Error registering pet: {ex.Message}");

                    // User-friendly error message
                    ModelState.AddModelError("", "An error occurred while registering your pet. Please try again.");
                }

                return View(pet);
            }

            private bool IsValidImage(HttpPostedFileBase file)
            {
                if (file == null || file.ContentLength == 0)
                    return false;

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                    return false;

                try
                {
                    using (var img = System.Drawing.Image.FromStream(file.InputStream))
                    {
                        return img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg) ||
                               img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png) ||
                               img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif);
                    }
                }
                catch
                {
                    return false;
                }
            }

        public ActionResult Dashboard()
        {
            // Get the logged-in user's ID
            string userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if not authenticated
            }

            // Retrieve only the pets belonging to the logged-in user
            var userPets = _context.pets.Where(p => p.UserId == userId).ToList();

            return View(userPets);
        }
        public ActionResult Edit(int id)
        {
            var pet = _context.pets.Find(id);
            if (pet == null || pet.UserId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        [HttpPost]
        public ActionResult Edit(Pet pet, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                var existingPet = _context.pets.Find(pet.Id);
                if (existingPet == null || existingPet.UserId != User.Identity.GetUserId())
                {
                    return HttpNotFound();
                }

                existingPet.Name = pet.Name;
                existingPet.DateOfBirth = pet.DateOfBirth;
                existingPet.Breed = pet.Breed;

                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    string uploadDir = Server.MapPath("~/Uploads/Pets/");
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                    string filePath = Path.Combine(uploadDir, fileName);
                    ImageFile.SaveAs(filePath);

                    existingPet.PicturePath = "/Uploads/Pets/" + fileName;
                }

                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View(pet);
        }


        public ActionResult Delete(int id)
        {
            var pet = _context.pets.Find(id);
            if (pet == null || pet.UserId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var pet = _context.pets.Find(id);
            if (pet == null || pet.UserId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }

            _context.pets.Remove(pet);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }


       
        public ActionResult AllPatients(string searchString, string petTypeFilter)
        {
            // First get the base query without the age calculation
            var petsQuery = _context.pets.Include(p => p.User).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                petsQuery = petsQuery.Where(p =>
                    p.Name.Contains(searchString) ||
                    p.Breed.Contains(searchString) ||
                    p.User.UserName.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(petTypeFilter))
            {
                petsQuery = petsQuery.Where(p => p.Type == petTypeFilter);
            }

            // Execute the query and then calculate ages in memory
            var petList = petsQuery
                .OrderBy(p => p.Name)
                .ToList()  // Important: Materialize the query here
                .Select(p => new PetPatientViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Type = p.Type,
                    Breed = p.Breed,
                    Age = CalculateAge(p.DateOfBirth),  // Now this works because we're in memory
                    DateOfBirth = p.DateOfBirth,
                    PicturePath = p.PicturePath,
                    OwnerName = p.User.UserName
                }).ToList();

            ViewBag.PetTypes = new SelectList(_context.pets.Select(p => p.Type).Distinct().OrderBy(t => t));
            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentTypeFilter = petTypeFilter;

            return View(petList);
        }

        private int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        public ActionResult GetImage(int id)
        {
            var pet = _context.pets.Find(id);
            if (pet == null || string.IsNullOrEmpty(pet.PicturePath))
            {
                return File(Server.MapPath("~/Content/Images/default-pet.jpg"), "image/jpeg");
            }

            var path = Server.MapPath(pet.PicturePath);
            return System.IO.File.Exists(path)
                ? File(path, "image/jpeg")
                : File(Server.MapPath("~/Content/Images/default-pet.jpg"), "image/jpeg");
        }

    }
}
