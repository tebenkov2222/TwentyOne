using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RussianMunchkin.Common.Models;
using RussianMunchkin.Server.Core.Player.Interfaces;
using RussianMunchkin.Server.Core.Player.Models;
using RussianMunchkin.Server.Game.Player.Interfaces;

namespace RussianMunchkin.Server.Game.TwentyOne
{
    public class GameController
    {
        private bool _isStartedGame;
        private Dictionary<int, IPlayerGameController> _players;
        private Random _random;

        public bool IsStartedGame => _isStartedGame;
        
        public GameController()
        {
            _players = new Dictionary<int, IPlayerGameController>();
            
            _random = new Random();
        }

        public void Init(List<IPlayerGameController> players)
        {
            foreach (var player in players)
            {
                Console.WriteLine($"Add player {player.PlayerModel.PlayerId} in game");
                _players.Add(player.PlayerModel.PlayerId, player);
            }

        }

        public void Enable()
        {
            _isStartedGame = true;
            foreach (var player in _players.Values)
            {
                player.NumberTaken+=PlayerOnNumberToked;
                player.PlayerReadyShow+=PlayerOnPlayerReadyShow;
                player.PlayerReadyToRestart+=PlayerOnPlayerReadyToRestart;
            }
            InitGame();

        }

        public void Disable()
        {
            _isStartedGame = false;

            foreach (var player in _players.Values)
            {
                player.NumberTaken-=PlayerOnNumberToked;
                player.PlayerReadyShow-=PlayerOnPlayerReadyShow;
                player.PlayerReadyToRestart-=PlayerOnPlayerReadyToRestart;
            }
        }

        private void PlayerOnPlayerReadyToRestart(PlayerModel obj)
        {
            if (_players.Values.All(p => p.GameModel.IsReadyRestart)) RestartGame();
        }

        public void ShowResults()
        {
            List<GamePlayerInfoModel> results = new List<GamePlayerInfoModel>();
            var playersRaiting = _players
                .OrderByDescending(player => player.Value.GameModel.Sum)
                .Where(player => player.Value.GameModel.Sum <= 21)
                .Select(p => p.Value)
                .ToList();
            int maxValue = Int32.MaxValue;
            if (playersRaiting.Count > 0)
            {
                var curMaxSum = playersRaiting[0].GameModel.Sum;
                var count = playersRaiting.Count(p => p.GameModel.Sum == curMaxSum);
                if (count == 1) maxValue = curMaxSum;
            }
            foreach (var result in _players.Values)
            {
                results.Add(new GamePlayerInfoModel()
                {
                    Id = result.PlayerModel.PlayerId,
                    Numbers = result.GameModel.Numbers,
                    Sum = result.GameModel.Sum,
                    IsWinner = maxValue == result.GameModel.Sum
                });
            }

            foreach (var player in _players.Values)
            {
                player.ShowResults(results);
            }
        }

        public void RestartGame()
        {
            foreach (var playerInRoom in _players)
            {
                playerInRoom.Value.RestartGame();
                playerInRoom.Value.GameModel.Numbers.Clear();
                playerInRoom.Value.GameModel.Sum = 0;
            }

            InitGame();
        }

        private void InitGame()
        {
            foreach (var player in _players)
            {
                PlayerOnNumberToked(player.Value.PlayerModel);
            }
        }
        private async void PlayerOnPlayerReadyShow(PlayerModel model)
        {
            
            foreach (var playerInRoom in _players.Values.Where(playerInRoom => playerInRoom.PlayerModel.PlayerId != model.PlayerId))
            {
                playerInRoom.OpponentReady(model);
            }
            if (_players.Values.All(p => p.GameModel.IsReadyShow))
            {
                ShowResults();
            }
        }

        private void PlayerOnNumberToked(PlayerModel model)
        {
            var number = _random.Next(1,12);
            var player = _players[model.PlayerId];
            player.ReceiveNumber(number);
            player.GameModel.Numbers.Add(number);
            player.GameModel.Sum += number;
            foreach (var playerInRoom in _players.Values.Where(playerInRoom => playerInRoom.PlayerModel.PlayerId != model.PlayerId))
            {
                playerInRoom.PlayerReceivedNumber(model);
            }
        }
    }
}