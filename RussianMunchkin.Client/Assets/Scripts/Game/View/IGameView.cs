using System;
using System.Collections.Generic;
using MatchMaking.Rooms;
using Models;
using RussianMunchkin.Common.Models;

namespace Game.View
{
    public interface IGameView
    {
        public event Action TakeNumber;
        public event Action ReadyShow;
        public event Action ReadyRestart;
        public void ControllerInit(PlayerModel playerModel ,RoomModel roomModel);
        public void StartGame();
        public void StopGame();
        public void UpdateTimeTurn(float value);
        public void EndTurn();
        public void LockTurn();
        public void ReceiveNumber(int number);
        public void ShowResults(List<GamePlayerInfoModel> results);
        public void PlayerTokedNumber(int playerId);
        public void PlayerReadyToShow(int playerId);
        public void RestartGame();
    }
}