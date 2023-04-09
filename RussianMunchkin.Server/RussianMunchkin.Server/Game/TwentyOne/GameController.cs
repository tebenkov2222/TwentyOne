using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RussianMunchkin.Common.Models;
using RussianMunchkin.Server.Core.Player.Interfaces;
using RussianMunchkin.Server.Core.Player.Models;
using RussianMunchkin.Server.Game.Player.Interfaces;
using RussianMunchkin.Server.Game.TwentyOne.Player.Models;

namespace RussianMunchkin.Server.Game.TwentyOne
{
    public class GameController
    {
        private bool _isStartedGame;
        private Dictionary<string, IPlayerGameController> _players;
        private Random _random;

        public bool IsStartedGame => _isStartedGame;
        
        public GameController()
        {
            _players = new Dictionary<string, IPlayerGameController>();
            
            _random = new Random();
        }

        public void Init(List<IPlayerGameController> players)
        {
            RestartGame();
            foreach (var player in players)
            {
                Console.WriteLine($"Add player {player.PlayerModel.Login} in game");
                _players.Add(player.PlayerModel.Login, player);
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
            var playersRiting = _players.Values
                .OrderByDescending(player => player.GameModel.Sum)
                .Where(player => player.GameModel.Sum <= 21)
                .ToList();
            int maxValue = Int32.MaxValue;
            if (playersRiting.Count > 0)
            {
                var curMaxSum = playersRiting[0].GameModel.Sum;
                var count = playersRiting.Count(p => p.GameModel.Sum == curMaxSum);
                if (count == 1) maxValue = curMaxSum;
            }
            foreach (var result in _players.Values)
            {
                results.Add(new GamePlayerInfoModel()
                {
                    Login = result.PlayerModel.Login,
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
            
            foreach (var playerInRoom in GetOpponents(model.Login))
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
            var player = _players[model.Login];
            player.ReceiveNumber(number);
            player.GameModel.Numbers.Add(number);
            player.GameModel.Sum += number;
            foreach (var playerInRoom in GetOpponents(model.Login))
            {
                playerInRoom.PlayerReceivedNumber(model);
            }
        }

        private IEnumerable<IPlayerGameController> GetOpponents(string login)
        {
            return _players.Values.Where(playerInRoom => playerInRoom.PlayerModel.Login != login);
        }
    }
}