namespace Cinema.Module.Statistic.DTO
{
    public class StatisticDTO
    {
        public List<int> MonthlyRevenue { get; set; } = new List<int>();

        public List<Dictionary<string, int>> FilmRevenue { get; set; } = new List<Dictionary<string, int>>();

        public List<int> QuarterRevenue { get; set; } = new List<int>();
    }
}
