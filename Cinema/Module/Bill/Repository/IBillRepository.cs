using Cinema.Model;

namespace Cinema.Module.Bill.Repository
{
    public interface IBillRepository
    {
        BillModel AddBill(BillModel model);

        List<BillModel> GetBillByDate(DateTime startDate, DateTime endDate, int userId = 0);

        List<BillModel> GetBill(int id);

        void Refund(int id);

    }
}
