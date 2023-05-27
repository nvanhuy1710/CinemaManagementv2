using AutoMapper;
using Cinema.Model;
using Cinema.Module.Account.DTO;
using Cinema.Module.Bill.DTO;
using Cinema.Module.Film.DTO;
using Cinema.Module.Food.DTO;
using Cinema.Module.FoodOrder.DTO;
using Cinema.Module.Genre.DTO;
using Cinema.Module.Reservation.DTO;
using Cinema.Module.Role.DTO;
using Cinema.Module.Room.DTO;
using Cinema.Module.Seat.DTO;
using Cinema.Module.SeatType.DTO;
using Cinema.Module.Show.DTO;
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
            CreateMap<RoomDTO, RoomModel>();
            CreateMap<RoomModel, RoomDTO>();
            CreateMap<SeatTypeModel, SeatTypeDTO>();
            CreateMap<SeatTypeDTO, SeatTypeModel>();
            CreateMap<SeatDTO, SeatModel>();
            CreateMap<SeatModel, SeatDTO>();
            CreateMap<ShowDTO, ShowModel>();
            CreateMap<ShowModel, ShowDTO>().ForMember(dest => dest.FilmName, opt => opt.MapFrom(src => src.Film.Name))
                                            .ForMember(dest => dest.Director, opt => opt.MapFrom(src => src.Film.Director))
                                           .ForMember(dest => dest.AgeLimit, opt => opt.MapFrom(src => src.Film.AgeLimit))
                                           .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.Room.Id))
                                           .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room.Name));
            CreateMap<FoodModel, FoodDTO>();
            CreateMap<FoodDTO, FoodModel>();
            CreateMap<FoodOrderModel, FoodOrderDTO>().ForMember(dest => dest.FoodDTO, opt => opt.MapFrom(src => src.FoodModel));
            CreateMap<FoodOrderDTO, FoodOrderModel>();
            CreateMap<BillDTO, BillModel>();
            CreateMap<BillModel, BillDTO>().ForMember(dest => dest.FoodOrderDTOs, opt => opt.MapFrom(src => src.FoodOrderModels));
            CreateMap<ReservationModel, ReservationDTO>();
        }
    }
}
