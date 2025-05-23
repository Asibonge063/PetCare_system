//using System;
//using System.Linq;
//using QuestPDF.Fluent;
//using QuestPDF.Helpers;
//using QuestPDF.Infrastructure;
//using PetCare_system.Models;

//namespace PetCare_system.Services
//{
//    public class PdfPrescriptionService
//    {
//        public byte[] GeneratePrescription(Vet_Consultations consultation)
//        {
//            QuestPDF.Settings.License = LicenseType.Community;

//            var document = Document.Create(container =>
//            {
//                container.Page(page =>
//                {
//                    page.Size(PageSizes.A4);
//                    page.Margin(2, Unit.Centimetre);
//                    page.PageColor(Colors.White);
//                    page.DefaultTextStyle(x => x.FontSize(12));

//                    page.Header()
//                        .AlignCenter()
//                        .Text("Prescription")
//                        .SemiBold().FontSize(24).FontColor(Colors.Blue.Medium);

//                    page.Content()
//                        .PaddingVertical(1, Unit.Centimetre)
//                        .Column(x =>
//                        {
//                            x.Spacing(20);

//                            x.Item().Text($"Consultation ID: {consultation.Consult_Id}");
//                            x.Item().Text($"Date: {consultation.Consult_Date:yyyy-MM-dd HH:mm}");
//                            x.Item().Text($"Pet Name: {consultation.PetName}");
//                            x.Item().Text($"Pet Type: {consultation.PetType}");
//                            x.Item().Text($"Pet Breed: {consultation.PetBreed}");
//                            x.Item().Text($"Prescribed Medications:");

//                            // Assuming prescriptions are stored in consultation or related model
//                            if (consultation. != null && consultation.Prescriptions.Any())
//                            {
//                                foreach (var prescription in consultation.Prescriptions)
//                                {
//                                    x.Item().Text($"- {prescription.MedicationName} - {prescription.Quantity} units");
//                                }
//                            }
//                            else
//                            {
//                                x.Item().Text("No prescriptions available.");
//                            }

//                            x.Item().PaddingTop(25).Text("Please follow the prescribed medication for your pet's health.");
//                        });

//                    page.Footer()
//                        .AlignCenter()
//                        .Text(x =>
//                        {
//                            x.Span("Page ");
//                            x.CurrentPageNumber();
//                        });
//                });
//            });

//            return document.GeneratePdf();
//        }
//    }
//}
