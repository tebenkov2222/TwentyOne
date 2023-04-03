using RussianMunchkin.Server.Core.Player;
using RussianMunchkin.Server.Game.Player.Interfaces;
using RussianMunchkin.Server.Player.Auth.AuthUsername;
using RussianMunchkin.Server.Player.MatchMaking.Rooms;

namespace RussianMunchkin.Server.Game.TwentyOne.Player
{
    public class PlayerControllerBase: PlayerBase<IClientAuthUsernameController, IClientRoomsController, IPlayerGameController>
    {
        protected PlayerAuthUsernameModel _authModel;
        protected ClientRoomsModel _roomsModel;

        public PlayerAuthUsernameModel AuthModel => _authModel;
        public ClientRoomsModel RoomsModel => _roomsModel;
        public PlayerControllerBase()
        {
            _authModel = new PlayerAuthUsernameModel();
            _roomsModel = new ClientRoomsModel();
        }

    }
}