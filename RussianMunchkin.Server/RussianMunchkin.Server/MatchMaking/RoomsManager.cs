using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Prometheus;
using RussianMunchkin.Common;
using RussianMunchkin.Common.Models;
using RussianMunchkin.Common.Packets;
using RussianMunchkin.Server.Core.Player.Interfaces;
using RussianMunchkin.Server.Core.Player.Models;
using RussianMunchkin.Server.Game.TwentyOne.Player;
using ServerFramework;

using Player = RussianMunchkin.Server.Game.TwentyOne.Player.PlayerControllerBase;

namespace RussianMunchkin.Server.MatchMaking
{
    public class RoomsManager
    {
        private Dictionary<string, Room> _rooms;
        private Dictionary<int, Room> _players;
        private Gauge _countRoomsGauge;

        public Dictionary<string, Room> Rooms => _rooms;

        public Dictionary<int, Room> Players => _players;

        public RoomsManager()
        {
            _rooms = new Dictionary<string, Room>();
            _players = new Dictionary<int, Room>();
            _countRoomsGauge = Prometheus.Metrics.CreateGauge("count_rooms", "");
        }

        public Room GetRoomByPlayer(int playerId)
        {
            return _players[playerId];
        }
        public bool TryGetRoomByPlayerId(int playerId, out Room room)
        {
            return _players.TryGetValue(playerId, out room);
        }
        public bool TryGetRoomByPlayer(PlayerModel model, out Room room)
        {
            return TryGetRoomByPlayerId(model.PlayerId, out room);
        }
        public Room CreateRoom(int id, bool isPrivate, int maxCountPlayers)
        {
            var uid = UidGenerator.GenerateUid(_rooms.Count+1);
            var password = UidGenerator.GeneratePassword();
            var roomModel = new RoomModel();
            roomModel.PlayerIdAdmin = id;
            roomModel.IsPrivate = isPrivate;
            roomModel.MaxCountPlayers = maxCountPlayers;
            roomModel.Uid = uid;
            roomModel.Password = password;
            var room = new Room(roomModel);
            room.GameOver+=RoomOnGameOver;
            _rooms.Add(uid,room);
            Console.WriteLine($"Room was be created uid = {uid} password = {password} idAdmin = {id} isPrivate = {isPrivate} maxCount = {maxCountPlayers}");
            Console.WriteLine($"Count rooms = {_rooms.Count}");
            _countRoomsGauge.Inc();
            return room;
        }

        private void RoomOnGameOver(Room room)
        {
            room.GameOver-=RoomOnGameOver;
            var roomPlayerKeys = room.Players.Keys.ToList();
            foreach (var player in roomPlayerKeys)
            {
                TryRemovePlayer(player);
            }
        }

        public void AddPlayerToRoom(PlayerControllerBase player, Room room)
        {
            Console.WriteLine($"Player {player.AuthModel.Username} added to room {room.Model.Uid}");
            _players.Add(player.PlayerModel.PlayerId,room);
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
                _players.Remove(player.PlayerModel.PlayerId);
            }

            _rooms.Remove(room.Model.Uid);
            _countRoomsGauge.Dec();
        }
        public bool TryRemovePlayer(int playerId)
        {
            if (!_players.TryGetValue(playerId, out var room)) return false;
            
            Console.WriteLine($"Try Remove player from room. Uid room = {room.Model.Uid}");
            room.RemovePlayer(playerId);
            _players.Remove(playerId);
            Console.WriteLine($"Count player in room {room.Model.Uid} = {room.Players.Count}");

            if (room.Players.Count == 0)
            {
                Console.WriteLine($"Count player == 0 and room was be remove");
                RemoveRoom(room);
            }
            Console.WriteLine($"Count rooms = {_rooms.Count}");
            return true;

        }
        public bool TryRemovePlayer(PlayerControllerBase player)
        {
            return TryRemovePlayer(player.PlayerModel.PlayerId);
        }
    }
}