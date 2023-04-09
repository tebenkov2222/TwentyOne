using System.Collections.Generic;
using Controllers;
using Core;
using Game.View;
using MatchMaking.Rooms;
using Models;
using RussianMunchkin.Common.Models;
using RussianMunchkin.Common.Packets.Game;
using RussianMunchkin.Common.Time;
using UnityEngine;

namespace Game
{
    public class GameController : ControllerBase
    {
        private const int _timeToTurnMillis = 10000; 
        
        private IGameView _view;
        private readonly PlayerModel _playerModel;
        private readonly RoomModel _roomModel;
        private List<PlayerInfoModel> _players;
        private TimerController _timerController;
        private bool _isGameStarted;
        private GameModel _gameModel;


        public IGameView View => _view;

        public GameController(Peer peer, IGameView view, PlayerModel playerModel, RoomModel roomModel) : base(peer)
        {
            _timerController = TimersManager.GenerateTimer(_timeToTurnMillis);
            _view = view;
            _playerModel = playerModel;
            _roomModel = roomModel;
            _gameModel = new GameModel();
            _view.ControllerInit(playerModel, roomModel);
        }
        public override void Enable()
        {
            _view.TakeNumber += ViewOnTokeNumber;
            _view.ReadyShow += ViewOnReady;
            _view.ReadyRestart += ViewOnReadyRestart;
            _timerController.Elapsed+=TimerControllerOnElapsed;
        }

        public void Update()
        {   
            if (!_isGameStarted) return;
            if (_timerController.IsEnabled)
            {
                _view.UpdateTimeTurn(1f-((float)_timerController.Millis/(float)_timerController.Interval));
            }
            else
            {
                _view.UpdateTimeTurn(0);
            }
        }

        public override void Disable()
        {
            _view.TakeNumber -= ViewOnTokeNumber;
            _view.ReadyShow -= ViewOnReady;
            _view.ReadyRestart -= ViewOnReadyRestart;
            _timerController.Elapsed-=TimerControllerOnElapsed;
            _timerController.Stop();
        }
        public void StartGame(List<PlayerInfoModel> players)
        {
            Debug.Log("StartGame");
            _isGameStarted = true;
            _players = players;
            ResetGame();
            _timerController.Start();
            _view.StartGame();
        }

        public void StopGame()
        {
            Debug.Log("StopGame");

            _timerController.Stop();
            _timerController.Reset();
            _isGameStarted = false;
            _view.StopGame();
        }

        private void TimerControllerOnElapsed()
        {
            _view.EndTurn();
        }

        private async void ViewOnReadyRestart()
        {
            await _peer.SendPacket(new RestartSessionPacket());
            //todo: response packet
        }

        private async void ViewOnReady()
        {
            _timerController.Stop();
            await _peer.SendPacket(new PlayerReadyGamePacket());
            //todo: response packet

        }

        private async void ViewOnTokeNumber()
        {
            _timerController.Restart();
            await _peer.SendPacket(new RequestGetNumberPacket());
            //todo: response packet

        }

        public void ReceiveNumber(int number)
        {   
            _gameModel.Sum += number;
            _view.ReceiveNumber(number);
            Debug.Log($"Receive Number Sum = { _gameModel.Sum}");
            if(_gameModel.Sum > 21) _view.LockTurn();
            
        }

        public void ResetGame()
        {
            _timerController.Reset();
            _gameModel.Sum = 0;
        }
        public void RestartGame()
        {
            ResetGame();
            
            _view.RestartGame();
            _timerController.Start();
        }
    }
}