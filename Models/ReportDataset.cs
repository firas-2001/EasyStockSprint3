namespace EasyStock.Models
{
    public class ReportDataset
    {
        public string Title { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string WorksheetName { get; set; } = "Rapport";
        public List<string> Headers { get; set; } = new();
        public List<IReadOnlyList<string>> Rows { get; set; } = new();
    }
}
