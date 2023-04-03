using System;
using System.Collections.Generic;
using LiteNetLib;
using LiteNetLib.Utils;
using MessagePack;

namespace ServerFramework
{
    public class Server<T>: IServer<T> where T: IPacket
    {
        private const DeliveryMethod _deliveryMethod = DeliveryMethod.ReliableOrdered;
        public event ChangeConnectionServerHandler ClientConnected;
        public event ChangeConnectionServerHandler ClientDisconnected;
        public event IServer<T>.ReceivePacketHandler ReceivePacket;
        
        private NetDataWriter _netDataWriter;
        private EventBasedNetListener _netListener;
        private NetManager _netManager;

        private Configuration _configuration;
        private Dictionary<int, NetPeer> _clients;
        private bool _isEnabled;
        public bool IsEnabled => _isEnabled;

        public Server()
        {
            _netListener = new EventBasedNetListener();
            _netListener = new EventBasedNetListener();
            _netDataWriter = new NetDataWriter();
            _netManager = new NetManager(_netListener);
            _clients = new Dictionary<int, NetPeer>();
        }

        public void Config(Configuration configuration)
        {
            _configuration = configuration;
        }

        public void Start()
        {
            _netListener.ConnectionRequestEvent += NetListenerOnConnectionRequestEvent;
            _netListener.PeerConnectedEvent += NetListenerOnPeerConnectedEvent;
            _netListener.PeerDisconnectedEvent += NetListenerOnPeerDisconnectedEvent;
            _netListener.NetworkReceiveEvent += NetListenerOnNetworkReceiveEvent;
            _netManager.Start(_configuration.Port);
            _isEnabled = true;
            Console.WriteLine("Server Started");
        }

        public void Update()
        {
            if(!_isEnabled) return;
            _netManager.PollEvents();
        }

        public void Stop()
        {
            Console.WriteLine("Server Stoped");

            _netManager.Stop();
            _isEnabled = false;
            _netListener.ConnectionRequestEvent -= NetListenerOnConnectionRequestEvent;
            _netListener.PeerConnectedEvent -= NetListenerOnPeerConnectedEvent;
            _netListener.PeerDisconnectedEvent -= NetListenerOnPeerDisconnectedEvent;
            _netListener.NetworkReceiveEvent -= NetListenerOnNetworkReceiveEvent;
        }

        public void SendPacket(int id, T packet)
        {
            if (!_clients.ContainsKey(id)) throw new Exception("Client is not found");
            if (packet == null) throw new NullReferenceException("Packet was be not null");
            var serializePacket = MessagePackSerializer.Serialize(packet);
            _netDataWriter.Reset();
            _netDataWriter.PutBytesWithLength(serializePacket);
            _clients[id].Send(_netDataWriter, _deliveryMethod);
        }

        private void NetListenerOnNetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliverymethod)
        {
            var boolArray = reader.GetBytesWithLength();
            var packet = MessagePackSerializer.Deserialize<T>(boolArray);
            ReceivePacket?.Invoke(peer.Id, packet);
        }

        private void NetListenerOnPeerConnectedEvent(NetPeer peer)
        {
            _clients.Add(peer.Id, peer);
            ClientConnected?.Invoke(peer.Id);
        }

        private void NetListenerOnPeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectinfo)
        {
            _clients.Remove(peer.Id);
            ClientDisconnected?.Invoke(peer.Id);
        }

        private void NetListenerOnConnectionRequestEvent(ConnectionRequest request)
        {
            request.AcceptIfKey(_configuration.ConnectionKey);
        }
    }
}