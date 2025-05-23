using Microsoft.AspNet.Identity;
using PetCare_system.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace PetCare_system.Controllers
{
    [Authorize]
    public class MembershipController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        // GET: Membership/Index
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var memberships = _context.Memberships
                .Include(m => m.Pet)
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.EndDate)
                .ToList();

            return View(memberships);
        }

        public ActionResult Register()
        {
            var userId = User.Identity.GetUserId();
            var pets = _context.pets
                .Where(p => p.UserId == userId)
                .ToList();

            if (!pets.Any())
            {
                TempData["ErrorMessage"] = "You need to register a pet first.";
                return RedirectToAction("Create", "Pet");
            }

            ViewBag.PetId = new SelectList(pets, "Id", "Name");
            ViewBag.MembershipTypes = new SelectList(new[]
            {
        new { Value = "Weekly", Text = "Weekly - R250" },
        new { Value = "Monthly", Text = "Monthly - R850" },
        new { Value = "Yearly", Text = "Yearly - R8500" }
    }, "Value", "Text");

            ViewBag.BankTypes = new SelectList(new[] { "Visa", "MasterCard", "American Express", "Discover" });

            // Initialize with default amount
            var model = new MembershipViewModel
            {
                AmountPaid = 250.00m // Default to Weekly price
            };

            return View(model);
        }
        // POST: Membership/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(MembershipViewModel model)
        {
            // Calculate expected amount
            decimal expectedAmount = CalculatePrice(model.MembershipType);

            // Round both values to 2 decimal places for comparison
            var roundedPaid = Math.Round(model.AmountPaid, 2);
            var roundedExpected = Math.Round(expectedAmount, 2);

            if (roundedPaid != roundedExpected)
            {
                ModelState.AddModelError("AmountPaid",
                    $"Payment amount must be R{roundedExpected} for {model.MembershipType} membership");
            }
            var userId = User.Identity.GetUserId();
            var pet = _context.pets.FirstOrDefault(p => p.Id == model.PetId && p.UserId == userId);

            if (pet == null)
            {
                ModelState.AddModelError("PetId", "Selected pet not found");
                return Register();
            }

            // Validate credit card if payment method is credit card
            if (model.PaymentMethod == "Credit Card")
            {
                if (string.IsNullOrEmpty(model.AccountNumber) ||
                    string.IsNullOrEmpty(model.CVV) ||
                    !model.ExpiryDate.HasValue ||
                    model.ExpiryDate.Value < DateTime.Today)
                {
                    ModelState.AddModelError("", "Complete credit card information is required");
                    return Register();
                }
            }

            // Check if pet already has an active membership
            var existingActiveMembership = _context.Memberships
                .Any(m => m.PetId == model.PetId &&
                         m.UserId == userId &&
                         m.Status == "Active" &&
                         m.EndDate >= DateTime.Today);

            if (existingActiveMembership)
            {
                TempData["ErrorMessage"] = "This pet already has an active membership.";
                return RedirectToAction("Index");
            }

            var membership = new Membership
            {
                UserId = userId,
                PetId = model.PetId,
                MembershipType = model.MembershipType,
                StartDate = DateTime.Today,
                EndDate = CalculateEndDate(model.MembershipType),
                Status = "Active",
                PaymentAmount = model.AmountPaid,
                PaymentMethod = model.PaymentMethod,
                PaymentStatus = "Completed",
                PaymentDate = DateTime.Now,
                AccountNumber = model.PaymentMethod == "Credit Card" ? model.AccountNumber : null,
                CVV = model.PaymentMethod == "Credit Card" ? model.CVV : null,
                ExpiryDate = model.PaymentMethod == "Credit Card" ? model.ExpiryDate : null,
                AccountHolder = model.PaymentMethod == "Credit Card" ? model.AccountHolder : null,
                BankType = model.PaymentMethod == "Credit Card" ? model.BankType : null
            };

            _context.Memberships.Add(membership);
            _context.SaveChanges();

            // Email credentials
            string emailFrom = "shezielihle186@gmail.com";
            string emailPassword = "xjop iuut owdu loav"; // Replace with a secure credential storage

            // Send email confirmation to the user
            try
            {
                using (MailMessage mm = new MailMessage(emailFrom, User.Identity.Name)) // Assuming the user's email is in User.Identity.Name
                {
                    mm.Subject = "Membership Registration Successful";
                    mm.Body = $"Dear Pet Owner,\n\n" +
                              "Your membership registration has been successfully processed.\n\n" +
                              $"Membership Type: {model.MembershipType}\n" +
                              $"Membership Start Date: {membership.StartDate.ToShortDateString()}\n" +
                              $"Membership End Date: {membership.EndDate.ToShortDateString()}\n\n" +
                              "Thank you for choosing our service!\n\n" +
                              "Best regards,\n" +
                              "PetCare Systems";

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

                TempData["SuccessMessage"] = "Membership registered, payment processed, and confirmation email sent successfully!";
            }
            catch (SmtpException ex)
            {
                // Log or handle email sending errors
                TempData["ErrorMessage"] = "Membership registered and payment processed, but the confirmation email could not be sent. Error: " + ex.Message;
            }

            return RedirectToAction("Details", "Membership", new { id = membership.Id });
        }

        // In the Renew action method of MembershipController
        public ActionResult Renew(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();
            var membership = _context.Memberships
                .Include(m => m.Pet)
                .FirstOrDefault(m => m.Id == id && m.UserId == userId);

            if (membership == null)
            {
                return HttpNotFound();
            }

            ViewBag.MembershipTypes = new SelectList(new[]
            {
        new { Value = "Weekly", Text = "Weekly - R250" },
        new { Value = "Monthly", Text = "Monthly - R850" },
        new { Value = "Yearly", Text = "Yearly - R8500" }
    }, "Value", "Text", membership.MembershipType);

            ViewBag.BankTypes = new SelectList(new[]
            {
        "Visa", "MasterCard", "American Express", "Discover"
    });

            ViewBag.CurrentEndDate = membership.EndDate;

            return View(new MembershipViewModel
            {
                PetId = membership.PetId,
                MembershipType = membership.MembershipType
            });
        }

        // POST: Membership/Renew/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Renew(int id, MembershipViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var oldMembership = _context.Memberships
                .FirstOrDefault(m => m.Id == id && m.UserId == userId);

            if (oldMembership == null)
            {
                return HttpNotFound();
            }

            // Validate payment amount matches membership type
            decimal expectedAmount = CalculatePrice(model.MembershipType);
            if (model.AmountPaid != expectedAmount)
            {
                ModelState.AddModelError("AmountPaid", $"Payment amount must be R{expectedAmount} for {model.MembershipType} membership");
                return Renew(id);
            }

            // Validate credit card if payment method is credit card
            if (model.PaymentMethod == "Credit Card")
            {
                if (string.IsNullOrEmpty(model.AccountNumber) ||
                    string.IsNullOrEmpty(model.CVV) ||
                    !model.ExpiryDate.HasValue ||
                    model.ExpiryDate.Value < DateTime.Today)
                {
                    ModelState.AddModelError("", "Complete credit card information is required");
                    return Renew(id);
                }
            }

            // Create new membership for renewal
            var renewedMembership = new Membership
            {
                UserId = userId,
                PetId = model.PetId,
                MembershipType = model.MembershipType,
                StartDate = DateTime.Today,
                EndDate = CalculateEndDate(model.MembershipType),
                Status = "Active",
                PaymentAmount = model.AmountPaid,
                PaymentMethod = model.PaymentMethod,
                PaymentStatus = "Completed",
                PaymentDate = DateTime.Now,
                AccountNumber = model.PaymentMethod == "Credit Card" ? model.AccountNumber : null,
                CVV = model.PaymentMethod == "Credit Card" ? model.CVV : null,
                ExpiryDate = model.PaymentMethod == "Credit Card" ? model.ExpiryDate : null,
                AccountHolder = model.PaymentMethod == "Credit Card" ? model.AccountHolder : null,
                BankType = model.PaymentMethod == "Credit Card" ? model.BankType : null,
                PreviousMembershipId = oldMembership.Id
            };

            // Mark old membership as expired
            oldMembership.Status = "Expired";

            _context.Memberships.Add(renewedMembership);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Membership renewed and payment processed successfully!";
            return RedirectToAction("Details", new { id = renewedMembership.Id });
        }

        // GET: Membership/Cancel/5
        public ActionResult Cancel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();
            var membership = _context.Memberships
                .Include(m => m.Pet)
                .FirstOrDefault(m => m.Id == id && m.UserId == userId);

            if (membership == null)
            {
                return HttpNotFound();
            }

            // Check if membership is active and not already expired
            if (membership.Status != "Active" || membership.EndDate < DateTime.Today)
            {
                TempData["ErrorMessage"] = "Only active memberships can be cancelled.";
                return RedirectToAction("Details", new { id = membership.Id });
            }

            return View(membership);
        }

        // POST: Membership/Cancel/5
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public ActionResult CancelConfirmed(int id)
        {
            var userId = User.Identity.GetUserId();
            var membership = _context.Memberships
                .FirstOrDefault(m => m.Id == id && m.UserId == userId);

            if (membership == null)
            {
                return HttpNotFound();
            }

            membership.Status = "Cancelled";
            membership.EndDate = DateTime.Today;
            membership.CancellationDate = DateTime.Now;

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Membership cancelled successfully.";
            return RedirectToAction("Index");
        }

        // Helper method to check if pet has active membership
        public static bool HasActiveMembership(ApplicationDbContext context, int petId, string userId)
        {
            return context.Memberships
                .Any(m => m.PetId == petId &&
                         m.UserId == userId &&
                         m.Status == "Active" &&
                         m.EndDate >= DateTime.Today &&
                         m.PaymentStatus == "Completed");
        }

        // Helper methods
        private DateTime CalculateEndDate(string membershipType)
        {
            switch (membershipType)
            {
                case "Weekly":
                    return DateTime.Today.AddDays(7);
                case "Monthly":
                    return DateTime.Today.AddMonths(1);
                case "Yearly":
                    return DateTime.Today.AddYears(1);
                default:
                    return DateTime.Today;
            }
        }

        private decimal CalculatePrice(string membershipType)
        {
            switch (membershipType)
            {
                case "Weekly":
                    return 250.00m;
                case "Monthly":
                    return 850.00m;
                case "Yearly":
                    return 8500.00m;
                default:
                    return 0.00m;
            }
        }
        // GET: Membership/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();
            var membership = _context.Memberships
                .Include(m => m.Pet)
                .FirstOrDefault(m => m.Id == id && m.UserId == userId);

            if (membership == null)
            {
                return HttpNotFound();
            }

            // Calculate remaining days for the membership
            var remainingDays = (membership.EndDate - DateTime.Today).Days;
            ViewBag.RemainingDays = remainingDays > 0 ? remainingDays : 0;
            ViewBag.IsActive = membership.Status == "Active" && membership.EndDate >= DateTime.Today;

            return View(membership);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}
