using Auth.Full.View;
using Connecting.View;
using Game.View;
using MatchMaking.Rooms.View;
using ServerHandler.View;
using UnityEngine;
using View.Views.Error;

namespace View
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private ServerHandlerView _serverHandlerView;
        [SerializeField] private AuthFullView _authFullView;
        [SerializeField] private RoomsView _roomsView;
        [SerializeField] private ConnectingView _connectingView;
        [SerializeField] private GameView _gameView;
        [SerializeField] private MessagePanel _messagePanel;

        public AuthFullView AuthFullView => _authFullView;
        public ServerHandlerView ServerHandlerView => _serverHandlerView;
        public RoomsView RoomsView => _roomsView;
        public ConnectingView ConnectingView => _connectingView;
        public GameView GameView => _gameView;

        private void Awake()
        {
            _authFullView.Show();
            _roomsView.Init();
        }

        private void OnEnable()
        {
            _authFullView.LoginSuccessed+=ServerViewOnLoginSuccessed;
            
            _serverHandlerView.FailedResponse+=ServerViewOnFailedResponse;
            _serverHandlerView.Disconnected+=ServerViewOnDisconnected;
            
            _roomsView.RoomLeaved+=RoomsViewOnRoomLeaved;
            
            _gameView.LeaveGame+=GameViewOnLeaveGame;
            _gameView.GameStarted+=GameViewOnGameStarted;
            _gameView.GameStopped+=GameViewOnGameStopped;
        }

        private void OnDisable()
        {
            _authFullView.LoginSuccessed-=ServerViewOnLoginSuccessed;
            
            _serverHandlerView.FailedResponse-=ServerViewOnFailedResponse;
            _serverHandlerView.Disconnected-=ServerViewOnDisconnected;
            
            _roomsView.RoomLeaved-=RoomsViewOnRoomLeaved;

            _gameView.LeaveGame-=GameViewOnLeaveGame;
            _gameView.GameStarted-=GameViewOnGameStarted;
            _gameView.GameStopped-=GameViewOnGameStopped;

        }

        private void ServerViewOnFailedResponse(string log)
        {
            _messagePanel.ShowFailed(log);
        }

        private void GameViewOnGameStopped()
        {
            _messagePanel.ShowInfo("Комната была расформирована");
        }

        private void RoomsViewOnRoomLeaved()
        {
            _gameView.Hide();
        }

        private void GameViewOnGameStarted()
        {
            RoomsView.Hide();
            GameView.Show();
        }

        private void GameViewOnLeaveGame()
        {
            RoomsView.SendRequestLeaveRoom();
            RoomsView.ShowGeneralPanel();
            GameView.Hide();
        }

        private void ServerViewOnLoginSuccessed()
        {
            Debug.Log("Server View Closed");
            _authFullView.Hide();
            _roomsView.ShowGeneralPanel();
        }

        private void ServerViewOnDisconnected()
        {
            ResetUi();
        }
        private void ResetUi()
        {
            _authFullView.Show();
            _roomsView.Hide();
            _gameView.Hide();
        }
    }
}