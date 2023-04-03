using System;
using RussianMunchkin.Common.Models;
using UnityEngine;
using UnityEngine.UI;
using View.Views.Window;

namespace MatchMaking.Rooms.View.Windows.Lobby
{
    public class LobbyWindow: WindowBase
    {
        public event Action<bool> ChangeReadyStatus;
        public event Action ExitFromRoom;
        public event Action GameStarted;
        [SerializeField] private Text _uidText;
        [SerializeField] private Text _passwordText;
        [SerializeField] private Toggle _isPrivateToggle;
        [SerializeField] private Text _countPlayerText;

        [SerializeField] private Button _exitFromRoomButton;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _readyButton;

        [SerializeField] private GameObject _readyButtonContent;
        [SerializeField] private GameObject _notReadyButtonContent;
        [Header("ListPlayer")] 
        [SerializeField] private LobbyPlayerItem _lobbyPlayerItemPrefab;
        [SerializeField] private LobbyPlayerItemsGroupView _itemsGroupView;
        
        private int _maxCountPlayers;
        private int _playerIdAdmin;
        private bool _isReady = false;

        public bool IsReady
        {
            get => _isReady;
            set
            {
                if (_isReady != value)
                {
                    ChangeReadyStatus?.Invoke(value);
                }
                _isReady = value;
                ShowIsReadyButton(value);
            }
        }

        private void Awake()
        {
            ResetIsReady();
        }

        private void OnEnable()
        {
            _readyButton.onClick.AddListener(ReadyButtonOnClick);
            _startGameButton.onClick.AddListener(StartGameButtonOnClick);
            _exitFromRoomButton.onClick.AddListener(ExitFromRoomButtonOnClick);
        }

        private void OnDisable()
        {
            ResetIsReady();
            SetMeIsAdmin(false);
            _itemsGroupView.RemoveAll();
            ChangeStatusStartGame(false);
            _startGameButton.onClick.RemoveListener(StartGameButtonOnClick);
            _readyButton.onClick.RemoveListener(ReadyButtonOnClick);
            _exitFromRoomButton.onClick.RemoveListener(ExitFromRoomButtonOnClick);
        }

        private void ResetIsReady()
        {
            _isReady = false;
            ShowIsReadyButton(_isReady);
        }
        
        private void ShowIsReadyButton(bool isReady)
        {
            _readyButtonContent.SetActive(isReady);
            _notReadyButtonContent.SetActive(!isReady);
        }
        
        private void StartGameButtonOnClick()
        {
            GameStarted?.Invoke();
        }
        
        private void ReadyButtonOnClick()
        {
            IsReady = !IsReady;
        }
        
        private void ExitFromRoomButtonOnClick()
        {
            ExitFromRoom?.Invoke();
        }
        
        public void ConnectedToRoom(RoomInfoModel model)
        {
            Debug.Log($"ConnectedToRoom with MaxCountPlayers = {model.MaxCountPlayers}");
            _uidText.text = model.Uid;
            _passwordText.text = model.Password;
            _isPrivateToggle.isOn = model.IsPrivate;
            _maxCountPlayers = model.MaxCountPlayers;
            SetAdmin(model.AdminPlayer);
        }
        
        public void SetAdmin(PlayerInfoModel playerInfoModel)
        {
            SetAdmin(playerInfoModel.PlayerId);
        }
        
        public void SetAdmin(int playerId)
        {
            _playerIdAdmin = playerId;
            foreach (var scrollBarItem in _itemsGroupView)
            {
                scrollBarItem.Value.SetAdmin(scrollBarItem.Key == playerId);
            }
        }

        public void EnterPlayerToRoom(PlayerInfoModel playerInfoModel)
        {
            Debug.Log("EnterPlayerToRoom");
            var item = _itemsGroupView.Add(playerInfoModel.PlayerId, _lobbyPlayerItemPrefab);
            item.Init(playerInfoModel.Username, playerInfoModel.PlayerId == _playerIdAdmin, playerInfoModel.IsReady);
            ChangeCountPlayer();
        }

        public void LeftPlayerFromRoom(PlayerInfoModel playerInfoModel)
        {
            _itemsGroupView.RemoveItem(playerInfoModel.PlayerId);
            ChangeCountPlayer();
        }

        public void ChangeStatusReady(int playerId, bool isReady)
        {
            _itemsGroupView[playerId].ChangeStatusReady(isReady);
        }

        public void ChangeStatusStartGame(bool isReady)
        {
            _startGameButton.interactable = isReady;
        }

        public void SetMeIsAdmin(bool isAdmin)
        {
            //SetAdmin(_playerModel);
            _startGameButton.gameObject.SetActive(isAdmin);
        }

        private void ChangeCountPlayer()
        {
            _countPlayerText.text = $"{_itemsGroupView.Count}/{_maxCountPlayers}";
        }
    }
}