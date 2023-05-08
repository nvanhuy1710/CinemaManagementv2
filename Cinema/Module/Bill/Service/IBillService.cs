using Cinema.Model;
using Cinema.Module.Bill.DTO;

namespace Cinema.Module.Bill.Service
{
    public interface IBillService
    {
        BillDTO AddBill(BillDTO bill);

        List<BillDTO> GetBillByUserId(int userId);
    }
}
