namespace ServerFramework
{
    public delegate void ChangeConnectionClientHandler();
    public interface IClient<T> where T: IPacket
    {
        public delegate void ReceivePacketHandler(T packet);

        public event ChangeConnectionClientHandler Connected;
        public event ChangeConnectionClientHandler Disconnected;
        public event ReceivePacketHandler ReceivePacket;
        public void Config(Configuration configuration);
        public void Connect();
        public void Update();
        public void Disconnect();
        public void SendPacket(T packet);
        public bool IsConnected { get; }
    }
}