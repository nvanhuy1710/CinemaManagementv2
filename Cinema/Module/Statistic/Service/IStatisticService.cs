using Cinema.Module.Statistic.DTO;

namespace Cinema.Module.Statistic.Service
{
    public interface IStatisticService
    {
        public StatisticDTO GetStatistic(int year);
    }
}
