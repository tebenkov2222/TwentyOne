using System;

namespace Connecting.View
{
    public interface IConnectingView
    {
        public event Action TryConnect;
        public void ShowConnectingWindow();
        public void Connected();
        public void ErrorConnect();
    }
}