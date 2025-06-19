using Domain.DTOs.User;
using Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDTO, User>()
                .ForMember(destino => destino.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(destino => destino.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(destino => destino.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(destino => destino.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(destino => destino.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<User, CreateUserDTO>()
                 .ForMember(destino => destino.Password, opt => opt.MapFrom(src => src.Password))
                 .ForMember(destino => destino.Role, opt => opt.MapFrom(src => src.Role))
                 .ForMember(destino => destino.Name, opt => opt.MapFrom(src => src.Name))
                 .ForMember(destino => destino.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<UpdateUserDTO, User>()
                .ForMember(destino => destino.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(destino => destino.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(destino => destino.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
