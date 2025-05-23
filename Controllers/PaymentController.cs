using PetCare_system.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace PetCare_system.Controllers
{
    public class PaymentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Payment/Create
        public ActionResult Create(int consultId)
        {
            try
            {
                var consultation = db.Vet_Consultations
                    .Include(c => c.Pet)  // Eager load Pet data
                    .FirstOrDefault(c => c.Consult_Id == consultId);

                if (consultation == null)
                {
                    return HttpNotFound("Consultation not found");
                }

                // Check if payment already exists
                var existingPayment = db.Payments.FirstOrDefault(p => p.ConsultId == consultId);
                if (existingPayment != null)
                {
                    return RedirectToAction("PaymentSuccess", new { paymentId = existingPayment.PaymentId });
                }

                var payment = new Payment
                {
                    ConsultId = consultId,
                    PaymentDate = DateTime.Now,
                    AmountPaid = 500m, // Fixed amount
                    VetConsultation = consultation  // Set navigation property
                };

                // Add consultation details to ViewBag for display
                ViewBag.PetName = consultation.Pet?.Name ?? "N/A";
                ViewBag.ConsultationDate = consultation.Consult_Date.ToString("d");
                ViewBag.ConsultationTime = consultation.Consult_Time.ToString("t");
                ViewBag.ConsultationType= consultation.ConsultationType;

                return View(payment);
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Trace.TraceError(ex.Message);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Payment payment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Verify consultation exists
                    var consultation = db.Vet_Consultations.Find(payment.ConsultId);
                    if (consultation == null)
                    {
                        ModelState.AddModelError("", "Invalid consultation reference");
                        return View(payment);
                    }

                    // Process the payment
                    payment.ProcessPayment(); // Sets PaymentStatus based on AmountPaid
                    payment.PaymentDate = DateTime.Now;

                    // Generate transaction reference
                    payment.TransactionReference = $"PETPAY-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";

                    db.Payments.Add(payment);
                    db.SaveChanges();

                    // Update consultation status
                    consultation.PaymentStatus = payment.PaymentStatus;
                    db.Entry(consultation).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("PaymentSuccess", new { paymentId = payment.PaymentId });
                }

                // If we got this far, something failed; redisplay form
                return View(payment);
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Trace.TraceError(ex.Message);
                ModelState.AddModelError("", "An error occurred while processing your payment. Please try again.");
                return View(payment);
            }
        }

        public ActionResult PaymentSuccess(int paymentId)
        {
            try
            {
                var payment = db.Payments
                    .Include(p => p.VetConsultation)
                    .Include(p => p.VetConsultation.Pet)
                    .FirstOrDefault(p => p.PaymentId == paymentId);

                if (payment == null)
                {
                    return HttpNotFound("Payment not found");
                }

                return View(payment);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
                return View("Error");
            }
        }

        public ActionResult DownloadReceipt(int paymentId)
        {
            try
            {
                var payment = db.Payments
                    .Include(p => p.VetConsultation)
                    .Include(p => p.VetConsultation.Pet)
                    .FirstOrDefault(p => p.PaymentId == paymentId);

                if (payment == null)
                {
                    return HttpNotFound();
                }

                var pdfService = new PdfReceiptService();
                var pdfBytes = pdfService.GenerateReceipt(payment);

                return File(pdfBytes, "application/pdf", $"PetCare_Receipt_{paymentId}.pdf");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
                return RedirectToAction("PaymentSuccess", new { paymentId, error = "Failed to generate receipt" });
            }
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