using System;

namespace RussianMunchkin.Server.Player.Auth.AuthFull
{
    public class PlayerAuthFullController: IClientAuthFullController
    {
        private PlayerAuthFullModel _authModel;

        public PlayerAuthFullModel AuthModel => _authModel;

        public PlayerAuthFullController()
        {
            _authModel = new PlayerAuthFullModel();
            LogOut();
        }

        public void LogIn(PlayerAuthFullModel authFullModel)
        {
            Console.WriteLine($"SetUserModel login = {authFullModel.Login}, userId = {authFullModel.UserId}");
            _authModel.IsAuthorized = true;
            _authModel.UserId = authFullModel.UserId;
            _authModel.Login = authFullModel.Login;
        }

        public void LogOut()
        {
            _authModel.IsAuthorized = false;
            _authModel.UserId = -1;
            _authModel.Login = default;
        }
    }
}