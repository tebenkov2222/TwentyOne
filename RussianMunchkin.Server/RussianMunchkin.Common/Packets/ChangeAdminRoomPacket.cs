using MessagePack;
using RussianMunchkin.Common.Models;

namespace RussianMunchkin.Common.Packets
{
    [MessagePackObject]
    public class ChangeAdminRoomPacket: Packet
    {
        [Key(0)]
        public PlayerInfoModel PlayerInfoModel { get; set; }
    }
}