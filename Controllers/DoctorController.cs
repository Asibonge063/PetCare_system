using Microsoft.AspNet.Identity;
using PetCare_system.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.Net;

namespace PetCare_system.Controllers
{

    public class DoctorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Doctor/AddDoctor
        public ActionResult AddDoctor()
        {
            return View(new Doctor());
        }
        public ActionResult Dashboard_Doc()
        {
            // Check if Doctor session exists
            if (Session["DoctorId"] == null)
            {
                // Redirect to login if session does not exist
                return RedirectToAction("Login", "Account");
            }

            int doctorId = Convert.ToInt32(Session["DoctorId"]);
            using (var db = new ApplicationDbContext())
            {
                var doctor = db.Doctors.FirstOrDefault(d => d.Id == doctorId);

                if (doctor != null)
                {
                    // You can return doctor information to the view if needed
                    return View(doctor);  // Pass doctor info to the dashboard view
                }
                else
                {
                    // If doctor not found, show error page
                    return View("Error");
                }
            }
        }


        // POST: Doctor/AddDoctor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDoctor(Doctor model)
        {
            if (ModelState.IsValid)
            {
                model.AvailabilityStatus = "Availiable";
                // Hash the doctor's password
                var passwordHasher = new PasswordHasher();
                model.PasswordHash = passwordHasher.HashPassword(model.Password);

                // Add the doctor to the database
                db.Doctors.Add(model);
                db.SaveChanges();

                // Email credentials
                string emailFrom = "shezielihle186@gmail.com";
                string emailPassword = "xjop iuut owdu loav"; // Replace with a secure storage for credentials

                // Send email to the doctor
                try
                {
                    using (MailMessage mm = new MailMessage(emailFrom, model.Email))
                    {
                        mm.Subject = "Welcome to PetCare Systems";
                        mm.Body = $"Dear {model.FullName},\n\n" +
                                  "Your account has been created successfully.\n\n" +
                                  "Login Details:\n" +
                                  $"Email: {model.Email}\n" +
                                  $"Password: {model.Password}\n\n" +
                                  "Please change your password after logging in.\n\n" +
                                  "Best regards,\n" +
                                  "PetCare Systems";

                        mm.IsBodyHtml = false;

                        using (SmtpClient smtp = new SmtpClient())
                        {
                            smtp.Host = "smtp.gmail.com"; // Gmail SMTP server
                            smtp.EnableSsl = true;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new NetworkCredential(emailFrom, emailPassword);
                            smtp.Port = 587; // Standard Gmail SMTP port

                            smtp.Send(mm);
                        }
                    }

                    // Confirmation feedback
                    TempData["Success"] = "Doctor added successfully! Login details sent via email.";
                }
                catch (SmtpException ex)
                {
                    // Handle email sending errors
                    TempData["Error"] = "Doctor was added, but the email could not be sent. Error: " + ex.Message;
                }
            }
            else
            {
                // Handle validation errors
                return View(model);
            }

            return RedirectToAction("DoctorList");
        }

        //// Method to send email
        //private void SendEmail(string toEmail, string subject, string body)
        //{
        //    try
        //    {
        //        MailMessage mail = new MailMessage();
        //        mail.To.Add(toEmail);
        //        mail.From = new MailAddress("admin@petcaresystems.com"); // Change to your email
        //        mail.Subject = subject;
        //        mail.Body = body;
        //        mail.IsBodyHtml = false;

        //        SmtpClient smtp = new SmtpClient();
        //        smtp.Host = "smtp.yourmailserver.com"; // Replace with your SMTP host
        //        smtp.Port = 587;
        //        smtp.Credentials = new System.Net.NetworkCredential("your-email", "your-password"); // Update credentials
        //        smtp.EnableSsl = true;
        //        smtp.Send(mail);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Email sending failed: " + ex.Message);
        //    }
        //}

        // GET: Doctor/DoctorList
        public ActionResult DoctorList()
        {
            var doctors = db.Doctors.ToList();
            return View(doctors);
        }


        public ActionResult ExportDoctors()
        {
            var doctors = db.Doctors.ToList(); // Get your data from database

            // Create DataTable
            DataTable dt = new DataTable("Doctors");
            dt.Columns.AddRange(new DataColumn[4] {
        new DataColumn("Full Name"),
        new DataColumn("Email"),
        new DataColumn("Phone"),
        new DataColumn("Specialization")
    });

            foreach (var doctor in doctors)
            {
                dt.Rows.Add(doctor.FullName, doctor.Email, doctor.PhoneNumber, doctor.Specialization);
            }

            // Export to Excel
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Doctors");
                worksheet.Cells.LoadFromDataTable(dt, true);

                var date = DateTime.Now.ToString("yyyyMMdd");
                var fileName = $"Doctors_{date}.xlsx";

                var memoryStream = new MemoryStream();
                package.SaveAs(memoryStream);
                memoryStream.Position = 0;

                return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

    }
}