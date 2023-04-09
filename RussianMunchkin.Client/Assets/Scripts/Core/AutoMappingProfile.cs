using Auth.Full.Model;
using Auth.Full.View;
using AutoMapper;
using RussianMunchkin.Common.Packets.Auth;

namespace Core
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<AuthorizationPacket, LogInViewModel>().ReverseMap();
            CreateMap<AuthorizationResultPacket, AuthorizationUserModel>().ReverseMap();
            CreateMap<RegistrationPacket, RegistrationViewModel>().ReverseMap();
        }
    }
}