using Cinema.Model;
using Cinema.Module.Bill.DTO;
using Cinema.Module.Bill.Service;
using Cinema.Module.FoodOrder.DTO;
using Cinema.Module.Seat.DTO;
using Cinema.Module.Seat.Service;
using Cinema.Module.Show.Service;
using Cinema.Module.Statistic.DTO;

namespace Cinema.Module.Statistic.Service
{
    public class StatisticService : IStatisticService
    {

        private readonly IBillService _billService;

        private readonly IShowService _showService;

        private readonly ISeatService _seatService;

        public StatisticService(IBillService billService, IShowService showService, ISeatService seatService)
        {
            _billService = billService;
            _showService = showService;
            _seatService = seatService;
        }

        public StatisticDTO GetStatistic(int year)
        {
            StatisticDTO statisticDTO = new StatisticDTO();
            for(int i = 0; i < 12; i++)
            {
                int seatSolds = 0;
                int seatRefunds = 0;
                int FoodRevenue = 0;
                int days = DateTime.DaysInMonth(year, i + 1);
                Dictionary<string, int> FilmRevenueInMonth = new Dictionary<string, int>();
                List<BillDTO> billDTOs = _billService.GetBillByDate(new DateTime(year, i + 1, 1), new DateTime(year, i + 1, days));
                int monthRevenue = 0;
                foreach (BillDTO billDTO in billDTOs)
                {
                    if(billDTO.BillStatus != Enum.BillStatus.REFUNDED)
                    {
                        seatSolds += billDTO.SeatIds.Count;
                        monthRevenue += billDTO.TotalCost;
                        foreach(int seatId in billDTO.SeatIds)
                        {
                            SeatDTO seatDTO = _seatService.GetSeat(seatId);
                            if(statisticDTO.SeatRevenue.ContainsKey(seatDTO.SeatTypeDTO.Name))
                            {
                                int revenue = statisticDTO.SeatRevenue[seatDTO.SeatTypeDTO.Name];
                                revenue += seatDTO.SeatTypeDTO.Cost;
                                statisticDTO.SeatRevenue[seatDTO.SeatTypeDTO.Name] = revenue;
                            }
                            else
                            {
                                statisticDTO.SeatRevenue.Add(seatDTO.SeatTypeDTO.Name, seatDTO.SeatTypeDTO.Cost);
                            }
                        }
                        string key = billDTO.ShowDTO.FilmName;
                        if (FilmRevenueInMonth.ContainsKey(key))
                        {
                            int Revenue = FilmRevenueInMonth[key];

                            Revenue += billDTO.SeatIds.Count;

                            FilmRevenueInMonth[key] = Revenue;
                        }
                        else
                        {
                            FilmRevenueInMonth.Add(key, billDTO.SeatIds.Count);
                        }
                        foreach(FoodOrderDTO foodOrder in billDTO.FoodOrderDTOs)
                        {
                            FoodRevenue += foodOrder.Count * foodOrder.FoodDTO.Cost;
                        }
                    }
                    else
                    {
                        seatRefunds += billDTO.SeatIds.Count;
                    }
                }
                statisticDTO.MonthlyRevenue.Add(monthRevenue);
                statisticDTO.FilmRevenue.Add(FilmRevenueInMonth);
                statisticDTO.SeatSold.Add(seatSolds);
                statisticDTO.SeatRefund.Add(seatRefunds);
                statisticDTO.FoodRevenue.Add(FoodRevenue);               
            }
            return statisticDTO;
        }
    }
}
