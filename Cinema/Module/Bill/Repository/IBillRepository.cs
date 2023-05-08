using Cinema.Model;

namespace Cinema.Module.Bill.Repository
{
    public interface IBillRepository
    {
        BillModel AddBill(BillModel model);

        List<BillModel> GetBillByUserId(int userId);
    }
}
