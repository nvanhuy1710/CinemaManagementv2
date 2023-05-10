using Cinema.Module.Bill.DTO;
using Cinema.Module.Food.DTO;
using Newtonsoft.Json;

namespace Cinema.Module.FoodOrder.DTO
{
    public class FoodOrderDTO
    {
        public int Id { get; set; }

        public int FoodId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int BillId { get; set; }

        public int Count { get; set; }

        public FoodDTO? FoodDTO { get; set; }

        public BillDTO? BillDTO { get; set; }
    }
}
