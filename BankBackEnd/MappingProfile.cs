using AutoMapper;
using BankBackEnd.Dtos;
using BankBackEnd.Models;

namespace BankBackEnd
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // TODO : Have mapped out DTO's to models and visa versa
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}