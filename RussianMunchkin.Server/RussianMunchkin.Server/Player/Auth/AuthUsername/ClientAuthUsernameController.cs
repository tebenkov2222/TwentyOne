using RussianMunchkin.Server.Game.Player.Interfaces;

namespace RussianMunchkin.Server.Player.Auth.AuthUsername
{
    public class ClientAuthUsernameController: IClientAuthUsernameController
    {
        private PlayerAuthUsernameModel _authModel;

        public PlayerAuthUsernameModel AuthModel => _authModel;

        public ClientAuthUsernameController(PlayerAuthUsernameModel authModel)
        {
            _authModel = authModel;
        }

        public void SetUsername(string username)
        {
            _authModel.Username = username;
        }
    }
}