using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PetCare_system.Models;

namespace PetCare_system.Controllers
{
    [Authorize]
    public class DayCareController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        // GET: DayCare
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var dayCares = _context.DayCares
                .Include(d => d.Pet)
                .Include(d => d.Membership)
                .Include(d => d.User)
                .Where(d => d.UserId == userId)
                .OrderByDescending(d => d.CheckInDate)
                .ToList();

            return View(dayCares);
        }

        // GET: DayCare/CheckIn
        public ActionResult CheckIn()
        {
            var userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var user = _context.Users.Find(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Owner = user;

            var activeMemberships = _context.Memberships
                .Include(m => m.Pet)
                .Where(m => m.UserId == userId &&
                            m.Status == "Active" &&
                            m.EndDate >= DateTime.Today &&
                            m.Pet != null)
                .ToList();

            if (!activeMemberships.Any())
            {
                TempData["ErrorMessage"] = "No active membership with a valid pet found.";
                return RedirectToAction("Index", "Membership");
            }

            ViewBag.MembershipId = new SelectList(activeMemberships, "Id", "Pet.Name");
            PopulateCareTypes(activeMemberships.First().MembershipType);

            return View(new DayCare
            {
                CheckInDate = DateTime.Now,
                CareType = activeMemberships.First().MembershipType
            });
        }

        // POST: DayCare/CheckIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckIn(DayCare model)
        {
            var userId = User.Identity.GetUserId();
            var user = _context.Users.Find(userId);
            ViewBag.Owner = user;

            var activeMemberships = _context.Memberships
                .Include(m => m.Pet)
                .Where(m => m.UserId == userId &&
                            m.Status == "Active" &&
                            m.EndDate >= DateTime.Today &&
                            m.Pet != null)
                .ToList();

            var membership = activeMemberships.FirstOrDefault(m => m.Id == model.MembershipId);

            if (membership == null)
            {
                ModelState.AddModelError("MembershipId", "Invalid pet selection.");
                return View(model);
            }

            ViewBag.MembershipId = new SelectList(activeMemberships, "Id", "Pet.Name");
            PopulateCareTypes(activeMemberships.FirstOrDefault()?.MembershipType ?? "");

            // Prevent duplicate check-ins
            var existingCheckIn = _context.DayCares
                .Include(d => d.Pet)
                .FirstOrDefault(d => d.PetId == membership.PetId && d.CheckOutDate == null);

            if (existingCheckIn != null)
            {
                ModelState.AddModelError("",
                    $"⛔ {membership.Pet.Name} is already checked in!\n" +
                    $"Checked in at: {existingCheckIn.CheckInDate:MMM dd, yyyy hh:mm tt}\n" +
                    $"Special instructions: {existingCheckIn.SpecialInstructions}");
                return View(model);
            }

            model.PetId = membership.PetId;
            model.UserId = userId;
            model.CareType = membership.MembershipType;

            _context.DayCares.Add(model);
            _context.SaveChanges();

            TempData["SuccessMessage"] = $"{membership.Pet.Name} checked in successfully!";
            return RedirectToAction("Details", new { id = model.Id });
        }

        // GET: DayCare/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var userId = User.Identity.GetUserId();
            var dayCare = _context.DayCares
                .Include(d => d.Pet)
                .Include(d => d.Membership)
                .Include(d => d.User)
                .FirstOrDefault(d => d.Id == id && d.UserId == userId);

            if (dayCare == null)
                return HttpNotFound();

            return View(dayCare);
        }

        // GET: DayCare/CheckOut/5
        public ActionResult CheckOut(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            var dayCare = _context.DayCares
                .Include(d => d.Pet)
                .Include(d => d.Membership)
                .FirstOrDefault(d => d.Id == id && d.UserId == userId && d.CheckOutDate == null);

            if (dayCare == null)
            {
                TempData["ErrorMessage"] = "Day care record not found or already checked out.";
                return RedirectToAction("Index");
            }

            return View(dayCare);
        }

        // POST: DayCare/CheckOut/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(int id)
        {
            var userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            var dayCare = _context.DayCares
                .Include(d => d.Pet)
                .FirstOrDefault(d => d.Id == id && d.UserId == userId && d.CheckOutDate == null);

            if (dayCare == null)
            {
                TempData["ErrorMessage"] = "Day care record not found or already checked out.";
                return RedirectToAction("Index");
            }

            dayCare.CheckOutDate = DateTime.Now;

            try
            {
                _context.SaveChanges();
                TempData["SuccessMessage"] = $"{dayCare.Pet.Name} checked out successfully!";
                return RedirectToAction("Details", new { id = dayCare.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while checking out: " + ex.Message);
                return View(dayCare);
            }
        }

        // Populate care types dropdown
        private void PopulateCareTypes(string selectedType)
        {
            var careTypes = new List<string> { "Basic", "Premium", "VIP" };
            ViewBag.CareType = new SelectList(careTypes, selectedType);
        }

        // Dispose context
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
