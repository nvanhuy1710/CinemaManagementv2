using Cinema.Module.Bill.DTO;
using Cinema.Module.Bill.Service;
using Cinema.Module.Show.Service;
using Cinema.Module.Statistic.DTO;

namespace Cinema.Module.Statistic.Service
{
    public class StatisticService : IStatisticService
    {

        private readonly IBillService _billService;

        private readonly IShowService _showService;

        public StatisticService(IBillService billService, IShowService showService)
        {
            _billService = billService;
            _showService = showService;
        }

        public StatisticDTO GetStatistic(int year)
        {
            StatisticDTO statisticDTO = new StatisticDTO();
            int quarterRevenue = 0;
            for(int i = 0; i < 12; i++)
            {
                int days = DateTime.DaysInMonth(year, i + 1);
                Dictionary<string, int> FilmRevenueInMonth = new Dictionary<string, int>();
                List<BillDTO> billDTOs = _billService.GetBillByDate(new DateTime(year, i + 1, 1), new DateTime(year, i + 1, days));
                int monthRevenue = 0;
                foreach (BillDTO billDTO in billDTOs)
                {
                    if(billDTO.BillStatus != Enum.BillStatus.REFUNDED)
                    {
                        monthRevenue += billDTO.TotalCost;
                        quarterRevenue += billDTO.TotalCost;
                        string key = billDTO.ShowDTO.FilmName;
                        if (FilmRevenueInMonth.ContainsKey(key))
                        {
                            int Revenue = FilmRevenueInMonth[key];

                            Revenue += 1;

                            FilmRevenueInMonth[key] = Revenue;
                        }
                        else
                        {
                            FilmRevenueInMonth.Add(key, 1);
                        }
                    }
                }
                statisticDTO.MonthlyRevenue.Add(monthRevenue);
                statisticDTO.FilmRevenue.Add(FilmRevenueInMonth);
                if (i + 1 == 3 || i + 1 == 6 || i + 1 == 9 || i + 1 == 12)
                {
                    statisticDTO.QuarterRevenue.Add(quarterRevenue);
                    quarterRevenue = 0;
                }
            }
            return statisticDTO;
        }
    }
}
