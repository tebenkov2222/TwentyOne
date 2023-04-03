using System;
using System.Threading;
using System.Threading.Tasks;
using Prometheus;
using RussianMunchkin.Common.Packets;
using RussianMunchkin.Server.Core.PacketsHandler.Decorator;
using RussianMunchkin.Server.Game.TwentyOne.Handlers;
using RussianMunchkin.Server.Game.TwentyOne.Player.Server;
using RussianMunchkin.Server.Handlers;
using RussianMunchkin.Server.MatchMaking;
using RussianMunchkin.Server.Metrics;
using RussianMunchkin.Server.Player;
using ServerFramework;

using ServerPlayer = RussianMunchkin.Server.Game.TwentyOne.Player.Server.ServerPlayerToController;

namespace RussianMunchkin.Server.Server
{
    public class ServerController
    {
        private IServer<Packet> _server;
        private Configuration _configurationServer = Configuration.Local;
        
        private ServerModel<ServerPlayer> _model;
        private IPacketsHandler<ServerPlayer> _packetsHandler;
        private RoomsManager _roomsManager;
        private PlayersController<ServerPlayer> _playersController;
        private Gauge  _countClientsGauge;
        public ServerModel<ServerPlayer> Model => _model;
        public ServerController()
        {
            _countClientsGauge = Prometheus.Metrics.CreateGauge("count_clients", "");

            _server = new Server<Packet>();
            _server.Config(_configurationServer);
            _model = new ServerModel<ServerPlayer>();
            _roomsManager = new RoomsManager();
            _playersController = new PlayersController<ServerPlayer>();
            InitHandlersPackets();
        }

        private void InitHandlersPackets()
        {
            var packetHandler = new PacketHandler<ServerPlayer>();
            var joinPacketHandler = new AuthUsernamePacketHandler(packetHandler);
            var roomPacketHandler =  new RoomPacketHandler(joinPacketHandler, _roomsManager);
            var gamePacketHandler = new GamePacketHandler(roomPacketHandler);
            _packetsHandler = gamePacketHandler;
        }

        public void Enable()
        {
            _server.Start();
            _server.ReceivePacket+=ServerOnReceivePacket;
            _server.ClientDisconnected+=ServerOnClientDisconnected;
            _server.ClientConnected+=ServerOnClientConnected;
        }

        public void Update()
        {
            _server.Update();
        }

        public void Disable()
        {
            _server.Stop();
            
            _server.ReceivePacket-=ServerOnReceivePacket;
            _server.ClientDisconnected-=ServerOnClientDisconnected;
        }

        private void ServerOnClientConnected(int id)
        {
            var netPeer = new NetPeer(_server, id);
            var serverPlayer = new ServerPlayer(netPeer, id);
            netPeer.ChangeConnection(true);
            _model.Clients.Add(id, serverPlayer);
            _playersController.AddPlayer(serverPlayer);
            _countClientsGauge.Inc();
            Console.WriteLine($"Connect player. Id = {id}. Counts = {_model.Clients.Count}");

        }

        private void ServerOnClientDisconnected(int id)
        {

            var serverPlayer = _model.Clients[id];
            serverPlayer.NetPeer.ChangeConnection(false);
            _roomsManager.TryRemovePlayer(serverPlayer);
            _model.Clients.Remove(id);
            _playersController.RemovePlayer(serverPlayer);
            _countClientsGauge.Dec();

            Console.WriteLine($"Disconnect player. Id = {id}. Count = {_model.Clients.Count}");

        }

        private void ServerOnReceivePacket(int id, Packet packet)
        {
            _packetsHandler.Handle(_model.Clients[id], packet);
        }
    }
}