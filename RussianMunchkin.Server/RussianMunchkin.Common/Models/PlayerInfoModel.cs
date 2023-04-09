using MessagePack;
using RussianMunchkin.Common.Packets;

namespace RussianMunchkin.Common.Models
{
    [MessagePackObject]
    public class PlayerInfoModel
    {
        [Key(0)]
        public string Login { get; set; }
        [Key(1)]
        public bool IsReady { get; set; }
    }
}