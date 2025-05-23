using Microsoft.AspNet.Identity;
using PetCare_system.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace PetCare_system.Controllers
{
    public class Vet_ConsultationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
       
        public ActionResult Index()
        {
            return View(db.Vet_Consultations.ToList());
        }
        [HttpGet]
        public ActionResult Index_Doc()
        {
            // Ensure a doctor is logged in
            if (Session["DoctorId"] != null)
            {
                int loggedDoctorId = (int)Session["DoctorId"];

                using (var db = new ApplicationDbContext())
                {
                    var consultations = db.Vet_Consultations
                        .Where(c => c.DoctorId == loggedDoctorId) // ✅ Filters consultations by logged-in doctor
                        .ToList();

                    return View(consultations);
                }
            }

            return RedirectToAction("Login", "Account"); // ✅ Redirects if no doctor is logged in
        }

        public ActionResult PhysicalForm(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                // Store Consult_Id in session
                Session["Consult_Id"] = id;

                var consultation = db.Vet_Consultations.Include("Pet")
                                     .FirstOrDefault(c => c.Consult_Id == id);

                if (consultation != null)
                {
                    ViewBag.DoctorId = consultation.DoctorId;
                    ViewBag.PetName = consultation.PetName;
                    ViewBag.PetType = consultation.PetType;
                    ViewBag.PetBreed = consultation.PetBreed;
                    ViewBag.PetDateOfBirth = consultation.PetDateOfBirth;
                    ViewBag.PetPicturePath = consultation.PetPicturePath;
                }

                // Load available medication names
                ViewBag.Medications = db.Inventory.Select(i => i.MedicationName).ToList();
            }

            return View();
        }



        [HttpPost]
        public ActionResult SubmitPhysicalForm(FormCollection form)
        {
            // Retrieve Consult_Id from Session
            int consultId = Convert.ToInt32(Session["Consult_Id"]);

            using (var db = new ApplicationDbContext())
            {
                // Find the consultation using Consult_Id
                var consultation = db.Vet_Consultations.FirstOrDefault(c => c.Consult_Id == consultId);

                if (consultation != null)
                {
                    // Update associated doctor to Available
                    var doctor = db.Doctors.FirstOrDefault(d => d.Id == consultation.DoctorId);
                    if (doctor != null)
                    {
                        doctor.AvailabilityStatus = "Available";
                    }

                    // Set DoctorId in consultation to "00"
                    consultation.DoctorId = null;
                }

                // Process medication inventory updates
                var medications = form.GetValues("MedicationName");
                var quantities = form.GetValues("MedicationQuantity");

                if (medications != null && quantities != null)
                {
                    for (int index = 0; index < medications.Length; index++)
                    {
                        string medName = medications[index];
                        int quantity = int.Parse(quantities[index]);

                        var inventoryItem = db.Inventory.FirstOrDefault(i => i.MedicationName == medName);
                        if (inventoryItem != null && inventoryItem.InventoryQuantity >= quantity)
                        {
                            inventoryItem.InventoryQuantity -= quantity;
                        }
                        else
                        {
                            // Handle insufficient inventory (optional)
                        }
                    }
                }

                db.SaveChanges(); // Save all changes
            }

            Session.Remove("Consult_Id");
            return RedirectToAction("SuccessPage");
        }


        public ActionResult SuccessPage()
        {
            return View();
        }
        public ActionResult Details(int id)
        {
            var consultation = db.Vet_Consultations.FirstOrDefault(c => c.Consult_Id == id);
            if (consultation == null)
            {
                return HttpNotFound();
            }
            return View(consultation);
        }


        [HttpGet]
        public ActionResult Create()
        {
            using (var db = new ApplicationDbContext())
            {
                string userId = User.Identity.GetUserId();
                var userPets = db.pets.Where(p => p.UserId == userId).ToList();

                if (!userPets.Any())
                {
                    TempData["ErrorMessage"] = "You must register a pet before booking a vet consultation.";
                    return RedirectToAction("Register", "Pet");
                }

                // ✅ Ensure ViewBag.PetId is always a SelectList
                ViewBag.PetId = new SelectList(userPets, "Id", "Name");
                return View(new Vet_Consultations());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Vet_Consultations vet_Consultations, string consultationType)
        {
            using (var db = new ApplicationDbContext())
            {
                if (ModelState.IsValid)
                {
                    var pet = db.pets.FirstOrDefault(p => p.Id == vet_Consultations.PetId);
                    if (pet != null)
                    {
                        // Set pet details
                        vet_Consultations.PetId = pet.Id;
                        vet_Consultations.PetName = pet.Name;
                        vet_Consultations.PetType = pet.Type;
                        vet_Consultations.PetBreed = pet.Breed;
                        vet_Consultations.PetDateOfBirth = pet.DateOfBirth;
                        vet_Consultations.PetPicturePath = pet.PicturePath;
                        vet_Consultations.ConsultationType = consultationType;
                        vet_Consultations.Feedback = "Pending";


                        // Find the first available doctor
                        var availableDoctor = db.Doctors.FirstOrDefault(d => d.AvailabilityStatus == "Availiable");

                        if (availableDoctor != null)
                        {
                            // Set doctor's availability to Unavailable
                            availableDoctor.AvailabilityStatus = "Unavailable";
                            db.Entry(availableDoctor).State = EntityState.Modified;

                            // Assign doctor to consultation
                            vet_Consultations.DoctorId = availableDoctor.Id;
                            vet_Consultations.DoctorAvailability = true; // Indicates that a doctor *was* assigned
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "No available doctors at the moment. Please try again later.";
                            return RedirectToAction("Create");
                        }




                        db.Vet_Consultations.Add(vet_Consultations);
                        db.SaveChanges();

                        // ✅ Send confirmation email
                        string emailFrom = "shezielihle186@gmail.com";
                        string emailPassword = "xjop iuut owdu loav"; // Store securely

                        try
                        {
                            using (MailMessage mm = new MailMessage(emailFrom, User.Identity.Name))
                            {
                                mm.Subject = "Vet Consultation Scheduled";
                                mm.Body = $"Dear Pet Owner,\n\nYour vet consultation has been scheduled.\n\n" +
                                          $"Pet Name: {vet_Consultations.PetName}\n" +
                                          $"Consultation Type: {vet_Consultations.ConsultationType}\n" +
                                          
                                          $"Consultation ID: {vet_Consultations.Consult_Id}\n" +
                                          "Please proceed to the payment page.\n\nBest regards,\nPetCare Systems";

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
                            TempData["Error"] = "Consultation created, but the email could not be sent. Error: " + ex.Message;
                        }

                        return RedirectToAction("Create", "Payment", new { consultId = vet_Consultations.Consult_Id });
                    }
                    ModelState.AddModelError("", "Selected pet does not exist.");
                }

                // ✅ Ensure ViewBag.PetId remains a SelectList for dropdown to work properly
                string userIdForDropDown = User.Identity.GetUserId();
                ViewBag.PetId = new SelectList(db.pets.Where(p => p.UserId == userIdForDropDown).ToList(), "Id", "Name");
                return View(vet_Consultations);
            }
        }


        // ✅ Helper method to reload the view with correct dropdown
        private ActionResult ReloadView(Vet_Consultations vet_Consultations, ApplicationDbContext db)
        {
            string userId = User.Identity.GetUserId();
            ViewBag.PetId = new SelectList(db.pets.Where(p => p.UserId == userId).ToList(), "Id", "Name");
            return View(vet_Consultations);
        }

        // ✅ Helper method to send the confirmation email
        private void SendConfirmationEmail(Vet_Consultations vet_Consultations, string doctorName)
        {
            string emailFrom = "shezielihle186@gmail.com";
            string emailPassword = "xjop iuut owdu loav"; // Store securely

            try
            {
                using (MailMessage mm = new MailMessage(emailFrom, User.Identity.Name))
                {
                    mm.Subject = "Vet Consultation Scheduled";
                    mm.Body = $"Dear Pet Owner,\n\nYour vet consultation has been scheduled.\n\n" +
                              $"Pet Name: {vet_Consultations.PetName}\n" +
                              $"Consultation Type: {vet_Consultations.ConsultationType}\n" +
                              $"Doctor Assigned: {doctorName}\n" +
                              $"Consultation ID: {vet_Consultations.Consult_Id}\n" +
                              "Please proceed to the payment page.\n\nBest regards,\nPetCare Systems";

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
                TempData["Error"] = "Consultation created, but the email could not be sent. Error: " + ex.Message;
            }
        }


        // GET: Provide Feedback
        public ActionResult ProvideFeedback(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Retrieve the consultation using the id
            Vet_Consultations vet_Consultations = db.Vet_Consultations.Find(id);

            // Check if the consultation exists
            if (vet_Consultations == null)
            {
                return HttpNotFound();
            }

            // Return the consultation to the view for feedback
            return View(vet_Consultations);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProvideFeedback(int Consult_Id, string feedback, bool DoctorAvailability)
        {
            if (Consult_Id == 0)
            {
                Debug.WriteLine("Consultation ID is missing.");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var consultation = db.Vet_Consultations.Find(Consult_Id);
            if (consultation == null)
            {
                Debug.WriteLine("Consultation not found in database.");
                return HttpNotFound();
            }

            Debug.WriteLine($"Updating Consultation ID: {consultation.Consult_Id}");
            Debug.WriteLine($"New Feedback: {feedback}");
            Debug.WriteLine($"New Availability: {DoctorAvailability}");

            // Update values
            consultation.Feedback = feedback;
            consultation.DoctorAvailability = DoctorAvailability; // 🔥 Ensure correct update

            try
            {
                db.Entry(consultation).State = EntityState.Modified;
                db.SaveChanges();
                Debug.WriteLine("Feedback successfully saved.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving changes: {ex.Message}");
            }

            return RedirectToAction("Index");
        }


        public ActionResult Detail()
        {
            var userId = User.Identity.GetUserId();
            var consultations = db.Vet_Consultations.Where(c => c.Pet.UserId == userId).ToList();
            return View(consultations);
        }
    }
}
