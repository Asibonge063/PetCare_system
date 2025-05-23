using PetCare_system.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetCare_system.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Dashboard()
        {
            ViewBag.Message = "Your contact page.";
            TempData["BookingCount"] = db.Vet_Consultations.Count();
            TempData["PaymentCount"] = db.paymentForBoards.Count();
            TempData["PetCount"] = db.pets.Count();


            return View();
        }
    }
}