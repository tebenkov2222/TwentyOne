using System.Collections.Generic;
using RussianMunchkin.Server.Core.Player.Server;
using RussianMunchkin.Server.Core.Player.Server.Models;
using RussianMunchkin.Server.Game.TwentyOne.Player.Models;
using RussianMunchkin.Server.Game.TwentyOne.Player.Server.Controllers;
using RussianMunchkin.Server.Player.Auth.AuthUsername;
using RussianMunchkin.Server.Player.MatchMaking.Rooms;
using RussianMunchkin.Server.Server;

namespace RussianMunchkin.Server.Game.TwentyOne.Player.Server
{
    public class ServerPlayerToController: PlayerControllerBase, INetPlayer
    {
        private readonly NetPeer _netPeer;
        private readonly NetPlayerModel _netModel;
        private readonly PlayerToGameModel _gameModel;

        public NetPlayerModel NetModel => _netModel;
        public NetPeer NetPeer => _netPeer;

        public ServerPlayerToController(NetPeer netPeer, int netId): base()
        {
            _netPeer = netPeer;
            _gameModel = new PlayerToGameModel();
            _netModel = new NetPlayerModel();
            _netModel.NetId = netId;

            _authController = new ClientAuthUsernameController(_authModel);
            _roomsController = new ServerClientRoomsController(_netPeer, _playerModel, _roomsModel);
            _gameController = new ServerPlayerGameToController(_netPeer, _playerModel,_gameModel);

        }
    }
}