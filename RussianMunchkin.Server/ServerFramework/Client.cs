using System;
using LiteNetLib;
using LiteNetLib.Utils;
using MessagePack;

namespace ServerFramework
{

    public class Client<T>: IClient<T> where T: IPacket
    {
        private const DeliveryMethod _deliveryMethod = DeliveryMethod.ReliableOrdered;

        private EventBasedNetListener _netListener;
        private NetManager _netManager;
        private NetDataWriter _netDataWriter;
        public event ChangeConnectionClientHandler Connected;
        public event ChangeConnectionClientHandler Disconnected;
        public event IClient<T>.ReceivePacketHandler ReceivePacket;
        private Configuration _configuration;
        private NetPeer _peer;
        private bool _isConnected;
        public bool IsConnected => _isConnected;

        public Client()
        {
            _netListener = new EventBasedNetListener();
            _netManager = new NetManager(_netListener);
            _netDataWriter = new NetDataWriter();
        }

        public void Config(Configuration configuration)
        {
            _configuration = configuration;
        }

        public void Connect()
        {
            _netListener.PeerConnectedEvent+=NetListenerOnPeerConnectedEvent;
            _netListener.PeerDisconnectedEvent+=NetListenerOnPeerDisconnectedEvent;
            _netListener.NetworkReceiveEvent+=NetListenerOnNetworkReceiveEvent;
            _netManager.Start();
            _netManager.Connect(_configuration.Host, _configuration.Port, _configuration.ConnectionKey);
        }

        public void Update()
        {
            _netManager.PollEvents();
        }

        public void Disconnect()
        {
            if(!_isConnected) return;
            _netManager.Stop();
            _netManager.DisconnectPeer(_peer);
        }

        public void SendPacket(T packet)
        {
            if (!_isConnected) throw new Exception("Not connected to server");
            if (packet == null) throw new NullReferenceException("Packet was be not null");
            var serializePacket = MessagePackSerializer.Serialize(packet);
            _netDataWriter.Reset();
            _netDataWriter.PutBytesWithLength(serializePacket);
            _peer.Send(_netDataWriter, _deliveryMethod);
        }

        private void NetListenerOnNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliverymethod)
        {
            var deserializePacket = reader.GetBytesWithLength();
            var packet = MessagePackSerializer.Deserialize<T>(deserializePacket);
            ReceivePacket?.Invoke(packet);
        }

        private void NetListenerOnPeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectinfo)
        {
            DisconnectBody();
            Disconnected?.Invoke();
        }

        private void NetListenerOnPeerConnectedEvent(NetPeer peer)
        {
            _peer = peer;
            _isConnected = true;
            Connected?.Invoke();
        }

        private void DisconnectBody()
        {
            _netManager.Stop();
            _peer = null;
            _isConnected = false;
            _netListener.PeerConnectedEvent-=NetListenerOnPeerConnectedEvent;
            _netListener.PeerDisconnectedEvent-=NetListenerOnPeerDisconnectedEvent;
            _netListener.NetworkReceiveEvent-=NetListenerOnNetworkReceiveEvent;

        }
    }
}