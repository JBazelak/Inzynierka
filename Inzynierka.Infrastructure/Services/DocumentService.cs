using Inzynierka.Core.Entities;
using Inzynierka.Infrastructure.Utils;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System.Globalization;

namespace Inzynierka.Infrastructure.Services
{
    public static class DocumentService
    {

        public static Document Create(Project project)
        {
            return Document.Create(container =>
            {
                // Strona główna z informacjami o projekcie
                container.Page(page =>
                {

                    page.Margin(40);
                    page.PageColor(QuestPDF.Infrastructure.Color.FromHex("eeeeee"));

                    page.Header().PaddingBottom(70).AlignCenter().Text($"Raport finansowy: {project.Name}")
                        .FontSize(20)
                        .Bold()
                        .Style(TypoGraphy.Normal);
                    

                    page.Content().Column(stack =>
                    {
                        stack.Spacing(5);

                        stack.Item().Text($"{project.Contractor.FirstName}\n" +
                            $"{project.Contractor.CompanyName}\n" +
                            $"{project.Contractor.Address}\n" +
                            $"{project.Contractor.PhoneNumber}\n" +
                            $"{project.Contractor.Email}\n" +
                            $"NIP: {project.Contractor.TaxIdNumber}\n" +
                            $"REGON: {project.Contractor.NationalBusinessRegistryNumber}");

                        stack.Item().PaddingTop(10).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(1);
                            });

                            // Nagłówki z tłem i obramowaniem
                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Darken2)
                                    .Border(1)
                                    .BorderColor(Colors.Grey.Darken1)
                                    .Text("Nazwa").Bold().AlignCenter();

                                header.Cell().Background(Colors.Grey.Darken2)
                                    .Border(1)
                                    .BorderColor(Colors.Grey.Darken1)
                                    .Text("Ilość").Bold().AlignCenter();

                                header.Cell().Background(Colors.Grey.Darken2)
                                    .Border(1)
                                    .BorderColor(Colors.Grey.Darken1)
                                    .Text("Jednostka").Bold().AlignCenter();

                                header.Cell().Background(Colors.Grey.Darken2)
                                    .Border(1)
                                    .BorderColor(Colors.Grey.Darken1)
                                    .Text("Cena za jednostkę").Bold().AlignCenter();

                                header.Cell().Background(Colors.Grey.Darken2)
                                    .Border(1)
                                    .BorderColor(Colors.Grey.Darken1)
                                    .Text("Koszt").Bold().AlignCenter();
                            });

                            // Wiersze z danymi
                            foreach (var material in project.Materials)
                            {
                                table.Cell().Background(Colors.Grey.Lighten5)
                                    .Border(1)
                                    .BorderColor(Colors.Grey.Darken1)
                                    .Padding(2)
                                    .Text(material.Name).AlignLeft();

                                table.Cell().Background(Colors.Grey.Lighten5)
                                    .Border(1)
                                    .BorderColor(Colors.Grey.Darken1)
                                    .Padding(2)
                                    .Text($"{material.Quantity}").AlignCenter();

                                table.Cell().Background(Colors.Grey.Lighten5)
                                    .Border(1)
                                    .BorderColor(Colors.Grey.Darken1)
                                    .Padding(2)
                                    .Text(material.Unit).AlignCenter();

                                table.Cell().Background(Colors.Grey.Lighten5)
                                    .Border(1)
                                    .BorderColor(Colors.Grey.Darken1)
                                    .Padding(2)
                                    .Text($"{material.PricePerUnit.ToString("C", new CultureInfo("pl-PL"))}").AlignRight();

                                table.Cell().Background(Colors.Grey.Lighten5)
                                    .Border(1)
                                    .BorderColor(Colors.Grey.Darken1)
                                    .Padding(2)
                                    .Text($"{material.TotalCost.ToString("C", new CultureInfo("pl-PL"))}").AlignRight();
                            }
                        });


                        stack.Item().AlignRight().Text($"Koszt projektu: {project.Materials.Sum(m => m.TotalCost).ToString("C", new CultureInfo("pl-PL"))}");
                    });

                    page.Footer().Background(Colors.Grey.Darken2).AlignCenter().Text(text =>
                    {
                        text.Span("Wygenerowano przez Material Manager");
                        text.Span(" | ");
                        text.Span($"Date: {System.DateTime.Now:yyyy-MM-dd}");
                    });
                });

                // Obsługa załączników
                foreach (var material in project.Materials.Where(m => !string.IsNullOrEmpty(m.AttachmentPath)))
                {
                    var attachmentPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", material.AttachmentPath.TrimStart('/'));

                    if (Path.GetExtension(attachmentPath).ToLower() == ".pdf")
                    {
                        AttachmentService.AddPdfAttachment(container, attachmentPath, material.Name);
                    }
                    else
                    {
                        AttachmentService.AddImageAttachment(container, attachmentPath, material.Name);
                    }
                }
            });
        }
    }
}
