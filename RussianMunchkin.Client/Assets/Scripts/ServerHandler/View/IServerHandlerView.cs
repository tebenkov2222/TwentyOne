namespace ServerHandler.View
{
    public interface IServerHandlerView
    {
        public void Failed(string log);
        public void ChangeConnection(bool isConnected);
    }
}