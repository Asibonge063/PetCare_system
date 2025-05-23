using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using PetCare_system.Models;
using System.Net.Mail;
using System.Net;
using System.Data;

namespace PetCare_system.Controllers
{
    public class PetAdoptionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PetAdoption/Create
        // GET: PetAdoption/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            if (user != null)
            {
                // Store user info in session
                Session["UserFullName"] = user.FirstName + " " + user.LastName;
                Session["UserEmail"] = user.Email;
                Session["UserPhone"] = user.CellphoneNumber;
            }

            // Dropdown options
            ViewBag.Id = new SelectList(db.pets.ToList(), "Id", "Name");
            ViewBag.ExperienceLevels = new SelectList(new[] { "None", "Some Experience", "Moderate Experience", "Experienced" });

            // Prepopulate model with session data
            var model = new Pet_Adoption
            {
                AdopterFullName = Session["UserFullName"]?.ToString(),
                AdopterEmail = Session["UserEmail"]?.ToString(),
                AdopterPhone = Session["UserPhone"]?.ToString(),
            };

            return View(model);
        }

        // POST: PetAdoption/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pet_Adoption adoption)
        {
            if (ModelState.IsValid)
            {
                var selectedPet = db.pets.Find(adoption.Id);
                var userId = User.Identity.GetUserId();
                var currentUser = db.Users.Find(userId);

                if (selectedPet == null)
                {
                    ModelState.AddModelError("Id", "Selected pet does not exist.");
                    ReloadDropdowns(adoption);
                    return View(adoption);
                }

                if (currentUser == null)
                {
                    ModelState.AddModelError("", "User not found.");
                    ReloadDropdowns(adoption);
                    return View(adoption);
                }

                // Assign user and date
                adoption.UserId = userId;
                adoption.ApplicationDate = DateTime.Now;

                // Save Pet snapshot
                adoption.PetName = selectedPet.Name;
                adoption.PetType = selectedPet.Type;
                adoption.PetBreed = selectedPet.Breed;
                adoption.DateOfBirth = selectedPet.DateOfBirth;

                // Save User snapshot
                adoption.AdopterFullName = currentUser.FirstName + " " + currentUser.LastName;
                adoption.AdopterEmail = currentUser.Email;
                adoption.AdopterPhone = currentUser.CellphoneNumber;

                // Set price based on pet type


                db.pet_Adoptions.Add(adoption);
                db.SaveChanges();

                // Store adoption info in session for ThankYou page
                Session["AdopterName"] = adoption.AdopterFullName;
                Session["PetType"] = adoption.PetType;
                Session["PetName"] = adoption.PetName;

                Session["AdopterPhone"] = adoption.AdopterPhone;
                Session["AdopterEmail"] = adoption.AdopterEmail;

                return RedirectToAction("ThankYouu");
            }

            ReloadDropdowns(adoption);
            return View(adoption);
        }

        // Helper to reload dropdowns
        private void ReloadDropdowns(Pet_Adoption adoption)
        {
            ViewBag.Id = new SelectList(db.pets.ToList(), "Id", "Name", adoption.Id);
            ViewBag.ExperienceLevels = new SelectList(new[] { "None", "Some Experience", "Moderate Experience", "Experienced" }, adoption.ExperienceLevel);
        }

        // GET: PetAdoption/ThankYou
        public ActionResult ThankYouu()
        {
            ViewBag.AdopterName = Session["AdopterName"];
            ViewBag.PetType = Session["PetType"];
            ViewBag.PetName = Session["PetName"];
            ViewBag.PhoneNumber = Session["AdopterPhone"];
            ViewBag.Email = Session["AdopterEmail"];
            ViewBag.Address = Session["AdopterAddress"];

            return View();
        }

        // GET: PetAdoption/Adopt
        public ActionResult Adopt()
        {
            var adoptions = db.pet_Adoptions
                              .Include(a => a.Pet)
                              .ToList(); // ✅ No filtering — gets all adoptions

            return View(adoptions);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitAdoptionSelection(int selectedAdoptionId)
        {
            var adoption = db.pet_Adoptions
                             .Include(a => a.Pet)
                             .FirstOrDefault(a => a.Adoption_Id == selectedAdoptionId);

            if (adoption == null)
                return HttpNotFound();

            // Redirect to form to fill details
            return RedirectToAction("FillApplication", new { id = selectedAdoptionId });
        }



        // GET: FillApplication
        public ActionResult FillApplication(int id)
        {
            var adoption = db.pet_Adoptions
                             .Include(a => a.Pet)
                             .FirstOrDefault(a => a.Adoption_Id == id);

            if (adoption == null)
                return HttpNotFound();

            ViewBag.ExperienceLevels = new SelectList(new[] { "None", "Some", "Experienced" });
            return View(adoption);
        }

        // POST: FillApplication
        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: PetAdoption/FillApplication

        public ActionResult FillApplication(Pet_Adoption model)
        {


            if (!model.HasAgreedToTerms)
            {
                ModelState.AddModelError("HasAgreedToTerms", "You must agree to the terms.");
            }

            if (ModelState.IsValid)
            {
                var adoptionInDb = db.pet_Adoptions.Find(model.Id);
                if (adoptionInDb == null)
                    return HttpNotFound();

                // Update the Pet_Adoption data
                adoptionInDb.ExperienceLevel = model.ExperienceLevel;
                adoptionInDb.HomeDescription = model.HomeDescription;
                adoptionInDb.AdoptionReason = model.AdoptionReason;
                adoptionInDb.HasAgreedToTerms = model.HasAgreedToTerms;
                adoptionInDb.Status = "Pending"; // Set status to Pending when form is submitted

                // Save changes to the Pet_Adoption record
                db.SaveChanges();

                // Now save the data into the Adoption table as well
                var newAdoption = new Adoption
                {
                    ExperienceLevel = adoptionInDb.ExperienceLevel,
                    HomeDescription = adoptionInDb.HomeDescription,
                    AdoptionReason = adoptionInDb.AdoptionReason,
                    Status = "Pending", // This can also be customized
                    SubmittedDate = adoptionInDb.ApplicationDate, // Use the original application date
                    UserId = adoptionInDb.UserId,
                    AdopterFullName = adoptionInDb.AdopterFullName,
                    AdopterEmail = adoptionInDb.AdopterEmail,
                    AdopterPhone = adoptionInDb.AdopterPhone,
                    Id = adoptionInDb.Pet.Id, // Reference the Pet
                    PetName = adoptionInDb.Pet.Name,
                    PetType = adoptionInDb.Pet.Type,
                    PetBreed = adoptionInDb.Pet.Breed,
                    PetDOB = adoptionInDb.Pet.DateOfBirth
                };

                // Save the new adoption record into the Adoption table
                db.Adoptions.Add(newAdoption);
                db.SaveChanges();
                // Send email confirmation to the adopter
                try
                {
                    string emailFrom = "shezielihle186@gmail.com";
                    string emailPassword = "xjop iuut owdu loav"; // Use secure storage in production

                    using (MailMessage mm = new MailMessage(emailFrom, newAdoption.AdopterEmail))
                    {
                        mm.Subject = "Adoption Application Submitted";
                        mm.Body = $"Dear {newAdoption.AdopterFullName},\n\n" +
                                  "Your adoption application has been successfully submitted and is currently under review.\n\n" +
                                  $"Pet Details:\n" +
                                  $"Name: {newAdoption.PetName}\n" +
                                  $"Type: {newAdoption.PetType}\n" +
                                  $"Breed: {newAdoption.PetBreed}\n\n" +
                                  "Our adoption management team will get back to you shortly.\n\n" +
                                  "Best regards,\n" +
                                  "PetCare Systems Team";

                        mm.IsBodyHtml = false;

                        using (SmtpClient smtp = new SmtpClient())
                        {
                            smtp.Host = "smtp.gmail.com";
                            smtp.EnableSsl = true;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new NetworkCredential(emailFrom, emailPassword);
                            smtp.Port = 587;

                            smtp.Send(mm);
                        }
                    }
                }
                catch (SmtpException ex)
                {
                    TempData["Error"] = "Application submitted, but confirmation email could not be sent. Error: " + ex.Message;
                }

                // Redirect to a confirmation page or adoption list
                return RedirectToAction("ThankYou", new { id = adoptionInDb.Id });
            }

            // Reload dropdowns in case of validation errors
            ViewBag.ExperienceLevels = new SelectList(new[] { "None", "Some", "Experienced" }, model.ExperienceLevel);
            return View(model);
        }

        // GET: PetAdoption/ThankYou
        // GET: PetAdoption/ThankYou
        public ActionResult ThankYou(int id)
        {
            var adoption = db.Adoptions
                .Include(a => a.Pet)
                .Include(a => a.User)
                .FirstOrDefault(a => a.Id == id);

            if (adoption == null)
            {
                return HttpNotFound();
            }

            return View(adoption); // Display adoption confirmation
        }
        //public ActionResult ThankYou(int id)
        //{
        //    var UserId=User.Identity.GetUserId();
        //    var userEmail = User.Identity.Name;
        //    var adoptions = db.Adoptions
        //                      .Include(a => a.Pet)
        //                      .Where(a => a.UserId == UserId)
        //                      .ToList();

        //    return View(adoptions); // must match the IEnumerable<Adoption> in the view
        //}

        // GET: PetAdoption/Payment/{id}
        // GET: PetAdoption/ThankYou

        public ActionResult Payment(int id)
        {
            var adoption = db.pet_Adoptions
                .Include(a => a.Pet)
                .FirstOrDefault(a => a.Id == id);

            if (adoption == null || adoption.UserId != User.Identity.GetUserId())
            {
                return HttpNotFound();
            }

            var paymentModel = new PaymentForAdoption
            {
                AdoptionId = id,

                PetName = $"{adoption.Pet?.Name} ({adoption.Pet?.Breed})"
            };

            ViewBag.PaymentMethods = new SelectList(new[] { "Credit Card", "Debit Card", "PayPal" });
            ViewBag.BankTypes = new SelectList(new[] { "Visa", "MasterCard", "American Express" });

            return View(paymentModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}