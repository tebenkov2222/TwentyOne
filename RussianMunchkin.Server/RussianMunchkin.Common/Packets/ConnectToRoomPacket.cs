using MessagePack;
using RussianMunchkin.Common.Models;

namespace RussianMunchkin.Common.Packets
{
    [MessagePackObject]
    public class ConnectToRoomPacket: Packet
    {
        [Key(0)]
        public RoomInfoModel RoomInfoModel { get; set; }
    }
}