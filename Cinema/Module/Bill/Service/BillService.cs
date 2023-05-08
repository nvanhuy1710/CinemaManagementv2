using AutoMapper;
using Cinema.Model;
using Cinema.Module.Bill.DTO;
using Cinema.Module.Bill.Repository;
using Cinema.Module.FoodOrder.DTO;
using Cinema.Module.FoodOrder.Service;
using Cinema.Module.Reservation.Repository;
using Cinema.Module.Seat.DTO;
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

        private readonly IUserService _userService;

        private readonly IMapper _mapper;

        public BillService(IBillRepository billRepository, IMapper mapper, 
            IFoodOrderService foodOrderService,
            IReservationRepository reservationRepository,
            IShowService showService,
            IUserService userService)
        {
            _billRepository = billRepository;
            _mapper = mapper;
            _foodOrderService = foodOrderService;
            _reservationRepository = reservationRepository;
            _userService = userService;
            _showService = showService;
        }

        public BillDTO AddBill(BillDTO bill)
        {
            if((DateTime.Today.Year - _userService.GetUser(bill.UserId).Birth.Year) < _showService.GetShow(bill.Id).AgeLimit) 
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
                ReservationModel reservationModel = new ReservationModel
                {
                    SeatId = seatId,
                    BillId = result.Id,
                    ShowId = bill.ShowId
                };
                _reservationRepository.AddReservation(reservationModel);
            }
            return _mapper.Map<BillDTO>(result);
        }

        public List<BillDTO> GetBillByUserId(int userId)
        {
            return _billRepository.GetBillByUserId(userId).Select(p =>
            {
                BillDTO billDTO = _mapper.Map<BillDTO>(p);
                billDTO.FoodOrderDTOs = p.FoodOrderModels.Select(y => _mapper.Map<FoodOrderDTO>(y)).ToList();
                //billDTO.SeatDTOs = p.ReservationModels.Select(z => _mapper.Map<SeatDTO>(z.SeatModel)).ToList();
                return billDTO;
            }).ToList();
        }
    }
}
