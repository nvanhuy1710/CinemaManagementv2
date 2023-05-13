using Cinema.Model;

namespace Cinema.Module.Bill.Repository
{
    public interface IBillRepository
    {
        BillModel AddBill(BillModel model);

        List<BillModel> GetBillByUserId(int userId);

        List<BillModel> GetBillByDate(DateTime startDate, DateTime endDate);

        List<BillModel> GetBill(int id);

        void Refund(int id);

    }
}
