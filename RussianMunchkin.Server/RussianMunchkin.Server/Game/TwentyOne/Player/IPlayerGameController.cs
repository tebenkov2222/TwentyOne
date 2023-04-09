using System;
using System.Collections.Generic;
using RussianMunchkin.Common.Models;
using RussianMunchkin.Server.Core.Player.Interfaces;
using RussianMunchkin.Server.Core.Player.Models;
using RussianMunchkin.Server.Game.TwentyOne.Player.Models;

namespace RussianMunchkin.Server.Game.Player.Interfaces
{
    public interface IPlayerGameController
    {
        public PlayerModel PlayerModel { get; }
        public PlayerToGameModel GameModel { get; }

        public event Action<PlayerModel> NumberTaken;
        public event Action<PlayerModel> PlayerReadyShow;
        public event Action<PlayerModel> PlayerReadyToRestart;
        public void StartGame();
        public void CloseGame();
        public void TakeNumber();
        public void ReceiveNumber(int number);
        public void PlayerReceivedNumber(PlayerModel playerModel);
        public void PlayerReadyToShow();
        public void ShowResults(List<GamePlayerInfoModel> results);
        public void RestartGame();
        public void OpponentReady(PlayerModel playerModel);
        public void  ReadyToRestart();
    }
}