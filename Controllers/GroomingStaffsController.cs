using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PetCare_system.Models;

namespace PetCare_system.Controllers
{
    public class GroomingStaffsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: GroomingStaffs
        public ActionResult Index()
        {
            return View(db.GroomingStaffs.ToList());
        }
        // GET: GroomingReport/Report
        // GET: GroomingReport/Report
        [HttpGet]
        // In your GroomingStaffsController.cs
       
        public ActionResult Report()
        {
            // Check if we have a booking in session
            var currentBooking = Session["CurrentBooking"] as dynamic;
            if (currentBooking != null)
            {
                ViewBag.BookingId = currentBooking.BookingId;
                ViewBag.PetName = currentBooking.PetName;
                ViewBag.PetType = $"{currentBooking.PetType} ({currentBooking.Breed})";
                ViewBag.OwnerName = currentBooking.OwnerName;
                ViewBag.GroomingType = currentBooking.ServiceType;
                ViewBag.Email = currentBooking.Email;
                ViewBag.PhoneNumber = currentBooking.PhoneNumber;
                ViewBag.SpecialInstructions = currentBooking.SpecialInstructions;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Report(FormCollection form)
        {
            // This is a demo - just show success message without saving to database
            TempData["Message"] = "Report have been submitted successfully.";

            // Clear the session for demo purposes
            Session.Remove("CurrentBooking");

            return RedirectToAction("Report");
        }

        [HttpGet]
        public ActionResult GetBookingDetails(int bookingId)
        {
            // Get the booking from the database
            var booking = db.spar_Groomings.FirstOrDefault(b => b.BookingId == bookingId);

            if (booking == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            // Store the booking details in session
            Session["CurrentBooking"] = new
            {
                BookingId = booking.BookingId,
                PetName = booking.PetName,
                PetType = booking.PetType.ToString(),
                Breed = booking.Breed,
                OwnerName = booking.OwnerName,
                ServiceType = booking.ServiceType.ToString(),
                Email = booking.Email,
                PhoneNumber = booking.PhoneNumber,
                SpecialInstructions = booking.SpecialInstructions
            };

            var result = new
            {
                BookingId = booking.BookingId,
                PetName = booking.PetName,
                PetType = $"{booking.PetType.ToString()} ({booking.Breed})",
                OwnerName = booking.OwnerName,
                GroomingType = booking.ServiceType.ToString(),
                Email = booking.Email,
                PhoneNumber = booking.PhoneNumber,
                SpecialInstructions = booking.SpecialInstructions
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IndexTwo()
        {
            var groomersWithSessions = db.GroomingStaffs.Include("SparGroomings").ToList();
            return View("IndexTwo", groomersWithSessions);
        }

        // GET: GroomingStaffs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroomingStaff groomingStaff = db.GroomingStaffs.Find(id);
            if (groomingStaff == null)
            {
                return HttpNotFound();
            }
            return View(groomingStaff);
        }

        // GET: GroomingStaffs/Create
        public ActionResult Create()
        {
            return View(new GroomingStaff());
        }
        // In your GroomingStaffsController.cs

   
// POST: GroomingStaffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GroomStaffId,Groom_Name,Groom_Surname,Groom_Email")] GroomingStaff groomingStaff)
        {
            if (ModelState.IsValid)
            {
                db.GroomingStaffs.Add(groomingStaff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(groomingStaff);
        }

        // GET: GroomingStaffs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroomingStaff groomingStaff = db.GroomingStaffs.Find(id);
            if (groomingStaff == null)
            {
                return HttpNotFound();
            }
            return View(groomingStaff);
        }

        // POST: GroomingStaffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GroomStaffId,Groom_Name,Groom_Surname,Groom_Email")] GroomingStaff groomingStaff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(groomingStaff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(groomingStaff);
        }

        // GET: GroomingStaffs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroomingStaff groomingStaff = db.GroomingStaffs.Find(id);
            if (groomingStaff == null)
            {
                return HttpNotFound();
            }
            return View(groomingStaff);
        }

        // POST: GroomingStaffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GroomingStaff groomingStaff = db.GroomingStaffs.Find(id);
            db.GroomingStaffs.Remove(groomingStaff);
            db.SaveChanges();
            return RedirectToAction("Index");
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
