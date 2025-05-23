using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;


namespace PetCare_system.Models
{
    // Services/PdfReceiptService.cs

    public class PdfReceiptService
    {
        public byte[] GenerateReceipt(Payment payment)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .AlignCenter()
                        .Text("Payment Receipt")
                        .SemiBold().FontSize(24).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(20);

                            x.Item().Text($"Receipt #: {payment.PaymentId}");
                            x.Item().Text($"Date: {payment.PaymentDate:yyyy-MM-dd HH:mm}");
                            x.Item().Text($"Amount: R {payment.AmountPaid:0.00}");
                            x.Item().Text($"Consultation ID: {payment.ConsultId}");

                            x.Item().PaddingTop(25).Text("Thank you for your payment!");
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            });

            return document.GeneratePdf();
        }
    }  }