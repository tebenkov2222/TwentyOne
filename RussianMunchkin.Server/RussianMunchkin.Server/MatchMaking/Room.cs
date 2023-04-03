using System;
using System.Collections.Generic;
using System.Linq;
using RussianMunchkin.Common.Models;
using RussianMunchkin.Server.Core.Player.Interfaces;
using RussianMunchkin.Server.Game.Player.Static;
using RussianMunchkin.Server.Game.TwentyOne;
using RussianMunchkin.Server.Game.TwentyOne.Player;
using RussianMunchkin.Server.Game.TwentyOne.Player.Server;
using RussianMunchkin.Server.Player.MatchMaking.Rooms;
using RussianMunchkin.Server.Server;

namespace RussianMunchkin.Server.MatchMaking
{
    public class Room
    {
        private static int _minCountPlayersStartGame = 2;
        private readonly RoomModel _model;
        private Dictionary<int, PlayerControllerBase> _players;
        private bool _isReadyStartGame;
        private GameController _gameController;

        public event Action<Room> GameOver;

        public bool IsReadyStartGame
        {
            get => _isReadyStartGame;
            set
            {
                if (_isReadyStartGame != value)
                {
                    Console.WriteLine($"Is Ready = {value}");
                    _players[_model.PlayerIdAdmin].RoomsController.ChangeStatusStartGame(value);

                }

                _isReadyStartGame = value;
            }
        }
        public RoomModel Model => _model;
        public Dictionary<int, PlayerControllerBase> Players => _players;

        public Room(RoomModel model)
        {
            _model = model;
            _players = new Dictionary<int, PlayerControllerBase>();
            _gameController = new GameController();

        }

        public void AddPlayer(PlayerControllerBase player)
        {
            foreach (var playerInRoom in _players.Values)
            {
                playerInRoom.RoomsController.PlayerEnteredToRoom(player);
            }
            _players.Add(player.PlayerModel.PlayerId, player);
            player.RoomsController.EnterToRoom(GetRoomInfoModel());
            
            foreach (var playerInRoom in _players.Values)
            {
                player.RoomsController.PlayerEnteredToRoom(playerInRoom);
            }

            Console.WriteLine($"Enter {player.PlayerModel.PlayerId} to room {_model.Uid}. CountPlayers = {_players.Count}");
            CheckReadyStartGame();

        }
        public void RemovePlayer(int playerId)
        {
            var player = _players[playerId];
            player.RoomsController.ExitFromRoom();
            _players.Remove(playerId);
            if (_gameController.IsStartedGame)
            {
                StopGame();
            }
            foreach (var playerInRoom in _players.Values)
            {
                playerInRoom.RoomsController.PlayerLeftFromRoom(player);
            }

            if (player.PlayerModel.PlayerId == _model.PlayerIdAdmin) ChangeAdmin();
            if (_players.Count > 0) CheckReadyStartGame();
        }

        private void CheckReadyStartGame()
        {
            Console.WriteLine($"CheckReadyStartGame = {_players.Count >= _minCountPlayersStartGame} && {_players.All(t =>t.Value.RoomsModel.IsReadyStartGame)}");

            IsReadyStartGame = _players.Count >= _minCountPlayersStartGame && _players.All(t =>t.Value.RoomsModel.IsReadyStartGame);
            
            foreach (var player in _players.Values)
            {
                Console.WriteLine($"Player {player.PlayerModel.PlayerId} is ready = {player.RoomsModel.IsReadyStartGame}");
            }
        }

        private void ChangeAdmin()
        {
            var playerFirst = _players.Values.FirstOrDefault(t =>t is ServerPlayerToController);
            if (playerFirst != default)
            {
                _model.PlayerIdAdmin = playerFirst.PlayerModel.PlayerId;
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

        public void ChangeReadyStatusPlayer(int playerId, bool isReady)
        {
            _players[playerId].RoomsController.ChangeStatusReady(isReady);
            foreach (var player in _players.Values)
            {
                player.RoomsController.ChangeStatusReadyPlayer(playerId, isReady); 
            }
            CheckReadyStartGame();
        }
        public RoomInfoModel GetRoomInfoModel()
        {
            return new RoomInfoModel()
            {
                Uid = _model.Uid,
                Password = _model.Password,
                AdminPlayer = GetPlayerInfoByPlayerId(_model.PlayerIdAdmin), //TODO костыль убери
                IsPrivate = _model.IsPrivate,
                MaxCountPlayers = _model.MaxCountPlayers,
                CurrentCountPlayers = _players.Count,
            };
        }
        public PlayerInfoModel GetPlayerInfoByPlayerId(int playerId)
        {
            return CommonModelCreatorStatic.GetPlayerInfoByPlayer(_players[playerId]);
        }
    }
}