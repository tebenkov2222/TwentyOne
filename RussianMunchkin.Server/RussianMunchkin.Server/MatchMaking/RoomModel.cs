namespace RussianMunchkin.Server.MatchMaking
{
    public class RoomModel
    {
        public bool IsPrivate { get; set; }
        public bool IsLocked { get; set; }
        public string Uid { get; set; }
        public string Password { get; set; }
        public int PlayerIdAdmin { get; set; }
        public int MaxCountPlayers { get; set; }
    }
}