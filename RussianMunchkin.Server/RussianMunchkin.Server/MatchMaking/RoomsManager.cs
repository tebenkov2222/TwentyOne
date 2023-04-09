using System;
using System.Collections.Generic;
using System.Linq;
using Prometheus;
using RussianMunchkin.Common;
using RussianMunchkin.Server.Core.Player.Models;
using RussianMunchkin.Server.Game.TwentyOne.Player;
using RussianMunchkin.Server.Player;
using PlayerController = RussianMunchkin.Server.Core.Player.PlayerControllerBase;

namespace RussianMunchkin.Server.MatchMaking
{
    public class RoomsManager
    {
        private Dictionary<string, Room> _rooms;
        private Dictionary<string, Room> _playersToRoom;

        private Gauge _countRoomsGauge;

        public Dictionary<string, Room> Rooms => _rooms;

        public Dictionary<string, Room> PlayersToRoom => _playersToRoom;

        public RoomsManager()
        {
            _rooms = new Dictionary<string, Room>();
            _playersToRoom = new Dictionary<string, Room>();
            _countRoomsGauge = Prometheus.Metrics.CreateGauge("count_rooms", "");
        }
        public bool TryGetRoomByPlayerLogin(string playerLogin, out Room room)
        {
            return _playersToRoom.TryGetValue(playerLogin, out room);
        }
        public Room CreateRoom(PlayerController player, bool isPrivate, int maxCountPlayers)
        {
            var uid = UidGenerator.GenerateUid(_rooms.Count+1);
            var password = UidGenerator.GeneratePassword();
            var roomModel = new RoomModel()
            {
                AdminLogin = (string)player,
                IsPrivate = isPrivate,
                MaxCountPlayers = maxCountPlayers,
                Uid = uid,
                Password = password,
            };
            
            var room = new Room(roomModel);
            room.GameOver+=RoomOnGameOver;
            _rooms.Add(uid,room);
            Console.WriteLine($"Room was be created uid = {uid} password = {password} adminLogin = {player.AuthController.AuthModel.Login} isPrivate = {isPrivate} maxCount = {maxCountPlayers}");
            Console.WriteLine($"Count rooms = {_rooms.Count}");
            _countRoomsGauge.Inc();
            return room;
        }

        private void RoomOnGameOver(Room room)
        {
            room.GameOver-=RoomOnGameOver;
            var roomPlayerValues = room.Players.Values.ToList();
            foreach (var player in roomPlayerValues)
            {
                TryRemovePlayer(player);
            }
        }

        public void AddPlayerToRoom(PlayerController player, Room room)
        {
            Console.WriteLine($"Player {player.AuthController.AuthModel.Login} added to room {room.Model.Uid}");
            _playersToRoom.Add((string)player,room);
            room.AddPlayer(player);
        }

        public void RemoveRoom(string uid)
        {
            RemoveRoom(_rooms[uid]);
        }
        public void RemoveRoom(Room room)
        {
            Console.WriteLine($"RemoveRoom");
            foreach (var player in room.Players.Values)
            {
                player.RoomsController.ExitFromRoom();
                _playersToRoom.Remove((string)player);
            }

            _rooms.Remove(room.Model.Uid);
            _countRoomsGauge.Dec();
        }
        public bool TryRemovePlayer(string login)
        {
            if (!_playersToRoom.TryGetValue(login, out var room)) return false;
            
            Console.WriteLine($"Try Remove player from room. Uid room = {room.Model.Uid}");
            room.RemovePlayer(login);
            _playersToRoom.Remove(login);
            Console.WriteLine($"Count player in room {room.Model.Uid} = {room.Players.Count}");

            if (room.Players.Count == 0)
            {
                Console.WriteLine($"Count player == 0 and room was be remove");
                RemoveRoom(room);
            }
            Console.WriteLine($"Count rooms = {_rooms.Count}");
            return true;

        }
        public bool TryRemovePlayer(PlayerController player)
        {
            return TryRemovePlayer((string)player);
        }
    }
}