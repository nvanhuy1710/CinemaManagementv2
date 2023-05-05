using Cinema.Module.Show.DTO;

namespace Cinema.Module.Show.Service
{
    public interface IShowService
    {
        ShowDTO AddShow(ShowDTO showDTO);

        ShowDTO UpdateShow(ShowDTO showDTO);

        void DeleteShow(int id);

        ShowDTO GetShow(int id);

        List<ShowDTO> GetShowByInfor(int filmId, int roomId, DateTime date);
    }
}
