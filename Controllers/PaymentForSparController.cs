using PetCare_system.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace PetCare_system.Controllers
{
   
    public class PaymentForSparController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: PaymentForSpar
        // GET: Payment/{bookingId}
        public ActionResult Payment(int bookingId)
        {
            var booking = db.bookingSpars
                //.Include(b => b.SpaService)
                .FirstOrDefault(b => b.BookingSparId == bookingId);

            if (booking == null || booking.UserId != User.Identity.Name)
            {
                return HttpNotFound();

            }

            return View(booking);
        }

        // POST: Payment/Confirm
        [HttpPost]
        public ActionResult Confirm(int bookingId, string paymentMethod)
        {
            var booking = db.bookingSpars
                //.Include(b => b.SpaService)
                .FirstOrDefault(b => b.BookingSparId == bookingId);

            if (booking == null || booking.UserId != User.Identity.Name)
            {
                return HttpNotFound();

            }

            var payment = new PaymentForSpar
            {
                BookingId = bookingId,
                Amount = booking.SpaService.Price,
                PaymentMethod = paymentMethod
            };

            booking.IsPaid = true;
            db.paymentForSpars.Add(payment);
            db.SaveChanges();

            return RedirectToAction("Confirmation", new { bookingId = bookingId });
        }

        // GET: Payment/Confirmation/{bookingId}
        public ActionResult Confirmation(int bookingId)
        {
            var booking = db.bookingSpars
                //.Include(b => b.SpaService)
                .FirstOrDefault(b => b.BookingSparId == bookingId);

            if (booking == null || booking.UserId != User.Identity.Name)
            {
                return HttpNotFound();

            }

            return View(booking);
        }
    }
}