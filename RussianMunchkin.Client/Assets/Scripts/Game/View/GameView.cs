using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.View.Players;
using Game.View.Windows;
using MatchMaking.Rooms;
using Models;
using RussianMunchkin.Common.Models;
using UnityEngine;
using UnityEngine.UI;
using View.Shared;
using View.Views.Window;

namespace Game.View
{
    public class GameView: WindowBase, IGameView
    {
        public event Action TakeNumber;
        public event Action ReadyShow;
        public event Action ReadyRestart;
        public event Action LeaveGame;
        public event Action GameStarted;
        public event Action GameStopped;
        
        [SerializeField] private GamePlayerItem _prefabGamePlayerItem;
        [SerializeField] private Button _tokeNumberButton;
        [SerializeField] private Button _readyButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private GameWinningWindow gameWinningWindow;
        [SerializeField] private FilledImageView _turnFilledImage;
        [SerializeField] private GamePlayerItemsGroupView _itemsGroupView;
        
        private PlayerModel _playerModel;
        private RoomModel _roomModel;

        public void ControllerInit(PlayerModel playerModel,RoomModel roomModel)
        {
            _roomModel = roomModel;
            _playerModel = playerModel;
        }
        
        private void OnEnable()
        {
            _tokeNumberButton.onClick.AddListener(TokeNumberButtonOnClick);
            _readyButton.onClick.AddListener(ReadyButtonOnClick);
            _closeButton.onClick.AddListener(CloseButtonOnClick);
        }

        private void OnDisable()
        {
            _tokeNumberButton.onClick.RemoveListener(TokeNumberButtonOnClick);
            _readyButton.onClick.RemoveListener(ReadyButtonOnClick);
            _closeButton.onClick.RemoveListener(CloseButtonOnClick);

        }

        public override void Hide()
        {
            base.Hide();
            ResetGame();

        }

        private void CloseButtonOnClick()
        {
            LeaveGame?.Invoke();
        }

        private void TokeNumberButtonOnClick()
        {
            TakeNumber?.Invoke();
        }

        private void ReadyButtonOnClick()
        {
            ReadyToShow();
        }

        private void ReadyToShow()
        {
            _readyButton.interactable = false;
            PlayerReadyToShow(_playerModel.Login);
            ReadyShow?.Invoke();
        }
        public void StartGame()
        {
            gameWinningWindow.Init(_roomModel.Players);
            ResetGame();
            foreach (var player in _roomModel.Players)
            {
                CreateItem(player);
            }
            GameStarted?.Invoke();
        }

        public void StopGame()
        {
            GameStopped?.Invoke();
        }

        public void UpdateTimeTurn(float value)
        {
            _turnFilledImage.UpdateValue(value);
        }

        public void EndTurn()
        {
            LockTurn();
            _turnFilledImage.UpdateValue(0);
            ReadyToShow();
        }

        public void LockTurn()
        {
            _tokeNumberButton.interactable = false;
        }

        private void CreateItem(PlayerInfoModel playerInfoModel)
        {
            var playerItem = _itemsGroupView.Add(playerInfoModel.Login, _prefabGamePlayerItem);
            playerItem.Show(playerInfoModel);
        }
        public void ReceiveNumber(int number)
        {
            _itemsGroupView[_playerModel.Login].TakenNumber(number);
        }

        public async void ShowResults(List<GamePlayerInfoModel> results)
        {
            Debug.Log($"Res count = {results.Count}");
            foreach (var result in results)
            {
                var playerView = _itemsGroupView[result.Login];
                if(_playerModel.Login != result.Login) await playerView.ShowResult(result);
            }

            await Task.Delay(1000);
            gameWinningWindow.Show();
            gameWinningWindow.ShowResults(results);
            await Task.Delay(2000);
            gameWinningWindow.Hide();
            ReadyRestart?.Invoke();
        }
        public void PlayerTokedNumber(string playerLogin)
        {
            _itemsGroupView[playerLogin].TakenNumber();
        }

        public void PlayerReadyToShow(string playerLogin)
        {
            _itemsGroupView[playerLogin].ReadyShow();
        }

        public void ResetGame()
        {
            _tokeNumberButton.interactable = true;
            _readyButton.interactable = true;
            _itemsGroupView.RemoveAll();
        }
        public void RestartGame()
        {
            _tokeNumberButton.interactable = true;
            _readyButton.interactable = true;
            foreach (var playerView in _itemsGroupView)
            {
                playerView.Value.Reset();
            }
        }
    }
}