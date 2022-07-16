using AutoMapper;
using DataPostAPI.Models;

namespace DataPostAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
            CreateMap<Client, ClientModel>();
            CreateMap<RegisterModelClient, Client>();
            CreateMap<UpdateModelClient, Client>();
        }
    }

}
