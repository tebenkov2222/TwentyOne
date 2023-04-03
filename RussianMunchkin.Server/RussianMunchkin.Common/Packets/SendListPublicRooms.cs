using System.Collections.Generic;
using MessagePack;
using RussianMunchkin.Common.Models;

namespace RussianMunchkin.Common.Packets
{
    [MessagePackObject]
    public class SendListPublicRooms: Packet
    {
        [Key(0)]
        public List<RoomInfoModel> Rooms { get; set; }
    }
}