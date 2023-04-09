using RussianMunchkin.Server.Core.Player.Models;

namespace RussianMunchkin.Server.Core.Player.Interfaces
{
    public interface IPlayer
    {
        public PlayerModel PlayerModel { get; }

        public void LogIn(string playerLogin);
    }
}