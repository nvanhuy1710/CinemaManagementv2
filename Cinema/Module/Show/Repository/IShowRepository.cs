using Cinema.Model;

namespace Cinema.Module.Show.Repository
{
    public interface IShowRepository
    {
        ShowModel AddShow(ShowModel model);

        ShowModel UpdateShow(ShowModel model);

        void DeleteShow(int id);

        ShowModel GetShow(int id);

        ShowModel GetShowForStatistic(int id);

        List<List<ShowModel>> GetShowByInfor(int filmId, int roomId, DateTime date);

        List<ShowModel> GetShowInTime(DateTime startDate, DateTime endDate);
    }
}
