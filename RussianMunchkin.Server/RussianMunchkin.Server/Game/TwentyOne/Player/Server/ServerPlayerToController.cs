using RussianMunchkin.Server.Core.Player;
using RussianMunchkin.Server.Core.Player.Server;
using RussianMunchkin.Server.Game.TwentyOne.Player.Models;
using RussianMunchkin.Server.Game.TwentyOne.Player.Server.Controllers;
using RussianMunchkin.Server.Player.Auth.AuthFull;
using RussianMunchkin.Server.Player.MatchMaking.Rooms;
using RussianMunchkin.Server.Server;

namespace RussianMunchkin.Server.Game.TwentyOne.Player.Server
{
    public class ServerPlayerToController: PlayerControllerBase, INetPlayer
    {
        private readonly Peer _peer;
        private readonly PlayerToGameModel _gameModel;

        public Peer Peer => _peer;

        public ServerPlayerToController(Peer peer): base()
        {
            _peer = peer;
            _gameModel = new PlayerToGameModel();

            var roomsModel = new PlayerRoomModel();
            _authController = new PlayerAuthFullController();
            _roomsController = new ServerPlayerRoomsController(_peer, _playerModel, roomsModel);
            _gameController = new ServerPlayerGameToController(_peer, _playerModel,_gameModel);

        }
    }
}