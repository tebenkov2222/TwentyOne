using System;
using System.Linq;
using RussianMunchkin.Common.Packets;
using RussianMunchkin.Server.Core.PacketsHandler.Decorator;
using RussianMunchkin.Server.Game.TwentyOne.Player.Server;

namespace RussianMunchkin.Server.MatchMaking
{
    public class RoomPacketHandler: PacketHandlerDecorator<ServerPlayerToController>
    {
        private RoomsManager _roomsManager;
        public RoomPacketHandler(IPacketsHandler<ServerPlayerToController> previewHandler,RoomsManager roomsManager) : base(previewHandler)
        {
            _roomsManager = roomsManager;
        }

        protected override bool TryHandle(ServerPlayerToController player, Packet packet)
        {
            switch (packet)
            {
                case CreateRoomPacket createRoomPacket:

                    CreateRoom(createRoomPacket, player);
                    break;
                case RequestConnectToRoomPacket requestConnectToRoomPacket:
                    HandleRequestConnectToRoom(player, requestConnectToRoomPacket);
                    
                    break;
                case ChangeStatusReadyPlayerPacket changeStatusReadyPlayerPacket:
                    HandleChangeStatusReady(changeStatusReadyPlayerPacket, player);
                    
                    break;
                case ChangeStatusGamePacket:
                    HandleStartGame(player);
                    
                    break;
                case ExitFromRoomPacket exitFromRoomPacket:
                    if (_roomsManager.TryRemovePlayer(exitFromRoomPacket.PlayerLogin))
                    {
                        player.Peer.SendResponse(true);
                        Console.WriteLine($"Exit Player {exitFromRoomPacket.PlayerLogin} From Room is complete");
                        player.RoomsController.ExitFromRoom();
                    }
                    else player.Peer.SendResponse(false, "room not found");
                    break;
                case GetListPublicRooms:
                    HandleGetListPublishRoom(player);
                    break;
                default: return false;
            }

            return true;
        }

        private async void HandleStartGame(ServerPlayerToController player)
        {
            if (!_roomsManager.TryGetRoomByPlayerLogin((string)player, out var roomStartGame))
            {
                player.Peer.SendResponse(false, "room not found");
            }
            else if (roomStartGame.IsReadyStartGame)
            {
                player.Peer.SendResponse(true);
                roomStartGame.StartGame();
                roomStartGame.Model.IsLocked = true;
            }
            else player.Peer.SendResponse(false, "not all ready to start");
        }
        private async void HandleGetListPublishRoom(ServerPlayerToController player)
        {
            player.Peer.SendResponse(true, "");
            var rooms = _roomsManager.Rooms.Values
                .Where(room => !room.Model.IsPrivate && !room.Model.IsLocked && room.Players.Count < room.Model.MaxCountPlayers)
                .Select(room => room.GetRoomInfoModel()).ToList();

            Console.WriteLine($"Count publish rooms = {rooms.Count}");
            var res = await player.Peer.SendPacket(new SendListPublicRooms() { Rooms = rooms });
            //todo: handle renponse
        }

        private void HandleChangeStatusReady(ChangeStatusReadyPlayerPacket changeStatusReadyPlayerPacket, ServerPlayerToController player)
        {
            if (_roomsManager.TryGetRoomByPlayerLogin(changeStatusReadyPlayerPacket.PlayerLogin, out var room))
            {
                player.Peer.SendResponse(true);
                room.ChangeReadyStatusPlayer(changeStatusReadyPlayerPacket.PlayerLogin, changeStatusReadyPlayerPacket.IsReady);
            }
            else player.Peer.SendResponse(false, "room not found");
        }
        private void HandleRequestConnectToRoom(ServerPlayerToController player, RequestConnectToRoomPacket requestConnectToRoomPacket)
        {
            var uid = requestConnectToRoomPacket.Uid;
            var password = requestConnectToRoomPacket.Password;
            if (_roomsManager.Rooms.TryGetValue(uid, out var room))
            {
                if (room.Model.IsLocked)
                {
                    player.Peer.SendResponse(false, "room is locked");
                    return;
                }

                Console.WriteLine($"Check count players {room.Players.Count} > {room.Model.MaxCountPlayers}");
                if (room.Players.Count >= room.Model.MaxCountPlayers)
                {
                    player.Peer.SendResponse(false, "players in room is max");
                    return;
                }
                if (room.Model.Password.Equals(password))
                {
                    player.Peer.SendResponse(true);
                    _roomsManager.AddPlayerToRoom(player ,room);
                }
                else
                {
                    player.Peer.SendResponse(false, "wrong password");
                }
            }   
            else
            {
                player.Peer.SendResponse(false, "room not found");
            }
        }
        private void CreateRoom(CreateRoomPacket createRoomPacket, ServerPlayerToController player)
        {
            player.Peer.SendResponse(true);
            var room = _roomsManager.CreateRoom(player, createRoomPacket.IsPrivate, createRoomPacket.MaxCountPlayers);
            _roomsManager.AddPlayerToRoom(player, room);
        }
    }
}