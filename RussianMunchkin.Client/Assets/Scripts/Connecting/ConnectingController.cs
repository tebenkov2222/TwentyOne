using Connecting.View;
using RussianMunchkin.Common.Packets;
using RussianMunchkin.Common.Time;
using ServerFramework;
using UnityEngine;

namespace Connecting
{
    public class ConnectingController
    {
        private const int _checkIntervalMillis = 5000;
        private const int _maxCountTryingConnecti = 5;
        private readonly IClient<Packet> _client;
        private readonly IConnectingView _view;
        private TimerController _timerController;
        private int _countTryingConnect;
        
        public ConnectingController(IClient<Packet> client, IConnectingView view)
        {
            _client = client;
            _view = view;
        }

        public void Init()
        {
            _timerController = TimersManager.GenerateTimer(_checkIntervalMillis);
        }
        public void Enable()
        {
            _view.TryConnect+=ViewOnTryConnect;
            
            _timerController.Elapsed+=TimerControllerOnElapsed;
            _client.Connected+=ClientOnConnected;
            _client.Disconnected+=ClientOnDisconnected;
        }

        public void Disable()
        {
            _view.TryConnect-=ViewOnTryConnect;
            
            _timerController.Elapsed-=TimerControllerOnElapsed;
            _client.Connected-=ClientOnConnected;
            _client.Disconnected-=ClientOnDisconnected;
        }

        public void Connect()
        {
            TryConnect();
        }

        public void Disconnect()
        {
            _client.Disconnect();
        }
        private void ViewOnTryConnect()
        {
            RestartTryingConnect();
            TryConnect();
        }

        private void TimerControllerOnElapsed()
        {
            if (!_client.IsConnected)
                TryConnect();
        }

        private void ClientOnConnected()
        {
            RestartTryingConnect();
            _view.Connected();
            _timerController.Stop();
            _timerController.Reset();
        }

        private void ClientOnDisconnected()
        {
            TryConnect();
        }

        public void RestartTryingConnect()
        {
            _countTryingConnect = 0;
        }
        private void TryConnect()
        {
            _view.ShowConnectingWindow();
            _countTryingConnect++;
            Debug.Log($"Try Connect {_countTryingConnect}");
            if (_countTryingConnect >= _maxCountTryingConnecti)
            {
                _view.ErrorConnect();
            }
            else
            {
                _client.Connect();
                _timerController.Start();   
            }
        }
    }
}