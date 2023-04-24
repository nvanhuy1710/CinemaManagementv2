using AutoMapper;
using Cinema.Model;
using Cinema.Module.Account.DTO;
using Cinema.Module.Film.DTO;
using Cinema.Module.Genre.DTO;
using Cinema.Module.Role.DTO;
using Cinema.Module.User.DTO;

namespace Cinema.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<UserDTO, UserModel>();
            CreateMap<UserModel, UserDTO>();
            CreateMap<RoleDTO, RoleModel>();
            CreateMap<RoleModel, RoleDTO>();
            CreateMap<AccountDTO, AccountModel>().ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                                                 .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
            CreateMap<AccountModel, AccountDTO>().ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                                                 .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password)) 
                                                 .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));
            CreateMap<GenreDTO, GenreModel>();
            CreateMap<GenreModel, GenreDTO>();
            CreateMap<FilmDTO, FilmModel>();
            CreateMap<FilmModel, FilmDTO>();
        }
    }
}
