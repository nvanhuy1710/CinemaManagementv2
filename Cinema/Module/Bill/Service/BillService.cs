using AutoMapper;
using Cinema.Model;
using Cinema.Module.Bill.DTO;
using Cinema.Module.Bill.Repository;
using Cinema.Module.Film.Service;
using Cinema.Module.Food.Service;
using Cinema.Module.FoodOrder.DTO;
using Cinema.Module.FoodOrder.Service;
using Cinema.Module.Reservation.DTO;
using Cinema.Module.Reservation.Repository;
using Cinema.Module.Room.Service;
using Cinema.Module.Seat.DTO;
using Cinema.Module.Seat.Service;
using Cinema.Module.Show.DTO;
using Cinema.Module.Show.Service;
using Cinema.Module.User.Service;

namespace Cinema.Module.Bill.Service
{
    public class BillService : IBillService
    {

        private readonly IBillRepository _billRepository;

        private readonly IFoodOrderService _foodOrderService;

        private readonly IReservationRepository _reservationRepository;

        private readonly IShowService _showService;

        private readonly IRoomService _roomService;

        private readonly IFilmService _filmService;

        private readonly ISeatService _seatService;

        private readonly IUserService _userService;

        private readonly IFoodService _foodService;

        private readonly IMapper _mapper;

        public BillService(IBillRepository billRepository, IMapper mapper, 
            IFoodOrderService foodOrderService,
            IReservationRepository reservationRepository,
            IShowService showService,
            IUserService userService,
            IRoomService roomService,
            IFilmService filmService,
            ISeatService seatService,
            IFoodService foodService)
        {
            _billRepository = billRepository;
            _mapper = mapper;
            _foodOrderService = foodOrderService;
            _reservationRepository = reservationRepository;
            _userService = userService;
            _showService = showService;
            _roomService = roomService;
            _filmService = filmService;
            _seatService = seatService;
            _foodService = foodService;
        }

        public BillDTO AddBill(BillDTO bill)
        {
            if((DateTime.Today.Year - _userService.GetUser(bill.UserId).Birth.Year) < _showService.GetShow(bill.ShowId).AgeLimit && _userService.GetUser(bill.UserId).RoleName == "USER") 
            {
                throw new ArgumentException("User not enough old!");
            }
            bill.DatePurchased = DateTime.Now.Date;
            BillModel result = _billRepository.AddBill(_mapper.Map<BillModel>(bill));
            if(bill.FoodOrderDTOs != null)
            {
                foreach(FoodOrderDTO foodOrderDTO in  bill.FoodOrderDTOs)
                {
                    foodOrderDTO.BillId = result.Id;
                    _foodOrderService.AddFoodOrder(foodOrderDTO);
                }
            }
            foreach(int seatId in bill.SeatIds)
            {
                SeatDTO seat = _seatService.GetSeat(seatId);
                ReservationModel reservationModel = new ReservationModel
                {
                    SeatId = seatId,
                    BillId = result.Id,
                    ShowId = bill.ShowId,
                    Cost = seat.SeatTypeDTO.Cost,
                    SeatTypeName = seat.SeatTypeDTO.Name
                };
                _reservationRepository.AddReservation(reservationModel);
            }
            return _mapper.Map<BillDTO>(result);
        }

        public List<BillDTO> GetBillByDate(DateTime startDate, DateTime endDate, int userId = 0)
        {
            return _billRepository.GetBillByDate(startDate, endDate, userId).Select(p =>
            {
                BillDTO billDTO = _mapper.Map<BillDTO>(p);
                billDTO.FoodOrderDTOs = p.FoodOrderModels.Select(y => _mapper.Map<FoodOrderDTO>(y)).ToList();
                billDTO.ShowDTO = _showService.GetShowForStatistic(p.ReservationModels.First().ShowId);
                billDTO.TotalCost = GetTotalCost(p);
                billDTO.Reservations = p.ReservationModels.Select(y => _mapper.Map<ReservationDTO>(y)).ToList();
                return billDTO;
            }).ToList();
        }

        public void Refund(int showId)
        {
            ShowDTO showDTO = _showService.GetShow(showId);
            if(showDTO.EndTime <= DateTime.Now)
            {
                _showService.DeleteShow(showId);
                return;
            }
            List<ReservationModel> reservationModels = _reservationRepository.GetReservationByShowId(showId);
            List<int> billIds = reservationModels.Select(p => p.BillId).Distinct().ToList();
            foreach(int billId in billIds)
            {
                _billRepository.Refund(billId);
            }
            _showService.DeleteShow(showId);
        }

        private int GetTotalCost(BillModel billModel)
        {
            int total = 0;
            foreach(ReservationModel reservation in billModel.ReservationModels)
            {
                total += reservation.Cost;
            }
            return total;
        }
    }
}
