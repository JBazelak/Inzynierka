﻿using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Drawing;

namespace Inzynierka.Infrastructure.Services
{
    public static class AttachmentService
    {
        public static void AddPdfAttachment(IDocumentContainer container, string pdfPath, string attachmentName)
        {
            using (var pdfDocument = PdfiumViewer.PdfDocument.Load(pdfPath))
            {
                for (int pageNumber = 0; pageNumber < pdfDocument.PageCount; pageNumber++)
                {
                    container.Page(page =>
                    {
                        page.Margin(20);

                        page.Content().Element(content =>
                        {
                            using (var image = pdfDocument.Render(pageNumber, 150, 150, PdfiumViewer.PdfRenderFlags.Annotations))
                            {
                                using (var memoryStream = new MemoryStream())
                                {
                                    image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                                    content.Image(memoryStream.ToArray()).FitWidth();
                                }
                            }
                        });

                        page.Footer().AlignCenter().Text(text =>
                        {
                            text.Span($"Attachment: {attachmentName}");
                            text.Span(" | ");
                            text.Span("Generated by Inzynierka App");
                            text.Span(" | ");
                            text.Span($"Date: {System.DateTime.Now:yyyy-MM-dd}");
                        });
                    });
                }
            }
        }

        public static void AddImageAttachment(IDocumentContainer container, string imagePath, string attachmentName)
        {
            container.Page(page =>
            {
                page.Margin(40);

                page.Content().Element(content =>
                {
                    if (!File.Exists(imagePath))
                    {
                        content.Text($"Attachment for {attachmentName} not found.")
                            .Italic()
                            .FontColor(Colors.Red.Medium);
                        return;
                    }

                    var resizedImageBytes = ResizeImage(imagePath, 515, 742);
                    content.Image(resizedImageBytes).FitWidth();
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span($"Attachment: {attachmentName}");
                    text.Span(" | ");
                    text.Span("Generated by Inzynierka App");
                    text.Span(" | ");
                    text.Span($"Date: {System.DateTime.Now:yyyy-MM-dd}");
                });
            });
        }

        private static byte[] ResizeImage(string imagePath, int maxWidth, int maxHeight)
        {
            using var image = System.Drawing.Image.FromFile(imagePath);
            var scaleFactor = Math.Min((double)maxWidth / image.Width, (double)maxHeight / image.Height);

            var newWidth = (int)(image.Width * scaleFactor);
            var newHeight = (int)(image.Height * scaleFactor);

            using var resized = new Bitmap(image, new System.Drawing.Size(newWidth, newHeight));
            using var memoryStream = new MemoryStream();
            resized.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            return memoryStream.ToArray();
        }
    }
}
