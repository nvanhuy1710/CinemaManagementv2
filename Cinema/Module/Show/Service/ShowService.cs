using AutoMapper;
using Cinema.Model;
using Cinema.Module.Bill.Service;
using Cinema.Module.Film.DTO;
using Cinema.Module.Film.Service;
using Cinema.Module.Reservation.Repository;
using Cinema.Module.Room.DTO;
using Cinema.Module.Room.Service;
using Cinema.Module.Show.DTO;
using Cinema.Module.Show.Repository;
using Microsoft.IdentityModel.Tokens;

namespace Cinema.Module.Show.Service
{
    public class ShowService : IShowService
    {

        private readonly IShowRepository _showRepository;

        private readonly IFilmService _filmService;

        private readonly IRoomService _roomService;

        private readonly IReservationRepository _reservationRepository;

        private readonly IMapper _mapper;

        public ShowService(IMapper mapper, IShowRepository showRepository, IFilmService filmService, IRoomService roomService, IReservationRepository reservationRepository)
        {
            _mapper = mapper;
            _showRepository = showRepository;
            _filmService = filmService;
            _roomService = roomService;
            _reservationRepository = reservationRepository;
        }

        public List<ShowDTO> AddShow(List<ShowDTO> showDTOs)
        {
            List<ShowDTO> result = new List<ShowDTO>();
            foreach(ShowDTO showDTO in showDTOs)
            {
                FilmDTO filmDTO = _filmService.GetFilm(showDTO.FilmId);
                showDTO.EndTime = showDTO.StartTime.AddMinutes(filmDTO.Length);
                if (_roomService.GetRoom(showDTO.RoomId).RoomStatus == Enum.RoomStatus.REPAIRING)
                {
                    throw new InvalidOperationException("Room is repairing");
                }
                if (_filmService.GetFilm(showDTO.FilmId).FilmStatus == Enum.FilmStatus.DELETED)
                {
                    throw new InvalidOperationException("Film deleted");
                }
                List<List<ShowDTO>> oldShowDTOs = GetShowByInfor(0, showDTO.RoomId, showDTO.StartTime.Date);
                foreach (List<ShowDTO> shows in oldShowDTOs)
                {
                    foreach (ShowDTO show in shows)
                    {
                        if (((showDTO.StartTime <= show.EndTime && showDTO.StartTime >= show.StartTime) ||
                            (showDTO.EndTime <= show.EndTime && showDTO.EndTime >= show.StartTime)) &&
                            showDTO.StartTime.Date == show.EndTime.Date)
                        {
                            throw new InvalidOperationException("This room has schedule conflict at " + showDTO.StartTime + " - " + showDTO.EndTime);
                        }
                    }
                }
                result.Add(MapModelToDTO(_showRepository.AddShow(_mapper.Map<ShowModel>(showDTO))));
            }
            return result;
        }

        public void DeleteShow(int id)
        {
            ShowModel showModel = _showRepository.GetShow(id);
            showModel.IsDeleted = true;
            _showRepository.UpdateShow(showModel);
        }

        public ShowDTO GetShow(int id)
        {
            return MapModelToDTO(_showRepository.GetShow(id));
        }

        public List<List<ShowDTO>> GetShowByInfor(int filmId, int roomId, DateTime date)
        {
            return _showRepository.GetShowByInfor(filmId, roomId, date).Select(p => p.Select(y => MapModelToDTO(y)).ToList()).ToList();
        }

        public ShowDTO GetShowForStatistic(int id)
        {
            return MapModelToDTO(_showRepository.GetShowForStatistic(id));
        }

        public List<ShowDTO> GetShowInTime(DateTime startDate, DateTime endDate)
        {
            return _showRepository.GetShowInTime(startDate.Date, endDate.Date).Select(p => MapModelToDTO(p)).ToList();
        }

        public ShowDTO UpdateShow(ShowDTO showDTO)
        {
            FilmDTO filmDTO = _filmService.GetFilm(showDTO.FilmId);
            showDTO.EndTime = showDTO.StartTime.AddMinutes(filmDTO.Length);
            if (_roomService.GetRoom(showDTO.RoomId).RoomStatus == Enum.RoomStatus.REPAIRING)
            {
                throw new InvalidOperationException("Room is repairing");
            }
            if (_filmService.GetFilm(showDTO.FilmId).FilmStatus == Enum.FilmStatus.DELETED)
            {
                throw new InvalidOperationException("Film deleted");
            }
            List<List<ShowDTO>> showDTOs = GetShowByInfor(0, showDTO.RoomId, showDTO.StartTime.Date);
            foreach (List<ShowDTO> shows in showDTOs)
            {
                foreach (ShowDTO show in shows)
                {
                    if ((((showDTO.StartTime <= show.EndTime && showDTO.StartTime >= show.StartTime) ||
                        (showDTO.EndTime <= show.EndTime && showDTO.EndTime >= show.StartTime)) &&
                        showDTO.StartTime.Date == show.EndTime.Date) && showDTO.Id != show.Id)
                    {
                        throw new InvalidOperationException("This room has schedule conflict");
                    }
                }
            }
            if(!_reservationRepository.GetReservationByShowId(showDTO.Id).IsNullOrEmpty()) throw new InvalidOperationException("This show had ticket");
            return MapModelToDTO(_showRepository.UpdateShow(_mapper.Map<ShowModel>(showDTO)));
        }

        private ShowDTO MapModelToDTO(ShowModel showModel)
        {
            ShowDTO showDTO = _mapper.Map<ShowDTO>(showModel);
            showDTO.Poster = _filmService.GetFilm(showModel.FilmId).PosterUrl;
            return showDTO;
        }
    }
}
