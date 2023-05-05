using AutoMapper;
using Cinema.Model;
using Cinema.Module.Film.DTO;
using Cinema.Module.Film.Service;
using Cinema.Module.Room.DTO;
using Cinema.Module.Show.DTO;
using Cinema.Module.Show.Repository;

namespace Cinema.Module.Show.Service
{
    public class ShowService : IShowService
    {

        private readonly IShowRepository _showRepository;

        private readonly IFilmService _filmService;

        private readonly IMapper _mapper;

        public ShowService(IMapper mapper, IShowRepository showRepository, IFilmService filmService)
        {
            _mapper = mapper;
            _showRepository = showRepository;
            _filmService = filmService;
        }

        public ShowDTO AddShow(ShowDTO showDTO)
        {
            List<ShowModel> showModels = GetShowByRoom(showDTO.RoomId, showDTO.StartTime.Date);
            foreach(ShowModel showModel in showModels) 
            {
                if(showDTO.StartTime <= showModel.EndTime && showDTO.StartTime.Date == showModel.EndTime.Date)
                {
                    throw new InvalidOperationException("This room already has scheduled");
                }
            }
            FilmDTO filmDTO = _filmService.GetFilm(showDTO.FilmId);
            showDTO.EndTime = showDTO.StartTime.AddMinutes(filmDTO.Length);
            return MapModelToDTO(_showRepository.AddShow(_mapper.Map<ShowModel>(showDTO)));
        }

        public void DeleteShow(int id)
        {
            throw new NotImplementedException();
        }

        public ShowDTO GetShow(int id)
        {
            return MapModelToDTO(_showRepository.GetShow(id));
        }

        public List<ShowDTO> GetShowByInfor(int filmId, int roomId, DateTime date)
        {
            return _showRepository.GetShowByInfor(filmId, roomId, date).Select(p => MapModelToDTO(p)).ToList();
        }

        public ShowDTO UpdateShow(ShowDTO showDTO)
        {
            List<ShowModel> showModels = GetShowByRoom(showDTO.RoomId, showDTO.StartTime.Date);
            foreach (ShowModel showModel in showModels)
            {
                if (showDTO.StartTime <= showModel.EndTime && showDTO.StartTime.Date == showModel.EndTime.Date)
                {
                    throw new InvalidOperationException("This room has schedule conflict");
                }
            }
            return MapModelToDTO(_showRepository.UpdateShow(_mapper.Map<ShowModel>(showDTO)));
        }

        private ShowDTO MapModelToDTO(ShowModel showModel)
        {
            ShowDTO showDTO = _mapper.Map<ShowDTO>(showModel);
            if(showModel.Film != null) showDTO.FilmDTO = _mapper.Map<FilmDTO>(showModel.Film);
            if (showModel.Room != null) showDTO.RoomDTO = _mapper.Map<RoomDTO>(showModel.Room);
            return showDTO;
        }

        private List<ShowModel> GetShowByRoom(int roomId, DateTime date)
        {
            return _showRepository.GetShowByInfor(0, roomId, date).ToList();
        }
    }
}
