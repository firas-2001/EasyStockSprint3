using EasyStock.Models;

namespace EasyStock.Models.ViewModels
{
    public class StockMovementHistoryViewModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ArticleId { get; set; }
        public string? TypeMouvement { get; set; }
        public string? Username { get; set; }
        public List<MouvementStock> Items { get; set; } = new();
    }
}
