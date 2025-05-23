using System;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PetCare_system;
using PetCare_system.Models;

namespace PetCare_system.Controllers
{
    public class Pet_BoardingController : Controller
    {
        // GET: Bookings/Create
        public ActionResult Pet_Boarding()
        {
            string currentUserId = User.Identity.GetUserId();
            Session["CurrentUserId"] = currentUserId;

            var model = new Pet_Boarding();

            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Id == currentUserId);
                if (user != null)
                {
                    model.OwnerName = user.FirstName; // Or user.FullName if available
                    model.Email = user.Email;
                    model.Phone = user.CellphoneNumber;
                }

                // Fetch pets owned by this user and pass them to the view as a dropdown list
                var userPets = db.pets.Where(p => p.UserId == currentUserId).ToList();
                ViewBag.Pets = new SelectList(userPets, "Id", "Name");
            }

            return View(model);
        }

        // GET: Bookings
       


        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Pet_Boarding(Pet_Boarding booking, int SelectedPetId)
        {
            if (!ModelState.IsValid)
            {
                return View(booking);
            }

            using (var db = new ApplicationDbContext())
            {
                string currentUserId = Session["CurrentUserId"]?.ToString();
                if (string.IsNullOrEmpty(currentUserId))
                {
                    return RedirectToAction("Login", "Account"); // Redirect if session expires
                }

                var user = db.Users.FirstOrDefault(u => u.Id == currentUserId);
                var pet = db.pets.FirstOrDefault(p => p.Id == SelectedPetId);

                if (user != null)
                {
                    booking.OwnerName = user.FirstName ?? "Not Provided";
                    booking.Email = user.Email ?? "Not Provided";
                    booking.Phone = user.CellphoneNumber ?? "Not Provided";
                }

                if (pet != null)
                {
                    booking.PetName = pet.Name ?? "Unknown";
                    booking.PetType = pet.Type ?? "Not Specified";
                    booking.PetBreed = pet.Breed ?? "Not Specified";
                }
                else
                {
                    ModelState.AddModelError("SelectedPetId", "Invalid pet selection.");
                    return View(booking); // Prevent saving invalid data
                }

                booking.BookingDate = DateTime.Now;
                booking.Status = "Pending";

                db.pet_Boardings.Add(booking);
                db.SaveChanges();

                // ✅ Send confirmation email
                string emailFrom = "shezielihle186@gmail.com";
                string emailPassword = "enno kjas mrlh sgwc"; // Consider storing securely (e.g., web.config or secrets manager)

                try
                {
                    using (MailMessage mm = new MailMessage(emailFrom, booking.Email))
                    {
                        mm.Subject = "Pet Boarding Confirmation";
                        mm.Body = $"Dear {booking.OwnerName},\n\n" +
                                  $"Your pet boarding has been booked successfully.\n\n" +
                                  $"Pet: {booking.PetName} ({booking.PetType})\n" +
                                  $"Package: {booking.Package} - R{booking.PackagePrice}\n" +
                                  $"Check-In: {booking.CheckInDate.ToShortDateString()}\n" +
                                  $"Check-Out: {booking.CheckOutDate.ToShortDateString()}\n" +
                                  $"Booking ID: {booking.board_Id}\n\n" +
                                  "Thank you for choosing PetCare Systems.";

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
                    TempData["Error"] = "Boarding booked, but the confirmation email could not be sent. Error: " + ex.Message;
                }

                return RedirectToAction("Confirmation", new { id = booking.board_Id });
            }
        }


        public JsonResult GetPetDetails(int petId)
        {
            using (var db = new ApplicationDbContext())
            {
                var pet = db.pets.FirstOrDefault(p => p.Id == petId);
                if (pet != null)
                {
                    return Json(new
                    {
                        Name = pet.Name,
                        Type = pet.Type,
                        Breed = pet.Breed
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        // GET: Bookings/Confirmation
        public ActionResult Confirmation(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var booking = db.pet_Boardings.Find(id);
                if (booking == null)
                {
                    return HttpNotFound();
                }

                return View(booking);
            }
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var booking = db.pet_Boardings.Find(id);
                if (booking == null)
                {
                    return HttpNotFound();
                }
                return View(booking);
            }
        }

        // GET: Bookings
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                var bookings = db.pet_Boardings.ToList();
                return View(bookings);
            }
        }
        

        public ActionResult IndexAdmin()
        {
            using (var db = new ApplicationDbContext())
            {
                var bookings = db.pet_Boardings.ToList();
                return View(bookings);
            }
        }



        public ActionResult AdminCheckIn(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var booking = db.pet_Boardings.Find(id);
                if (booking == null)
                {
                    TempData["Error"] = "Booking not found.";
                    return RedirectToAction("Index");
                }

                if (string.IsNullOrEmpty(booking.Check_Status) || booking.Check_Status.ToLower() == "null")
                {
                    // Initial check-in request — mark as pending
                    booking.Check_Status = "Pending";
                    db.SaveChanges();
                    TempData["Message"] = $"Pet '{booking.PetName}' has been marked as 'Pending', awaiting ADMIN confirmation.";
                }
                else if (booking.Check_Status.ToLower() == "pending")
                {
                    // Admin confirms the check-in
                    booking.Check_Status = "Checked In";
                    db.SaveChanges();
                    TempData["Message"] = $"Pet '{booking.PetName}' has been successfully checked in.";
                }
                else
                {
                    TempData["Error"] = $"Pet '{booking.PetName}' is already '{booking.Check_Status}'.";
                }

                return RedirectToAction("Index");
            }
        }
        public ActionResult CheckOut(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var booking = db.pet_Boardings.Find(id);
                if (booking == null)
                {
                    TempData["Error"] = "Booking not found.";
                    return RedirectToAction("Index");
                }

                if (booking.Check_Status != null && booking.Check_Status.ToLower() == "checked in")
                {
                    booking.Check_Status = "Checked Out";
                    db.SaveChanges();
                    TempData["Message"] = $"Pet '{booking.PetName}' has been successfully checked out.";
                }
                else
                {
                    TempData["Error"] = $"Pet '{booking.PetName}' cannot be checked out because current status is '{booking.Check_Status}'.";
                }

                return RedirectToAction("Index");
            }
        }


        // GET: Bookings/CheckIn/5
        public ActionResult CheckIn(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var booking = db.pet_Boardings.Find(id);
                if (booking == null)
                {
                    TempData["Error"] = "Booking not found.";
                    return RedirectToAction("Index");
                }

                if (string.IsNullOrEmpty(booking.Check_Status) || booking.Check_Status.ToLower() == "null")
                {
                    booking.Check_Status = "Pending";
                    db.SaveChanges();
                    TempData["Message"] = $"Pet '{booking.PetName}' has been successfully checked in, awaiting ADMIN confirmation.";
                }
                else
                {
                    TempData["Error"] = $"Pet '{booking.PetName}' is already checked in or has a different status.";
                }

                return RedirectToAction("Index");
            }
        }


        // GET: Bookings/Delete/5
        public ActionResult Delete(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var booking = db.pet_Boardings.Find(id);
                if (booking == null)
                {
                    return HttpNotFound();
                }
                return View(booking);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var booking = db.pet_Boardings.Find(id);
                if (booking != null)
                {
                    db.pet_Boardings.Remove(booking);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
        }
    }
}