using RussianMunchkin.Common.Models;
using RussianMunchkin.Common.Packets.Game;
using ServerFramework;

namespace RussianMunchkin.Common.Packets
{
    [MessagePack.Union(0, typeof(ResponsePacket))]
    [MessagePack.Union(1, typeof(JoinPacket))]
    [MessagePack.Union(2, typeof(CreateRoomPacket))]
    [MessagePack.Union(3, typeof(ConnectToRoomPacket))]
    [MessagePack.Union(4, typeof(RequestConnectToRoomPacket))]
    [MessagePack.Union(5, typeof(ExitFromRoomPacket))]
    [MessagePack.Union(6, typeof(ChangeConnectionToRoomPacket))]
    [MessagePack.Union(7, typeof(ChangeStatusStartGamePacket))]
    [MessagePack.Union(8, typeof(ChangeStatusReadyPlayerPacket))]
    [MessagePack.Union(9, typeof(ChangeAdminRoomPacket))]
    [MessagePack.Union(10, typeof(ChangeStatusGamePacket))]
    [MessagePack.Union(11, typeof(GetListPublicRooms))]
    [MessagePack.Union(12, typeof(SendListPublicRooms))]
    
    [MessagePack.Union(13, typeof(PlayerReadyGamePacket))]
    [MessagePack.Union(14, typeof(PlayerReceivingNumberPacket))]
    [MessagePack.Union(15, typeof(PlayerTokeNumberPacket))]
    [MessagePack.Union(16, typeof(RequestGetNumberPacket))]
    [MessagePack.Union(17, typeof(ShowResultsPacket))]
    [MessagePack.Union(18, typeof(StartSessionPacket))]
    [MessagePack.Union(19, typeof(RestartSessionPacket))]
    
    
    public abstract class Packet: IPacket
    {
        
    }
}