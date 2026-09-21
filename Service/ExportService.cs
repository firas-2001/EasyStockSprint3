using System.Text;
using ClosedXML.Excel;
using EasyStock.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace EasyStock.Service
{
    public class ExportService
    {
        public byte[] ExportCsv(ReportDataset dataset)
        {
            var builder = new StringBuilder();
            builder.AppendLine(string.Join(';', dataset.Headers.Select(EscapeCsv)));
            foreach (var row in dataset.Rows)
            {
                builder.AppendLine(string.Join(';', row.Select(EscapeCsv)));
            }

            return Encoding.UTF8.GetBytes(builder.ToString());
        }

        public byte[] ExportExcel(ReportDataset dataset)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(dataset.WorksheetName);

            for (var i = 0; i < dataset.Headers.Count; i++)
            {
                worksheet.Cell(1, i + 1).Value = dataset.Headers[i];
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            }

            for (var rowIndex = 0; rowIndex < dataset.Rows.Count; rowIndex++)
            {
                for (var colIndex = 0; colIndex < dataset.Rows[rowIndex].Count; colIndex++)
                {
                    worksheet.Cell(rowIndex + 2, colIndex + 1).Value = dataset.Rows[rowIndex][colIndex];
                }
            }

            worksheet.Columns().AdjustToContents();
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public byte[] ExportPdf(ReportDataset dataset)
        {
            using var stream = new MemoryStream();
            using var writer = new PdfWriter(stream);
            using var pdf = new PdfDocument(writer);
            using var document = new Document(pdf, iText.Kernel.Geom.PageSize.A4.Rotate());

            document.Add(new Paragraph(dataset.Title)
                .SetFontSize(16)
                
                .SetTextAlignment(TextAlignment.CENTER));
            document.Add(new Paragraph($"Généré le {DateTime.Now:yyyy-MM-dd HH:mm}")
                .SetFontSize(9)
                .SetTextAlignment(TextAlignment.RIGHT));

            var table = new Table(dataset.Headers.Count).UseAllAvailableWidth();
            foreach (var header in dataset.Headers)
            {
                table.AddHeaderCell(new Cell().Add(new Paragraph(header)));
            }

            foreach (var row in dataset.Rows)
            {
                foreach (var value in row)
                {
                    table.AddCell(new Cell().Add(new Paragraph(value ?? string.Empty).SetFontSize(8)));
                }
            }

            document.Add(table);
            document.Close();
            return stream.ToArray();
        }

        private static string EscapeCsv(string? value)
        {
            var safeValue = value ?? string.Empty;
            if (safeValue.Contains(';') || safeValue.Contains('"') || safeValue.Contains('\n'))
            {
                return $"\"{safeValue.Replace("\"", "\"\"")}\"";
            }

            return safeValue;
        }
    }
}

