using AutoMapper;
using Cinema.Model;
using Cinema.Module.Account.Service;
using Cinema.Module.Genre.DTO;
using Cinema.Module.Genre.Repository;
using Cinema.Module.User.Repository;

namespace Cinema.Module.Genre.Service
{
    public class GenreService : IGenreService
    {

        private readonly IGenreRepository _genreRepository;

        private readonly IMapper _mapper;

        public GenreService(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public GenreDTO AddGenre(GenreDTO genreDTO)
        {
            return _mapper.Map<GenreDTO>(_genreRepository.AddGenre(_mapper.Map<GenreModel>(genreDTO)));
        }

        public void DeleteGenre(int id)
        {
            _genreRepository.DeleteGenre(id);
        }

        public GenreDTO GetGenre(int id)
        {
            return _mapper.Map<GenreDTO>(_genreRepository.GetGenre(id));
        }

        public List<GenreDTO> GetGenres()
        {
            return _genreRepository.GetAllGenres().Select(p => _mapper.Map<GenreDTO>(p)).ToList();
        }

        public GenreDTO UpdateGenre(GenreDTO genreDTO)
        {
            return _mapper.Map<GenreDTO>(_genreRepository.UpdateGenre(_mapper.Map<GenreModel>(genreDTO)));
        }
    }
}
