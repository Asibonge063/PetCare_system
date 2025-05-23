// Controllers/SpaController.cs
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using PetCare_system.Models;

public class SpaController : Controller
{
    private ApplicationDbContext db = new ApplicationDbContext();

    // GET: Spa/Book
    public ActionResult Book()
    {
        //var model = new SpaBookingViewModel
        //{
        //    AvailableServices = db.spaServices.ToList(),
        //    Booking = new BookingSpar()
        //};

        //if (!model.AvailableServices.Any())
        //{
        //    return View("NoServices");
        //}

        return View(new SpaService());
    }

    // POST: Spa/Book
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Book(SpaBookingViewModel model)
    {
        if (ModelState.IsValid)
        {
            model.Booking.UserId = User.Identity.Name;
            model.Booking.SpaServiceId = model.Service.SpaServiceId;
            model.Booking.IsPaid = false;

            db.bookingSpars.Add(model.Booking);
            db.SaveChanges();

            return RedirectToAction("Payment", new { bookingId = model.Booking.BookingSparId });
        }

        // If we got this far, something failed
        model.AvailableServices = db.spaServices.ToList();
        return View(model);
    }

    // GET: Spa/Payment/5
    public ActionResult Payment(int bookingId)
    {
        var booking = db.bookingSpars
            .Include(b => b.SpaService)
            .FirstOrDefault(b => b.BookingSparId == bookingId);

        if (booking == null || booking.UserId != User.Identity.Name)
        {
            return HttpNotFound();
        }

        return View(booking);
    }

    // POST: Spa/CompletePayment
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult CompletePayment(int bookingId, string paymentMethod)
    {
        var booking = db.bookingSpars
            .Include(b => b.SpaService)
            .FirstOrDefault(b => b.BookingSparId == bookingId);

        if (booking == null || booking.UserId != User.Identity.Name)
        {
            return HttpNotFound();
        }

        booking.IsPaid = true;
        db.SaveChanges();

        return RedirectToAction("Confirmation", new { bookingId = bookingId });
    }

    // GET: Spa/Confirmation/5
    public ActionResult Confirmation(int bookingId)
    {
        var booking = db.bookingSpars
            .Include(b => b.SpaService)
            .FirstOrDefault(b => b.BookingSparId == bookingId);

        if (booking == null || booking.UserId != User.Identity.Name)
        {
            return HttpNotFound();
        }

        return View(booking);
    }
}