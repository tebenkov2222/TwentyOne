using RussianMunchkin.Server.Core.Player.Interfaces;
using RussianMunchkin.Server.Core.Player.Models;
using RussianMunchkin.Server.Game.Player.Interfaces;
using RussianMunchkin.Server.Player.Auth.AuthFull;
using RussianMunchkin.Server.Player.MatchMaking.Rooms;

namespace RussianMunchkin.Server.Core.Player
{
    public class PlayerControllerBase: IPlayer
    {
        protected IClientAuthFullController _authController;
        protected IPlayerRoomsController _roomsController;
        protected IPlayerGameController _gameController;
        
        protected PlayerModel _playerModel;
        
        public IClientAuthFullController AuthController => _authController;
        public IPlayerRoomsController RoomsController => _roomsController;
        public IPlayerGameController GameController => _gameController;
        
        public PlayerModel PlayerModel => _playerModel;

        protected PlayerControllerBase()
        {
            _playerModel = new PlayerModel();
        }

        public void LogIn(string playerLogin)
        {
            _playerModel.Login = playerLogin;
        }
        public static explicit operator string(PlayerControllerBase player)
        {
            return player.PlayerModel.Login;
        }
    }
}