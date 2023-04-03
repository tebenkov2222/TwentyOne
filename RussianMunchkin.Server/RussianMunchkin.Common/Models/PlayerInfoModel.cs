using MessagePack;
using RussianMunchkin.Common.Packets;

namespace RussianMunchkin.Common.Models
{
    [MessagePackObject]
    public class PlayerInfoModel: Packet
    {
        [Key(0)]
        public string Username { get; set; }
        [Key(1)]
        public int PlayerId { get; set; }
        [Key(2)]
        public bool IsReady { get; set; }
    }
}