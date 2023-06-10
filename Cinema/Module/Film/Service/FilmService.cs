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
using System.Text.RegularExpressions;

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
        public FilmDTO AddFilm(FilmDTO filmDTO, string PosterUrl, string AdPosterUrl)
        {
            filmDTO.PosterUrl = PosterUrl.Substring(PosterUrl.IndexOf("wwwroot"));
            filmDTO.AdPosterUrl = AdPosterUrl.Substring(AdPosterUrl.IndexOf("wwwroot"));
            filmDTO.FilmStatus = Enum.FilmStatus.AVAILABLE;
            filmDTO.ReleaseDate = filmDTO.ReleaseDate.Date;
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
            filmDTO.ReleaseDate = filmDTO.ReleaseDate.Date;
            FilmModel oldFilm = _filmRepository.GetFilm(filmDTO.Id);
            string oldPath = Path.GetDirectoryName(oldFilm.PosterUrl);
            string newFullPath = GetFolderPath(RemoveSpecialCharacters(filmDTO.Name) + "_" + RemoveSpecialCharacters(filmDTO.Director));
            string newPath = newFullPath.Substring(newFullPath.IndexOf("wwwroot"));
            if(!newPath.Equals(oldPath))
            {
                if (Directory.Exists(oldPath))
                {
                    Directory.Move(oldPath, newFullPath);
                }
                _filmRepository.UpdateImage(filmDTO.Id, newPath + "\\Poster.png", newPath + "\\AdPoster.png");
            }
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
            filmDTO.PosterUrl = GetFileUrl(filmDTO.PosterUrl);
            filmDTO.AdPosterUrl = GetFileUrl(filmDTO.AdPosterUrl, true);
            return filmDTO;
        }

        public List<FilmDTO> GetFilms()
        {
            return _filmRepository.GetFilms().Select(model =>
            {
                FilmDTO filmDTO = _mapper.Map<FilmDTO>(model);
                filmDTO.Genres = model.FilmGenreModels.Select(p => _mapper.Map<GenreDTO>(p.Genre)).ToList();
                filmDTO.PosterUrl = GetFileUrl(filmDTO.PosterUrl);
                filmDTO.AdPosterUrl = GetFileUrl(filmDTO.AdPosterUrl, true);
                return filmDTO;
            }).ToList();
        }

        public List<FilmDTO> GetCurrentFilms()
        {
            return _filmRepository.GetCurrentFilms().Select(model => 
            {
                FilmDTO filmDTO = _mapper.Map<FilmDTO>(model);
                filmDTO.Genres = model.FilmGenreModels.Select(p => _mapper.Map<GenreDTO>(p.Genre)).ToList();
                filmDTO.PosterUrl = GetFileUrl(filmDTO.PosterUrl);
                filmDTO.AdPosterUrl = GetFileUrl(filmDTO.AdPosterUrl, true);
                return filmDTO;
            }).ToList();
        }

        public List<FilmDTO> GetIncomingFilms()
        {
            return _filmRepository.GetIncomingFilms().Select(model =>
            {
                FilmDTO filmDTO = _mapper.Map<FilmDTO>(model);
                filmDTO.Genres = model.FilmGenreModels.Select(p => _mapper.Map<GenreDTO>(p.Genre)).ToList();
                filmDTO.PosterUrl = GetFileUrl(filmDTO.PosterUrl);
                filmDTO.AdPosterUrl = GetFileUrl(filmDTO.AdPosterUrl, true);
                return filmDTO;
            }).ToList();
        }

        public List<FilmDTO> GetFilms(string name)
        {
            if(name == null) return GetFilms();
            return _filmRepository.GetFilms(name).Select(model =>
            {
                FilmDTO filmDTO = _mapper.Map<FilmDTO>(model);
                filmDTO.Genres = model.FilmGenreModels.Select(p => _mapper.Map<GenreDTO>(p.Genre)).ToList();
                filmDTO.PosterUrl = GetFileUrl(filmDTO.PosterUrl);
                filmDTO.AdPosterUrl = GetFileUrl(filmDTO.AdPosterUrl, true);
                return filmDTO;
            }).ToList();
        }

        public string SavePoster(string name, string director, IFormFile file, bool isAdPoster = false)
        {
            string Filepath = GetFolderPath(RemoveSpecialCharacters(name) + "_" + RemoveSpecialCharacters(director));

            if (!System.IO.Directory.Exists(Filepath))
            {
                System.IO.Directory.CreateDirectory(Filepath);
            }
            string imagepath;
            if(isAdPoster) imagepath = Filepath + "\\AdPoster.png";
            else imagepath = Filepath + "\\Poster.png";
            if (System.IO.File.Exists(imagepath))
            {
                System.IO.File.Delete(imagepath);
            }
            using (FileStream stream = System.IO.File.Create(imagepath))
            {
                file.CopyTo(stream);
            }
            return imagepath;
        }

        private string GetFolderPath(string FolderName)
        {
            return this._environment.WebRootPath + "\\Uploads\\Movies\\" + FolderName;
        }

        private string GetFileUrl(string path, bool IsAdPoster = false)
        {
            if (path == null) return null;
            string Url = string.Empty;
            string HostUrl = "https://localhost:44308/";
            string Filepath = path;
            if (!System.IO.File.Exists(Filepath))
            {
                return null;
            }
            else
            {
                string[] folder = path.Split(Path.DirectorySeparatorChar);
                if(IsAdPoster) Url = HostUrl + "Uploads/Movies/" + folder[3] + "/" + "AdPoster.png";
                else Url = HostUrl + "Uploads/Movies/" + folder[3] + "/" + "Poster.png";
            }
            return Url;

        }
        private string RemoveSpecialCharacters(string str)
        {
            // Danh sách các ký tự tiếng Việt có dấu và không dấu muốn giữ nguyên
            string vietnameseCharacters = "áàảãạâấầẩẫậăắằẳẵặđéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵ" +
                                          "ÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴ";

            // Tạo biểu thức chính quy để chỉ thay thế các ký tự đặc biệt khỏi chuỗi
            string pattern = @"[^0-9a-zA-Z" + vietnameseCharacters + "]+";

            string result = Regex.Replace(str, pattern, "");
            result = Regex.Replace(result, @"-+", "");
            result = result.Trim('-');

            return result;
        }

        public List<FilmDTO> GetDeletedFilms(string name)
        {
            return _filmRepository.GetDeletedFilms(name).Select(p => _mapper.Map<FilmDTO>(p)).ToList();
        }
    }
}
