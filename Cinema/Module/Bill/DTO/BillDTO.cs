using Cinema.Module.FoodOrder.DTO;
using Cinema.Module.Show.DTO;
using Cinema.Module.User.DTO;

namespace Cinema.Module.Bill.DTO
{
    public class BillDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ShowId { get; set; }

        public DateTime? DatePurchased { get; set; }

        public int? TotalCost { get; set; }

        public List<int> SeatIds { get; set; }

        public List<FoodOrderDTO>? FoodOrderDTOs { get; set; }

        public ShowDTO? ShowDTO { get; set; }

        public UserDTO? UserDTO { get; set; }
    }
}
