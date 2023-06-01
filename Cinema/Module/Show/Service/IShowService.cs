using Cinema.Module.Show.DTO;

namespace Cinema.Module.Show.Service
{
    public interface IShowService
    {
        List<ShowDTO> AddShow(List<ShowDTO> showDTOs);

        ShowDTO UpdateShow(ShowDTO showDTO);

        void DeleteShow(int id);

        ShowDTO GetShow(int id);

        ShowDTO GetShowForStatistic(int id);

        List<ShowDTO> GetShowInTime(DateTime startDate, DateTime endDate);

        List<List<ShowDTO>> GetShowByInfor(int filmId, int roomId, DateTime date);
    }
}
