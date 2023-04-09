using AutoMapper;
using RussianMunchkin.Common.Models;
using RussianMunchkin.Common.Packets.Auth;
using RussianMunchkin.Server.Core.Player;
using RussianMunchkin.Server.DataBase.Models;
using RussianMunchkin.Server.Game.TwentyOne.Player;
using RussianMunchkin.Server.Player.Auth.AuthFull;

namespace RussianMunchkin.Server.Core
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<AuthorizationResultPacket, AuthorizationModel>().ReverseMap();
            CreateMap<AuthorizationResultPacket, RegistrationModel>().ReverseMap();
            CreateMap<AuthorizationResultPacket, AuthorizationModel>().ReverseMap();
            CreateMap<AuthorizationResultPacket, PlayerAuthFullModel>().ReverseMap();

            CreateMap<AuthorizationPacket, AuthorizationModel>().ReverseMap();
            CreateMap<RegistrationPacket, RegistrationModel>().ReverseMap();

            CreateMap<CheckLoginModel, CheckLoginPacket>().ReverseMap();
            
            CreateMap<PlayerControllerBase, PlayerInfoModel>()
                .ForMember(dest => dest.Login, 
                    opt => opt.MapFrom(src => src.PlayerModel.Login))
                .ForMember(dest => dest.IsReady, 
                    opt => opt.MapFrom(src => src.RoomsController.PlayerRoomModel.IsReadyStartGame));
        }
    }
}