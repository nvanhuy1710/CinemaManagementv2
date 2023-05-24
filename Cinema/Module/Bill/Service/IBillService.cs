using Cinema.Model;
using Cinema.Module.Bill.DTO;

namespace Cinema.Module.Bill.Service
{
    public interface IBillService
    {
        BillDTO AddBill(BillDTO bill);

        List<BillDTO> GetBillByDate(DateTime startDate, DateTime endDate, int userId = 0);

        void Refund(int showId);
    }
}
