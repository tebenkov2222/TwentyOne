using RussianMunchkin.Server.Core.Player.Interfaces;
using RussianMunchkin.Server.Core.Player.Models;

namespace RussianMunchkin.Server.Core.Player
{
    public abstract class PlayerBase<TAuth, TRooms, TGame>: IPlayer where TAuth: IClientAuth where TRooms: IClientRooms where TGame: IClientGame
    {
        protected TAuth _authController;
        protected TRooms _roomsController;
        protected TGame _gameController;
        
        protected PlayerModel _playerModel;
        
        public TAuth AuthController => _authController;
        public TRooms RoomsController => _roomsController;
        public TGame GameController => _gameController;
        
        public PlayerModel PlayerModel => _playerModel;

        protected PlayerBase()
        {
            _playerModel = new PlayerModel();
        }

        public void SetPlayerId(int playerId)
        {
            _playerModel.PlayerId = playerId;
        }
    }
}