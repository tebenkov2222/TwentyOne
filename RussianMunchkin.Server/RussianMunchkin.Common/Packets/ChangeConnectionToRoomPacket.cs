using MessagePack;
using RussianMunchkin.Common.Models;

namespace RussianMunchkin.Common.Packets
{
    [MessagePackObject]
    public class ChangeConnectionToRoomPacket: Packet
    {
        [Key(0)]
        public PlayerInfoModel PlayerInfoModel { get; set; } 
        [Key(1)]
        public bool IsConnected { get; set; }
    }
}