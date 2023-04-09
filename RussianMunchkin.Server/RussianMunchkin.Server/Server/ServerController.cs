using RussianMunchkin.Common.Packets;
using RussianMunchkin.Server.Configuration;
using ServerFramework;
using ServerPlayer = RussianMunchkin.Server.Game.TwentyOne.Player.Server.ServerPlayerToController;

namespace RussianMunchkin.Server.Server
{
    public class ServerController
    {
        private ConfigurationController _configurationController;
        
        private IServer<Packet> _server;
        private ServerLogic _serverLogic;
        private ServerModel<ServerPlayer> _model;

        public ServerController()
        {
            _configurationController = new ConfigurationController();
            _serverLogic = new ServerLogic(_configurationController);
            _server = new Server<Packet>();
            _server.Config(_configurationController.ConfigurationServer);
            _model = new ServerModel<ServerPlayer>();
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
            var netPeer = new Peer(_server, id);
            var serverPlayer = new ServerPlayer(netPeer);
            netPeer.ChangeConnection(true);
            _model.Clients.Add(id, serverPlayer);
            _serverLogic.ClientConnected(serverPlayer);
        }

        private void ServerOnClientDisconnected(int id)
        {
            var serverPlayer = _model.Clients[id];
            serverPlayer.Peer.ChangeConnection(false);
            _serverLogic.ClientDisconnected(serverPlayer);
            _model.Clients.Remove(id);
        }

        private void ServerOnReceivePacket(int id, Packet packet)
        {
            _serverLogic.HandlePacket(_model.Clients[id], packet);
        }
    }
}