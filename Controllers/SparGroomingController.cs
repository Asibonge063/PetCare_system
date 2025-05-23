using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PetCare_system.Models;

namespace PetCare_system.Controllers
{
    public class SparGroomingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SpaGrooming/Create
        // GET: SpaGrooming/Create
        // GET: SpaGrooming/Create

        // GET: SpaGrooming/Create
        public ActionResult Create()
        {
            var model = new Spar_Grooming();

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);

                if (user != null)
                {
                    // Populate owner information
                    model.OwnerName = $"{user.FirstName} {user.LastName}";
                    model.Email = user.Email;
                    model.PhoneNumber = user.CellphoneNumber;

                    // Get user's pets and create select list
                    var userPets = db.pets
                                   .Where(p => p.UserId == userId)
                                   .OrderBy(p => p.Name)
                                   .ToList();

                    ViewBag.PetId = new SelectList(userPets, "Id", "Name");

                    // Debug output
                    System.Diagnostics.Debug.WriteLine($"User {userId} has {userPets.Count} pets");
                }
            }

            PopulateViewBags();
            return View(model);
        }

        [HttpGet]
        public JsonResult GetPetDetails(int id)
        {
            var pet = db.pets.Find(id);
            if (pet == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                pet.Name,
                pet.Type,
                pet.Breed
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Spar_Grooming booking)
        {
            if (ModelState.IsValid)
            {
                // If a pet was selected, ensure details match
                if (booking.SelectedPetId.HasValue)
                {
                    var selectedPet = db.pets.Find(booking.SelectedPetId.Value);
                    if (selectedPet != null)
                    {
                        booking.PetName = selectedPet.Name;
                        booking.PetType = (PetType)Enum.Parse(typeof(PetType), selectedPet.Type);
                        booking.Breed = selectedPet.Breed;
                    }
                }

                booking.BookingDate = DateTime.Now;
                booking.Status = BookingStatus.Pending;
                booking.PaymentStatus = PaymentStatus.Pending;

                db.spar_Groomings.Add(booking);
                db.SaveChanges();

                return RedirectToAction("Confirmation", new { id = booking.BookingId });
            }

            // Repopulate ViewBag if validation fails
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var userPets = db.pets
                               .Where(p => p.UserId == userId)
                               .ToList();
                ViewBag.PetId = new SelectList(userPets, "Id", "Name");
            }

            PopulateViewBags();
            return View(booking);
        }

        // GET: SpaGrooming/Confirmation/5
        // GET: SpaGrooming/Confirmation/5
        public ActionResult Confirmation(int id)
        {
            var booking = db.spar_Groomings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }

            // If payment is already completed, show receipt
            if (booking.PaymentStatus == PaymentStatus.Completed)
            {
                return RedirectToAction("Receipt", new { id = booking.BookingId });
            }

            return View(booking);
        }

        // POST: SpaGrooming/Confirmation/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirmation(int id, string action)
        {
            var booking = db.spar_Groomings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }

            if (action == "PayNow")
            {
                return RedirectToAction("Payment", "SparGrooming", new { id = booking.BookingId });
            }

            return View(booking);
        }

        private void PopulateViewBags()
        {
            // Time slots
            var timeSlots = new Dictionary<string, string>
            {
                { "9am", "9:00 AM" },
                { "10am", "10:00 AM" },
                { "11am", "11:00 AM" },
                { "12pm", "12:00 PM" },
                { "1pm", "1:00 PM" },
                { "2pm", "2:00 PM" },
                { "3pm", "3:00 PM" },
                { "4pm", "4:00 PM" }
            };
            ViewBag.PreferredTimes = new SelectList(timeSlots, "Key", "Value");

            // Duration options
            var durations = new Dictionary<double, string>
            {
                { 1, "1 hour (R50)" },
                { 1.5, "1.5 hours (R75)" },
                { 2, "2 hours (R100)" },
                { 2.5, "2.5 hours (R125)" },
                { 3, "3 hours (R150)" }
            };
            ViewBag.DurationOptions = new SelectList(durations, "Key", "Value");
        }

        // GET: SpaGrooming/Payment/5
        // GET: SpaGrooming/Payment/5
        public ActionResult Payment(int id)
        {
            var booking = db.spar_Groomings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }

            var model = new Spar_GroomPayment
            {
                BookingId = booking.BookingId,
                Amount = booking.TotalPrice
            };

            // Pass the booking details to the view
            ViewBag.BookingDetails = booking; // This line was missing

            return View(model);
        }

        // POST: SparGrooming/Payment/8
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Payment(Spar_GroomPayment model)
        {
            if (ModelState.IsValid)
            {
                var booking = db.spar_Groomings.Find(model.BookingId);
                if (booking == null)
                {
                    return HttpNotFound();
                }

                booking.PaymentStatus = PaymentStatus.Completed;
                booking.PaymentDate = DateTime.Now;
                booking.PaymentMethod = PaymentMethod.CreditCard;
                booking.TransactionId = Guid.NewGuid().ToString().Substring(0, 12).ToUpper();

                db.SaveChanges();

                return RedirectToAction("Receipt", new { id = booking.BookingId });
            }

            return View(model);
        }
        // GET: SpaGrooming/Receipt/5
        public ActionResult Receipt(int id)
        {
            var booking = db.spar_Groomings.Find(id);
            if (booking == null || booking.PaymentStatus != PaymentStatus.Completed)
            {
                return HttpNotFound();
            }

            return View(booking);
        }

        // GET: SpaGrooming/PrintReceipt/5
        public ActionResult PrintReceipt(int id)
        {
            var booking = db.spar_Groomings.Find(id);
            if (booking == null || booking.PaymentStatus != PaymentStatus.Completed)
            {
                return HttpNotFound();
            }

            return View("Receipt", booking);
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