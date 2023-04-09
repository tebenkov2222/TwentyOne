using Auth.Full;
using Auth.Full.View.Interfaces;
using AutoMapper;
using Configuration;
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
using View;

namespace Core
{
    public class ClientLogic
    {
        private IPacketsHandler _packetsHandler;

        private readonly Peer _peer;
        private readonly IClient<Packet> _client;
        private readonly IConnectingView _connectingView;
        private readonly IServerHandlerView _serverHandlerView;
        private readonly IAuthFullView _authFullView;
        private readonly IRoomsView _roomsView;
        private readonly IGameView _gameView;
        private readonly ConfigurationSo _configurationSo;

        private readonly ConnectingController _connectingController;
        private readonly ServerHandlerController _serverHandlerController;
        private readonly AuthFullController _authFullController;
        private readonly RoomsController _roomsController;
        private readonly GameController _gameController;
        
        private readonly PlayerModel _playerModel;
        private readonly RoomModel _roomModel;
        private readonly Mapper _mapper;

        public ClientLogic(
            UIManager uiManager,
            Peer peer,
            IClient<Packet> client,
            ConfigurationSo configurationSo)
        {
            _peer = peer;
            _client = client;
            _connectingView = uiManager.ConnectingView;
            _roomsView = uiManager.RoomsView;
            _gameView = uiManager.GameView;
            _serverHandlerView = uiManager.ServerHandlerView;
            _authFullView = uiManager.AuthFullView;
            _configurationSo = configurationSo;

            _playerModel = new PlayerModel();
            _roomModel = new RoomModel();

            _mapper = new Mapper(new MapperConfiguration(cfg
                => cfg.AddProfile(new AutoMappingProfile())));
            
            
            _connectingController = new ConnectingController(_client,_connectingView);
            _connectingController.Init();
            _serverHandlerController = new ServerHandlerController(_serverHandlerView);
            _authFullController = new AuthFullController(_peer, _authFullView, _playerModel, _mapper);
            _roomsController = new RoomsController(_peer, _roomsView, _playerModel, _roomModel);
            _gameController = new GameController(_peer, _gameView, _playerModel, _roomModel);
            InitPacketHandler();
        }

        private void InitPacketHandler()
        {
            var packetHandler = new PacketHandler();
            var responsePacketHandler = new ResponsePacketHandler(packetHandler, _peer);
            var authFullPacketHandler= new AuthFullPacketHandler(responsePacketHandler, _peer, _authFullController, _mapper);
            var roomPacketsHandler = new RoomPacketsHandler(authFullPacketHandler, _peer, _roomsController);
            var gamePacketsHandler = new GamePacketsHandler(roomPacketsHandler, _peer, _gameController, _roomModel);
            _packetsHandler = gamePacketsHandler;
        }

        public void Enable()
        {
            _connectingController.Connect();

            _peer.FailureResponse+=PeerOnFailureResponse;
            
            _connectingController.Enable();
            _authFullController.Enable();
            _roomsController.Enable();
            _gameController.Enable();
        }

        public void Update()
        {
            _gameController.Update();
        }

        public void Disable()
        {
            _connectingController.Disconnect();
            
            _peer.FailureResponse-=PeerOnFailureResponse;

            _connectingController.Disable();
            _authFullController.Disable();
            _roomsController.Disable();
            _gameController.Disable();
        }

        private void PeerOnFailureResponse(string log)
        {
            _serverHandlerController.Failed(log);
        }
        public void ConnectedToServer()
        {
            _serverHandlerController.ChangeConnection(true);
            _peer.ChangeConnection(true);
        }
        public void HandlePacket(Packet packet)
        {
            _packetsHandler.Handle(packet);
        }

        public void DisconnectedFromServer()
        {
            _serverHandlerController.ChangeConnection(false);

            _peer.ChangeConnection(false);
        }
    }
}