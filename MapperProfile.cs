using AutoMapper;
using System.Dynamic;
using IdentityWebApiSample.Server.Dtos;
using IdentityWebApiSample.Server.Entities;
using IdentityWebApiSample.Server.Dtos.Login;
using IdentityWebApiSample.Server.Dtos.Register;

namespace IdentityWebApiSample.Server
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<LoginRequestCreateDto, LoginRequest>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dst, srcMember) => srcMember != null));
            CreateMap<LoginRequest, LoginRequestDtos>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dst, srcMember) => srcMember != null));
            CreateMap<LoginRequestDtos, LoginRequest>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dst, srcMember) => srcMember != null));

            CreateMap<RegisterRequestCreateDto, RegisterRequest>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dst, srcMember) => srcMember != null));
            CreateMap<RegisterRequest, RegisterRequestDtos>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dst, srcMember) => srcMember != null));
            CreateMap<RegisterRequestDtos, RegisterRequest>().ReverseMap()
                .ForAllMembers(opt => opt.Condition((src, dst, srcMember) => srcMember != null));
        }
    }
}
