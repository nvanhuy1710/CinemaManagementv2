﻿using AutoMapper;
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
            List<List<ShowDTO>> showDTOs = GetShowByInfor(0, showDTO.RoomId, showDTO.StartTime.Date);
            foreach (List<ShowDTO> shows in showDTOs)
            {
                foreach (ShowDTO show in shows)
                {
                    if (showDTO.StartTime <= show.EndTime && showDTO.StartTime.Date == show.EndTime.Date)
                    {
                        throw new InvalidOperationException("This room has schedule conflict");
                    }
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

        public List<List<ShowDTO>> GetShowByInfor(int filmId, int roomId, DateTime date)
        {
            return _showRepository.GetShowByInfor(filmId, roomId, date).Select(p => p.Select(y => MapModelToDTO(y)).ToList()).ToList();
        }

        public ShowDTO UpdateShow(ShowDTO showDTO)
        {
            List<List<ShowDTO>> showDTOs = GetShowByInfor(0, showDTO.RoomId, showDTO.StartTime.Date);
            foreach (List<ShowDTO> shows in showDTOs)
            {
                foreach(ShowDTO show in shows)
                {
                    if (showDTO.StartTime <= show.EndTime && showDTO.StartTime.Date == show.EndTime.Date)
                    {
                        throw new InvalidOperationException("This room has schedule conflict");
                    }
                }
            }
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
