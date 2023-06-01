using Cinema.Model;
using Cinema.Module.Bill.DTO;
using Cinema.Module.Bill.Service;
using Cinema.Module.FoodOrder.DTO;
using Cinema.Module.Reservation.DTO;
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
                        seatSolds += billDTO.Reservations.Count;
                        monthRevenue += billDTO.TotalCost;
                        foreach(ReservationDTO reservation in billDTO.Reservations)
                        {
                            if (statisticDTO.SeatRevenue.ContainsKey(reservation.SeatTypeName))
                            {
                                int revenue = statisticDTO.SeatRevenue[reservation.SeatTypeName];
                                revenue += reservation.Cost;
                                statisticDTO.SeatRevenue[reservation.SeatTypeName] = revenue;
                            }
                            else
                            {
                                statisticDTO.SeatRevenue.Add(reservation.SeatTypeName, reservation.Cost);
                            }
                        }
                        string key = billDTO.ShowDTO.FilmName;
                        if (FilmRevenueInMonth.ContainsKey(key))
                        {
                            int Revenue = FilmRevenueInMonth[key];

                            Revenue += billDTO.Reservations.Count;

                            FilmRevenueInMonth[key] = Revenue;
                        }
                        else
                        {
                            FilmRevenueInMonth.Add(key, billDTO.Reservations.Count);
                        }
                        foreach(FoodOrderDTO foodOrder in billDTO.FoodOrderDTOs)
                        {
                            FoodRevenue += foodOrder.Count * foodOrder.FoodDTO.Cost;
                        }
                    }
                    else
                    {
                        seatRefunds += billDTO.Reservations.Count;
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

        private int GetToTal(BillDTO billDTO)
        {
            int total = 0;
            foreach(ReservationDTO reservation in billDTO.Reservations)
            {
                total += reservation.Cost;
            }
            return total;
        }
    }
}
