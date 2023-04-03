using RussianMunchkin.Server.Core.Player.Interfaces;

namespace RussianMunchkin.Server.Player.Auth.AuthUsername
{
    public interface IClientAuthUsernameController: IClientAuth
    {
        public PlayerAuthUsernameModel AuthModel { get; }

        public void SetUsername(string username);
    }
}