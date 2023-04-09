using System;
using System.Collections.Generic;
using System.Linq;
using RussianMunchkin.Common.Models;
using RussianMunchkin.Server.Game.TwentyOne;
using RussianMunchkin.Server.Game.TwentyOne.Player.Server;
using RussianMunchkin.Server.Mapper;

using PlayerController = RussianMunchkin.Server.Core.Player.PlayerControllerBase;
namespace RussianMunchkin.Server.MatchMaking
{
    public class Room
    {
        private static int _minCountPlayersStartGame = 2;
        private readonly RoomModel _model;
        private Dictionary<string, PlayerController> _players;
        public PlayerController this[string login] => Players[login];
        public PlayerController this[PlayerController player] => this[player.AuthController.AuthModel.Login];
        private bool _isReadyStartGame;
        private GameController _gameController;

        public Dictionary<string, PlayerController> Players => _players;
        public event Action<Room> GameOver;

        public bool IsReadyStartGame
        {
            get => _isReadyStartGame;
            set
            {
                if (_isReadyStartGame != value)
                {
                    Console.WriteLine($"Is Ready = {value}");
                    _players[_model.AdminLogin].RoomsController.ChangeStatusStartGame(value);

                }

                _isReadyStartGame = value;
            }
        }
        public RoomModel Model => _model;

        public Room(RoomModel model)
        {
            _model = model;
            _players = new Dictionary<string, PlayerController>();
            _gameController = new GameController();

        }

        public void AddPlayer(PlayerController player)
        {
            foreach (var playerInRoom in _players.Values)
            {
                playerInRoom.RoomsController.PlayerEnteredToRoom(player);
            }
            _players.Add(player.AuthController.AuthModel.Login,player);
            player.RoomsController.EnterToRoom(GetRoomInfoModel());
            
            foreach (var playerInRoom in _players.Values)
            {
                player.RoomsController.PlayerEnteredToRoom(playerInRoom);
            }

            Console.WriteLine();
            Console.WriteLine($"Enter {player.PlayerModel.Login} to room {_model.Uid}. CountPlayers = {_players.Count}");
            CheckReadyStartGame();

        }
        public void RemovePlayer(string login)
        {
            var player = _players[login];
            player.RoomsController.ExitFromRoom();
            _players.Remove(login);
            if (_gameController.IsStartedGame)
            {
                StopGame();
            }
            foreach (var playerInRoom in _players.Values)
            {
                playerInRoom.RoomsController.PlayerLeftFromRoom(player);
            }

            if (player.AuthController.AuthModel.Login == _model.AdminLogin) ChangeAdmin();
            if (_players.Count > 0) CheckReadyStartGame();
        }

        private void CheckReadyStartGame()
        {
            IsReadyStartGame = _players.Count >= _minCountPlayersStartGame && _players.All(t =>t.Value.RoomsController.PlayerRoomModel.IsReadyStartGame);
        }

        private void ChangeAdmin()
        {
            var playerFirst = _players.Values.FirstOrDefault(t =>t is ServerPlayerToController);
            if (playerFirst != default)
            {
                _model.AdminLogin = playerFirst.AuthController.AuthModel.Login;
                foreach (var playerInRoom in _players.Values)
                {
                    playerInRoom.RoomsController.ChangeAdmin(playerFirst);
                }
            }
            else
            {
                foreach (var playerInList in _players.Values)
                {
                    playerInList.RoomsController.ExitFromRoom();
                }
                _players.Clear();
            }
        }

        public void StartGame()
        {
            Console.WriteLine($"START GAME IN ROOM {Model.Uid}");
            _gameController.Init(_players.Values.Select(p => p.GameController).ToList());

            foreach (var player in _players.Values)
            {
                player.GameController.StartGame();
            }
            _gameController.Enable();
        }

        public void StopGame()
        {
            foreach (var player in _players.Values)
            {
                player.GameController.CloseGame();
            }
            _gameController.Disable();
            GameOver?.Invoke(this);
        }

        public void ChangeReadyStatusPlayer(string playerLogin, bool isReady)
        {
            this[playerLogin].RoomsController.ChangeStatusReady(isReady);
            foreach (var playerInRoom in _players.Values)
            {
                playerInRoom.RoomsController.ChangeStatusReadyPlayer(playerLogin, isReady); 
            }
            CheckReadyStartGame();
        }
        public RoomInfoModel GetRoomInfoModel()
        {
            return new RoomInfoModel()
            {
                Uid = _model.Uid,
                Password = _model.Password,
                AdminPlayer = GetPlayerInfoByPlayerLogin(_model.AdminLogin),
                IsPrivate = _model.IsPrivate,
                MaxCountPlayers = _model.MaxCountPlayers,
                CurrentCountPlayers = _players.Count,
            };
        }
        public PlayerInfoModel GetPlayerInfoByPlayerLogin(string playerLogin)
        {
            return MapperInstance.Mapper.Map<PlayerInfoModel>(_players[playerLogin]);
        }
    }
}