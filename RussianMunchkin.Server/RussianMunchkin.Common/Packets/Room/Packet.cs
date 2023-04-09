using RussianMunchkin.Common.Models;
using RussianMunchkin.Common.Packets.Game;
using RussianMunchkin.Common.Packets.Room;
using ServerFramework;

namespace RussianMunchkin.Common.Packets
{
    [MessagePack.Union(3002, typeof(CreateRoomPacket))]
    [MessagePack.Union(3003, typeof(ConnectToRoomPacket))]
    [MessagePack.Union(3004, typeof(RequestConnectToRoomPacket))]
    [MessagePack.Union(3005, typeof(ExitFromRoomPacket))]
    [MessagePack.Union(3006, typeof(ChangeConnectionToRoomPacket))]
    [MessagePack.Union(3007, typeof(ChangeStatusStartGamePacket))]
    [MessagePack.Union(3008, typeof(ChangeStatusReadyPlayerPacket))]
    [MessagePack.Union(3009, typeof(ChangeAdminRoomPacket))]
    [MessagePack.Union(3010, typeof(ChangeStatusGamePacket))]
    [MessagePack.Union(3011, typeof(GetListPublicRooms))]
    [MessagePack.Union(3012, typeof(SendListPublicRooms))]

    public abstract partial class Packet: IPacket
    {
        
    }
}