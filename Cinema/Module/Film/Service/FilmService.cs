using AutoMapper;
using Cinema.Model;
using Cinema.Module.Film.DTO;
using Cinema.Module.Film.Repository;
using Cinema.Module.FilmGenre.Repository;
using Cinema.Module.Genre.DTO;
using Cinema.Module.Genre.Repository;
using Cinema.Module.Genre.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.IO;

namespace Cinema.Module.Film.Service
{
    public class FilmService : IFilmService
    {

        private readonly IFilmRepository _filmRepository;

        private readonly IFilmGenreRepository _filmGenreRepository;

        private readonly IMapper _mapper;

        private readonly IWebHostEnvironment _environment;

        public FilmService(IFilmRepository filmRepository, IMapper mapper, IFilmGenreRepository filmGenreRepository, IWebHostEnvironment environment)
        {
            _filmRepository = filmRepository;
            _mapper = mapper;
            _filmGenreRepository = filmGenreRepository;
            _environment = environment;
        }
        public FilmDTO AddFilm(FilmDTO filmDTO)
        {
            filmDTO.FilmStatus = Enum.FilmStatus.NOSCHEDULED;
            FilmModel model = _filmRepository.AddFilm(_mapper.Map<FilmModel>(filmDTO));
            foreach(GenreDTO genreDTO in filmDTO.Genres)
            {
                FilmGenreModel filmGenreModel = new FilmGenreModel
                {
                    FilmId = model.Id,
                    GenreId = genreDTO.Id
                };
                _filmGenreRepository.AddFilmGenre(filmGenreModel);
            }
            return _mapper.Map<FilmDTO>(model);
        }

        public FilmDTO UpdateFilm(FilmDTO filmDTO)
        {
            FilmModel model = _filmRepository.UpdateFilm(_mapper.Map<FilmModel>(filmDTO));
            List<FilmGenreModel> filmGenreModels = _filmGenreRepository.GetFilmGenreModelsByFilmId(model.Id);
            foreach (GenreDTO genreDTO in filmDTO.Genres)
            {
                if(!filmGenreModels.Any(p => p.GenreId == genreDTO.Id))
                {
                    FilmGenreModel filmGenreModel = new FilmGenreModel
                    {
                        FilmId = model.Id,
                        GenreId = genreDTO.Id
                    };
                    _filmGenreRepository.AddFilmGenre(filmGenreModel);
                }
                
            }
            foreach (FilmGenreModel filmGenreModel in filmGenreModels)
            {
                if (!filmDTO.Genres.Any(p => p.Id == filmGenreModel.GenreId))
                {
                    _filmGenreRepository.DeleteFilmGenre(filmGenreModel.Id);
                }

            }
            return _mapper.Map<FilmDTO>(model);
        }

        public void DeleteFilm(int id)
        {
            FilmModel filmModel = _filmRepository.GetFilm(id);
            filmModel.FilmStatus = Enum.FilmStatus.DELETED;
            _filmRepository.UpdateFilm(filmModel);
        }

        public FilmDTO GetFilm(int id)
        {
            FilmModel filmModel = _filmRepository.GetFilm(id);
            FilmDTO filmDTO = _mapper.Map<FilmDTO>(filmModel);
            filmDTO.Genres = filmModel.FilmGenreModels.Select(p => _mapper.Map<GenreDTO>(p.Genre)).ToList();
            filmDTO.Poster = GetFilePath(filmDTO.Id, filmDTO.Name.Replace(" ", ""));
            return filmDTO;
        }

        public List<FilmDTO> GetFilms()
        {
            return _filmRepository.GetFilms().Select(model =>
            {
                FilmDTO filmDTO = _mapper.Map<FilmDTO>(model);
                filmDTO.Genres = model.FilmGenreModels.Select(p => _mapper.Map<GenreDTO>(p.Genre)).ToList();
                filmDTO.Poster = GetFilePath(filmDTO.Id, filmDTO.Name.Replace(" ", ""));
                return filmDTO;
            }).ToList();
        }

        public void SavePoster(int id, string name, IFormFile file)
        {
            string Filepath = GetFolderPath(id + name.Replace(" ", ""));

            if (!System.IO.Directory.Exists(Filepath))
            {
                System.IO.Directory.CreateDirectory(Filepath);
            }

            string imagepath = Filepath + "\\Poster.png";

            if (System.IO.File.Exists(imagepath))
            {
                System.IO.File.Delete(imagepath);
            }
            using (FileStream stream = System.IO.File.Create(imagepath))
            {
                file.CopyTo(stream);
            }
        }

        private string GetFolderPath(string FolderName)
        {
            return this._environment.WebRootPath + "\\Uploads\\Movies\\" + FolderName;
        }

        private string GetFilePath(int MovieId, string MovieName)
        {
            string Url = string.Empty;
            string HostUrl = "https://localhost:7163/";
            string Folderpath = GetFolderPath(MovieId + MovieName);
            string Filepath = Folderpath + "\\" + "Poster.png";
            if (!System.IO.File.Exists(Filepath))
            {
                return null;
            }
            else
            {
                Url = HostUrl + "/Uploads/Movies/" + MovieId + MovieName + "/" + "Poster.png";
            }
            return Url;

        }
    }
}
