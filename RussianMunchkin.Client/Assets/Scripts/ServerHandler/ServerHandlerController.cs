using Connecting;
using ServerHandler.View;

namespace ServerHandler
{
    public class ServerHandlerController
    {
        private readonly IServerHandlerView _view;

        public ServerHandlerController(IServerHandlerView view)
        {
            _view = view;
        }
        public void Failed(string log)
        {
            _view.Failed(log);
        }
        public void ChangeConnection(bool isConnected)
        {
            _view.ChangeConnection(isConnected);
        }
    }
}