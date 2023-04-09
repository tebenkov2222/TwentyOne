using AutoMapper;
using Configuration;
using RussianMunchkin.Common.Packets;
using ServerFramework;
using View;

namespace Core
{
    public class ClientController
    {
        public bool IsConnected => _isConnected;

        private bool _isConnected;
        
        private readonly IClient<Packet> _client;
        private readonly Peer _peer;

        private ClientLogic _clientLogic;
        private readonly Mapper _mapper;

        public ClientController(UIManager uiManager,
            ConfigurationSo configurationSo)
        {
            _client = new Client<Packet>();
            _client.Config(configurationSo.ServerConfiguration);
            _peer = new Peer(_client);

            _clientLogic = new ClientLogic(uiManager, _peer, _client, configurationSo);
        }

        public void Enable()
        {
            _client.Connected+=ClientOnConnected;
            _client.Disconnected+=ClientOnDisconnected;
            _client.ReceivePacket+=ClientOnReceivePacket;

            _clientLogic.Enable();
        }

        public void Update()
        {
            _clientLogic.Update();
            _client.Update();
        }

        public void Disable()
        {
            _clientLogic.Disable();

            _client.Connected-=ClientOnConnected;
            _client.Disconnected-=ClientOnDisconnected;
            _client.ReceivePacket-=ClientOnReceivePacket;
        }

        private void ClientOnConnected()
        {
            _clientLogic.ConnectedToServer();
            _peer.ChangeConnection(true);
            _isConnected = true;
        }
        private void ClientOnReceivePacket(Packet packet)
        {
            _clientLogic.HandlePacket(packet);
        }

        private void ClientOnDisconnected()
        {
            _clientLogic.DisconnectedFromServer();
            _peer.ChangeConnection(false);
            _isConnected = false;
        }
    }
}