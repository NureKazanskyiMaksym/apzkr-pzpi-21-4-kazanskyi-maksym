using EquipmentWatcher.Models;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace EquipmentWatcher
{
    public static class PdfReportUtility
    {
        public static MemoryStream GenerateReport(string reportTitle, string issuer, object inputModel, IEnumerable<object> data)
        {
            var inputModelType = inputModel.GetType();
            var inputProperties = inputModelType.GetProperties();
            var activeColumns = inputProperties.Where(p => p.Name.StartsWith("Is") && (bool)p.GetValue(inputModel))
                .Select(p => p.Name.Substring(2))
                .ToArray();
            
            if (!activeColumns.Any())
            {
                throw new InvalidOperationException("No columns selected.");
            }

            var filters = inputProperties.Where(p => p.Name.EndsWith("Filter"))
                .ToDictionary(p => p.Name, p =>
                {
                    if (p.DeclaringType == typeof(DateTime))
                    {
                        return ((DateTime)p.GetValue(inputModel)).ToString("u");
                    }
                    else
                    {
                        return p.GetValue(inputModel)?.ToString();
                    }
                });

            var resultData = data.Select(
                d => d.GetType()
                .GetProperties()
                .Where(p => activeColumns.Contains(p.Name))
                .Select(p => p.GetValue(d)?.ToString() ?? "null")
                .ToArray())
                .ToArray();

            var stream = new MemoryStream();
            var writer = new PdfWriter(stream);
            var pdfDocument = new PdfDocument(writer.SetSmartMode(true));
            pdfDocument.AddNewPage();

            var headerFont = PdfFontFactory.CreateFont(StandardFonts.COURIER_BOLD);
            var header = new Paragraph(reportTitle)
                .SetFont(headerFont)
                .SetFontSize(16)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

            var sb = new StringBuilder();
            sb.AppendLine("Generated at: " + DateTimeOffset.Now.ToString("o"));
            sb.AppendLine("Issuer: " + issuer);
            sb.Append("Raw filters:");

            if (!filters.Any(x => x.Value != null))
            {
                sb.AppendLine(" none.");
            }
            else
            {
                sb.AppendLine();
                foreach (var filter in filters)
                {
                    sb.AppendLine($"{filter.Key}: {filter.Value}");
                }
            }

            var info = new Paragraph(sb.ToString())
                .SetFont(headerFont)
                .SetFontSize(12)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT);

            using (var document = new Document(pdfDocument))
            {
                document.Add(header);
                document.Add(info);

                var table = new Table(new float[activeColumns.Length]);

                foreach (var column in activeColumns)
                {
                    table.AddCell(new Cell().Add(new Paragraph(column).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                }

                foreach (var row in resultData)
                {
                    foreach (var cell in row)
                    {
                        table.AddCell(new Cell().Add(new Paragraph(cell).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    }
                }

                document.Add(table);
            }

            pdfDocument.Close();

            return stream;
        }

        public static MemoryStream GenerateReport(string reportTitle, Account issuer, object inputModel, IEnumerable<object> data)
        {
            return GenerateReport(reportTitle, $"{issuer.Person.FirstName} {issuer.Person.MiddleName}{(issuer.Person.MiddleName == "" ? "" : " ")}{issuer.Person.LastName}", inputModel, data);
        }

        public static MemoryStream GenerateReport(this PageModel pageModel, DbApp dbApp, object inputModel)
        {
            var inputModelType = inputModel.GetType();
            var inputProperties = inputModelType.GetProperties();
            var activeColumns = inputProperties.Where(p => p.Name.StartsWith("Is") && (bool)p.GetValue(inputModel))
                .Select(p => p.Name.Substring(2))
                .ToArray();

            if (!activeColumns.Any())
            {
                throw new InvalidOperationException("No columns selected.");
            }

            var user = dbApp.Accounts.Include(x => x.Person)
                .First(x => x.Login == pageModel.User.Identity.Name);
            
            var modelStr = pageModel.GetType().Namespace.Split('.').Last();

            var reportTitle = modelStr + " Report";

            StringBuilder sqlQueryBuilder = new StringBuilder($"SELECT * FROM {modelStr} WHERE 1=1");

            var filters = inputProperties.Where(p => p.Name.EndsWith("Filter"))
                .Where(p => p.GetValue(inputModel) != null)
                .ToDictionary(p => p.Name.Substring(0, p.Name.Length - "Filter".Length), p =>
                {
                    var value = p.GetValue(inputModel);
                    if (value.GetType() == typeof(DateTime))
                    {
                        if (p.Name.EndsWith("AfterFilter"))
                            sqlQueryBuilder.Append($" AND {p.Name.Substring(0, p.Name.Length - "AfterFilter".Length)} > '{((DateTime)value).ToString("u")}'");
                        else if (p.Name.EndsWith("BeforeFilter"))
                            sqlQueryBuilder.Append($" AND {p.Name.Substring(0, p.Name.Length - "BeforeFilter".Length)} < '{((DateTime)value).ToString("u")}'");
                        else
                            sqlQueryBuilder.Append($" AND {p.Name.Substring(0, p.Name.Length - 6)} = '{((DateTime)value).ToString("u")}'");
                        return ((DateTime)value).ToString("u");
                    }
                    else
                    {
                        sqlQueryBuilder.Append($" AND {p.Name.Substring(0, p.Name.Length - 6)} IN (SELECT value FROM STRING_SPLIT('{value?.ToString()}', ','))");
                        return value?.ToString();
                    }
                });

            List<List<string>> resultData = new List<List<string>>();

            using (SqlConnection connection = new SqlConnection(dbApp.Database.GetConnectionString()))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlQueryBuilder.ToString(), connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new List<string>();
                            foreach (var column in activeColumns)
                            {
                                row.Add(reader[column].ToString() ?? "null");
                            }
                            resultData.Add(row);
                        }
                    }
                }
            }

            var stream = new MemoryStream();
            var writer = new PdfWriter(stream);
            var pdfDocument = new PdfDocument(writer.SetSmartMode(true));
            pdfDocument.AddNewPage();

            var headerFont = PdfFontFactory.CreateFont(StandardFonts.COURIER_BOLD);
            var header = new Paragraph(reportTitle)
                .SetFont(headerFont)
                .SetFontSize(16)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

            var sb = new StringBuilder();
            sb.AppendLine("Generated at: " + DateTimeOffset.Now.ToString("o"));
            sb.AppendLine("Issuer: " + $"{user.Person.FirstName} {user.Person.MiddleName}{(user.Person.MiddleName == "" ? "" : " ")}{user.Person.LastName}");
            sb.Append("Raw filters:");

            if (!filters.Any(x => x.Value != null))
            {
                sb.AppendLine(" none.");
            }
            else
            {
                sb.AppendLine();
                foreach (var filter in filters)
                {
                    sb.AppendLine($"{filter.Key}: {filter.Value}");
                }
            }

            var info = new Paragraph(sb.ToString())
                .SetFont(headerFont)
                .SetFontSize(12)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT);

            using (var document = new Document(pdfDocument))
            {
                document.Add(header);
                document.Add(info);

                var table = new Table(new float[activeColumns.Length]);

                foreach (var column in activeColumns)
                {
                    table.AddCell(new Cell().Add(new Paragraph(column).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                }

                foreach (var row in resultData)
                {
                    foreach (var cell in row)
                    {
                        table.AddCell(new Cell().Add(new Paragraph(cell).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));
                    }
                }

                document.Add(table);
            }

            pdfDocument.Close();

            return stream;
        }
    }
}
