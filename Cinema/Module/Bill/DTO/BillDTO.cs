using Cinema.Enum;
using Cinema.Model;
using Cinema.Module.FoodOrder.DTO;
using Cinema.Module.Reservation.DTO;
using Cinema.Module.Show.DTO;
using Cinema.Module.User.DTO;
using Newtonsoft.Json;

namespace Cinema.Module.Bill.DTO
{
    public class BillDTO
    {
        public int Id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int UserId { get; set; }

        public int ShowId { get; set; }

        public DateTime? DatePurchased { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int TotalCost { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BillStatus BillStatus { get; set; }

        public List<int> SeatIds { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ReservationDTO>? Reservations { get; set; }

        public List<FoodOrderDTO>? FoodOrderDTOs { get; set; }

        public ShowDTO? ShowDTO { get; set; }

        public UserDTO? UserDTO { get; set; }
    }
}
