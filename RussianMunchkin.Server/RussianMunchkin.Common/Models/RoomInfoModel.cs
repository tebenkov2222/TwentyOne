using MessagePack;

namespace RussianMunchkin.Common.Models
{
    [MessagePackObject]
    public class RoomInfoModel
    {
        [Key(0)]
        public bool IsPrivate { get; set; }
        [Key(1)]
        public string Uid { get; set; }
        [Key(2)]
        public string Password { get; set; }
        [Key(3)]
        public PlayerInfoModel AdminPlayer { get; set; }
        [Key(4)]
        public int MaxCountPlayers { get; set; }
        [Key(5)]
        public int CurrentCountPlayers { get; set; }
    }
}