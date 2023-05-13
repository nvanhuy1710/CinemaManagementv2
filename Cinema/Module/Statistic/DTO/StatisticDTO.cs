namespace Cinema.Module.Statistic.DTO
{
    public class StatisticDTO
    {
        public List<int> MonthlyRevenue { get; set; } = new List<int>();

        public List<Dictionary<string, int>> FilmRevenue { get; set; } = new List<Dictionary<string, int>>();

        public Dictionary<string, int> SeatRevenue { get; set; } = new Dictionary<string, int>();

        public List<int> SeatSold { get; set; } = new List<int>();

        public List<int> SeatRefund { get; set; } = new List<int>();

        public List<int> FoodRevenue { get; set; } = new List<int>();
    }
}
