using Auth.Username;
using Auth.Username.View;
using Connecting;
using Connecting.View;
using Controllers;
using Core.Net;
using Core.PacketHandlers;
using Game;
using Game.View;
using MatchMaking.Rooms;
using MatchMaking.Rooms.View;
using Models;
using PacketHandlers.Core;
using PacketHandlers.Handlers;
using RussianMunchkin.Common.Packets;
using ServerFramework;
using ServerHandler;
using ServerHandler.View;

namespace Core
{
    public class ClientController
    {
        public bool IsConnected => _isConnected;

        private bool _isConnected;
        
        private readonly IClient<Packet> _client;
        //private readonly Configuration _configuration = Configuration.Local;
        private readonly Configuration _configuration = new Configuration()
        {
            Host = "88.87.85.226",
            Port = 8002,
            ConnectionKey = "kAs!5s"
        };

        private readonly NetPeer _netPeer;
        private IPacketsHandler _packetsHandler;

        private readonly IConnectingView _connectingView;
        private readonly IServerHandlerView _serverHandlerView;
        private readonly IAuthUsernameView _authUsernameView;
        private readonly IRoomsView _roomsView;
        private readonly IGameView _gameView;

        private readonly ConnectingController _connectingController;
        private readonly ServerHandlerController _serverHandlerController;
        private readonly AuthUsernameController _authUsernameController;
        private readonly RoomsController _roomsController;
        private readonly GameController _gameController;
        
        private readonly PlayerModel _playerModel;
        private readonly RoomModel _roomModel;

        public ClientController(IConnectingView connectingView,IServerHandlerView serverHandlerView, IAuthUsernameView authUsernameView, IRoomsView roomsView, IGameView gameView)
        {
            _client = new Client<Packet>();
            _client.Config(_configuration);
            _netPeer = new NetPeer(_client);

            _connectingView = connectingView;
            _authUsernameView = authUsernameView;
            _roomsView = roomsView;
            _gameView = gameView;
            _serverHandlerView = serverHandlerView;

            _playerModel = new PlayerModel();
            _roomModel = new RoomModel();

            _connectingController = new ConnectingController(_client,_connectingView);
            _connectingController.Init();
            _serverHandlerController = new ServerHandlerController(_serverHandlerView);
            _authUsernameController = new AuthUsernameController(_netPeer, _authUsernameView, _playerModel);
            _roomsController = new RoomsController(_netPeer, _roomsView, _playerModel, _roomModel);
            _gameController = new GameController(_netPeer, _gameView, _playerModel, _roomModel);
            InitPacketHandler();
        }

        private void InitPacketHandler()
        {
            var packetHandler = new PacketHandler();
            var responsePacketHandler = new ResponsePacketHandler(packetHandler, _netPeer);
            var roomPacketsHandler = new RoomPacketsHandler(responsePacketHandler, _netPeer, _roomsController);
            var gamePacketsHandler = new GamePacketsHandler(roomPacketsHandler, _netPeer, _gameController, _roomModel);
            _packetsHandler = gamePacketsHandler;
        }

        public void Enable()
        {
        
            _client.Connected+=ClientOnConnected;
            _client.Disconnected+=ClientOnDisconnected;
            _client.ReceivePacket+=ClientOnReceivePacket;
            _connectingController.Connect();
            
        
            _netPeer.FailureResponse+=NetPeerOnFailureResponse;
            
            _connectingController.Enable();
            _authUsernameController.Enable();
            _roomsController.Enable();
            _gameController.Enable();
        }

        public void Update()
        {
            _gameController.Update();
            _client.Update();
        }

        public void Disable()
        {
            _connectingController.Disconnect();

            _client.Connected-=ClientOnConnected;
            _client.Disconnected-=ClientOnDisconnected;
            _client.ReceivePacket-=ClientOnReceivePacket;
            
            _netPeer.FailureResponse-=NetPeerOnFailureResponse;

            _connectingController.Disable();
            _authUsernameController.Disable();
            _roomsController.Disable();
            _gameController.Disable();
        }

        private void NetPeerOnFailureResponse(string log)
        {
            _serverHandlerController.Failed(log);
        }
        private void ClientOnConnected()
        {
            _serverHandlerController.ChangeConnection(true);
            _netPeer.ChangeConnection(true);
            _isConnected = true;
        }
        private void ClientOnReceivePacket(Packet packet)
        {
            _packetsHandler.Handle(packet);
        }

        private void ClientOnDisconnected()
        {
            _serverHandlerController.ChangeConnection(false);

            _netPeer.ChangeConnection(false);
            _isConnected = false;
        }
    }
}