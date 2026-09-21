namespace EasyStock.Models.ViewModels
{
    public class ReportIndexViewModel
    {
        public DateTime? StartDate { get; set; } = DateTime.Today.AddMonths(-1);
        public DateTime? EndDate { get; set; } = DateTime.Today;
        public int? ArticleId { get; set; }
        public string? TypeMouvement { get; set; }
        public string? Username { get; set; }
        public string Format { get; set; } = "csv";
    }
}
