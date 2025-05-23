////using PetCare_system.Models;
////using System;
////using System.Linq;
////using System.Web.Mvc;
////using System.Data.Entity;
////using Microsoft.AspNet.Identity;

////namespace PetCare_system.Controllers
////{
////    public class PaymentForAdoptionController : Controller
////    {
////        private ApplicationDbContext db = new ApplicationDbContext();

////        // GET: PaymentForAdoption/Create/{adoptionId}
////        public ActionResult Payment(int? adoptionId)
////        {
////            if (adoptionId == null)
////            {
////                return new HttpNotFoundResult();
////            }

////            var adoption = db.pet_Adoptions
////                .FirstOrDefault(a => a.Adoption_Id == adoptionId);

////            if (adoption == null)
////            {
////                return HttpNotFound();
////            }

////            var model = new PaymentForAdoption
////            {
////                AdoptionId = adoption.Adoption_Id,
////                Amount = adoption.PetPrice
////            };

////            ViewBag.AdoptionDetails = adoption;
////            ViewBag.PaymentMethods = new SelectList(new[] { "Credit Card", "Debit Card", "PayPal" });
////            ViewBag.BankTypes = new SelectList(new[] { "Visa", "MasterCard", "American Express" });

////            return View(model);
////        }

////        // POST: PaymentForAdoption/Create
////        [HttpPost]
////        [ValidateAntiForgeryToken]
////        public ActionResult Payment([Bind(Include = "AdoptionId,Amount,PaymentMethod,BankType,CardHolderName,AccountNumber,CVV,ExpiryDate")] PaymentForAdoption payment)
////        {
////            var adoption = db.pet_Adoptions.Find(payment.AdoptionId);
////            if (adoption == null)
////            {
////                return HttpNotFound();
////            }

////            if (ModelState.IsValid)
////            {
////                // Set additional payment properties
////                payment.PaymentDate = DateTime.Now;
////                payment.Status = "Completed";
////                payment.TransactionId = GenerateTransactionId();
////                payment.UserId = User.Identity.GetUserId();

////                db.paymentForAdoptions.Add(payment);
////                db.SaveChanges();

////                // Update adoption status if needed
////                adoption.Status = "Approved";
////                db.Entry(adoption).State = EntityState.Modified;
////                db.SaveChanges();

////                return RedirectToAction("Confirmation", new { id = payment.Payment_Id });
////            }

////            ViewBag.AdoptionDetails = adoption;
////            ViewBag.PaymentMethods = new SelectList(new[] { "Credit Card", "Debit Card", "PayPal" }, payment.PaymentMethod);
////            ViewBag.BankTypes = new SelectList(new[] { "Visa", "MasterCard", "American Express" }, payment.BankType);

////            return View(payment);
////        }

////        // GET: PaymentForAdoption/PaymentSuccessful/{id}
////        public ActionResult Confirmation(int? id)
////        {
////            if (id == null)
////            {
////                return new HttpNotFoundResult();
////            }

////            var payment = db.paymentForAdoptions
////                .Include(p => p.Adoption)
////                .Include(p => p.User)
////                .FirstOrDefault(p => p.Payment_Id == id);

////            if (payment == null)
////            {
////                return HttpNotFound();
////            }

////            return View(payment);
////        }

////        private string GenerateTransactionId()
////        {
////            var random = new Random();
////            return $"ADOPT{DateTime.Now:yyyyMMddHHmmss}{random.Next(1000, 9999)}";
////        }

////        protected override void Dispose(bool disposing)
////        {
////            if (disposing)
////            {
////                db.Dispose();
////            }
////            base.Dispose(disposing);
////        }
////    }
////}

//using PetCare_system.Models;
//using System;
//using System.Linq;
//using System.Web.Mvc;
//using System.Data.Entity;
//using Microsoft.AspNet.Identity;

//namespace PetCare_system.Controllers
//{
//    public class PaymentForAdoptionController : Controller
//    {
//        private ApplicationDbContext db = new ApplicationDbContext();

//        // GET: PaymentForAdoption/Create/{adoptionId}
//        public ActionResult Create(int? adoptionId)
//        {
//            if (adoptionId == null)
//            {
//                return new HttpNotFoundResult();
//            }

//            var adoption = db.pet_Adoptions
//                .FirstOrDefault(a => a.Adoption_Id == adoptionId);

//            if (adoption == null)
//            {
//                return HttpNotFound();
//            }

//            var model = new PaymentForAdoption
//            {
//                AdoptionId = adoption.Adoption_Id,
//                Amount = adoption.PetPrice,
//                PetName = $"{adoption.PetType} ({adoption.SpecificBreed})"
//            };

//            ViewBag.AdoptionDetails = adoption;
//            ViewBag.PaymentMethods = new SelectList(new[] { "Credit Card", "Debit Card", "PayPal" });
//            ViewBag.BankTypes = new SelectList(new[] { "Visa", "MasterCard", "American Express" });

//            return View(model);
//        }

//        // POST: PaymentForAdoption/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "AdoptionId,Amount,PetName,PaymentMethod,BankType,CardHolderName,AccountNumber,CVV,ExpiryDate")] PaymentForAdoption payment)
//        {
//            var adoption = db.pet_Adoptions.Find(payment.AdoptionId);
//            if (adoption == null)
//            {
//                return HttpNotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                // Set additional payment properties
//                payment.PaymentDate = DateTime.Now;
//                payment.Status = "Completed";
//                payment.TransactionId = GenerateTransactionId();
//                payment.UserId = User.Identity.GetUserId();

//                db.paymentForAdoptions.Add(payment);
//                db.SaveChanges();

//                // Update adoption status if needed
//                adoption.Status = "Approved";
//                db.Entry(adoption).State = EntityState.Modified;
//                db.SaveChanges();

//                return RedirectToAction("Confirmation", new { id = payment.Payment_Id });
//            }

//            ViewBag.AdoptionDetails = adoption;
//            ViewBag.PaymentMethods = new SelectList(new[] { "Credit Card", "Debit Card", "PayPal" }, payment.PaymentMethod);
//            ViewBag.BankTypes = new SelectList(new[] { "Visa", "MasterCard", "American Express" }, payment.BankType);

//            return View(payment);
//        }

//        // GET: PaymentForAdoption/Confirmation/{id}
//        public ActionResult Confirmation(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpNotFoundResult();
//            }

//            var payment = db.paymentForAdoptions
//                .Include(p => p.Adoption)
//                .Include(p => p.User)
//                .FirstOrDefault(p => p.Payment_Id == id);

//            if (payment == null)
//            {
//                return HttpNotFound();
//            }

//            // Prepare the confirmation view model
//            ViewBag.Title = "Adoption Confirmed";
//            return View(payment);
//        }

//        private string GenerateTransactionId()
//        {
//            var random = new Random();
//            return $"ADOPT{DateTime.Now:yyyyMMddHHmmss}{random.Next(1000, 9999)}";
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}