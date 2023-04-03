namespace ServerFramework
{
    public delegate void ChangeConnectionServerHandler(int id);
    public interface IServer<T> where T: IPacket
    {
        public delegate void ReceivePacketHandler(int id, T packet);

        public event ChangeConnectionServerHandler ClientConnected;
        public event ChangeConnectionServerHandler ClientDisconnected;
        public event ReceivePacketHandler ReceivePacket;
        public void Config(Configuration configuration);
        public void Start();
        public void Update();
        public void Stop();
        public void SendPacket(int id, T packet);
        public bool IsEnabled { get; }
    }
}