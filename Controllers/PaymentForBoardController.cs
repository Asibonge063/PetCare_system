using PetCare_system.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Globalization;

namespace PetCare_system.Controllers
{
    public class PaymentForBoardController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext(); // Assuming your DbContext is set up

        // GET: Payment/Create/{boardingId}
        public ActionResult Create(int? boardingId)
        {
            if (boardingId == null)
            {
                return new HttpNotFoundResult();
            }

            var boarding = db.pet_Boardings
                .FirstOrDefault(b => b.board_Id == boardingId);

            if (boarding == null)
            {
                return HttpNotFound();
            }

            var model = new PaymentForBoard
            {
                BoardingId = boarding.board_Id,
                AmountPaid = 0 // Will be set based on package
            };

            // Set amount based on package
            model.SetAmountFromPackage(boarding.Package);

            ViewBag.BoardingDetails = boarding;
            ViewBag.PaymentMethods = new SelectList(new[] { "Credit Card", "Debit Card", "PayPal" });
            ViewBag.BankTypes = new SelectList(new[] { "Visa", "MasterCard", "American Express" });

            return View(model);
        }

        // POST: Payment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BoardingId,AmountPaid,PaymentMethod,BankType,CardHolderName,AccountNumber,CVV,ExpiryDate")] PaymentForBoard payment)
        {
            var boarding =db.pet_Boardings.Find(payment.BoardingId);
            if (boarding == null)
            {
                return HttpNotFound();
            }

            // Set amount based on package
            payment.SetAmountFromPackage(boarding.Package);

            if (ModelState.IsValid)
            {
                // Set additional payment properties
                payment.PaymentDate = DateTime.Now;
                payment.Status = "Completed";
                payment.TransactionId = GenerateTransactionId();
                payment.UserId = User.Identity.GetUserId();

                db.paymentForBoards.Add(payment);
                db.SaveChanges();

                // Update boarding status if needed
                boarding.Status = "Confirmed";
                db.Entry(boarding).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("PaymentSuccessful", new { id = payment.Payment_boardId });
            }

            ViewBag.BoardingDetails = boarding;
            ViewBag.PaymentMethods = new SelectList(new[] { "Credit Card", "Debit Card", "PayPal" }, payment.PaymentMethod);
            ViewBag.BankTypes = new SelectList(new[] { "Visa", "MasterCard", "American Express" }, payment.BankType);

            return View(payment);
        }

        // GET: Payment/PaymentSuccessful/{id}
        public ActionResult PaymentSuccessful(int? id)
        {
            if (id == null)
            {
                return new HttpNotFoundResult();
            }

            var payment = db.paymentForBoards
                .Include(p => p.Boarding)
                .Include(p => p.User)
                .FirstOrDefault(p => p.Payment_boardId == id);

            if (payment == null)
            {
                return HttpNotFound();
            }

            return View(payment);
        }

        private string GenerateTransactionId()
        {
            var random = new Random();
            return $"TXN{DateTime.Now:yyyyMMddHHmmss}{random.Next(1000, 9999)}";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}