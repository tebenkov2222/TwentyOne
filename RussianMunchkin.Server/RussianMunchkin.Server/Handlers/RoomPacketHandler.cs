using System;
using System.Linq;
using RussianMunchkin.Common.Packets;
using RussianMunchkin.Server.Core.PacketsHandler.Decorator;
using RussianMunchkin.Server.Game.TwentyOne.Player.Server;
using RussianMunchkin.Server.MatchMaking;

namespace RussianMunchkin.Server.Handlers
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
                    if (_roomsManager.TryRemovePlayer(exitFromRoomPacket.PlayerId))
                    {
                        player.NetPeer.SendResponse(true);
                        Console.WriteLine($"Exit Player {exitFromRoomPacket.PlayerId} From Room is complete");
                        player.RoomsController.ExitFromRoom();
                    }
                    else player.NetPeer.SendResponse(false, "room not found");
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
            if (!_roomsManager.TryGetRoomByPlayer(player.PlayerModel, out var roomStartGame))
            {
                player.NetPeer.SendResponse(false, "room not found");
            }
            else if (roomStartGame.IsReadyStartGame)
            {
                player.NetPeer.SendResponse(true);
                roomStartGame.StartGame();
                roomStartGame.Model.IsLocked = true;
            }
            else player.NetPeer.SendResponse(false, "not all ready to start");
        }
        private async void HandleGetListPublishRoom(ServerPlayerToController player)
        {
            player.NetPeer.SendResponse(true, "");
            var rooms = _roomsManager.Rooms.Values
                .Where(room => !room.Model.IsPrivate && !room.Model.IsLocked && room.Players.Count < room.Model.MaxCountPlayers)
                .Select(room => room.GetRoomInfoModel()).ToList();

            Console.WriteLine($"Count publish rooms = {rooms.Count}");
            var res = await player.NetPeer.SendPacket(new SendListPublicRooms() { Rooms = rooms });
            //todo: handle renponse
        }

        private void HandleChangeStatusReady(ChangeStatusReadyPlayerPacket changeStatusReadyPlayerPacket, ServerPlayerToController player)
        {
            if (_roomsManager.TryGetRoomByPlayerId(changeStatusReadyPlayerPacket.PlayerId, out var room))
            {
                room.ChangeReadyStatusPlayer(changeStatusReadyPlayerPacket.PlayerId, changeStatusReadyPlayerPacket.IsReady);
                player.NetPeer.SendResponse(true);
            }
            else player.NetPeer.SendResponse(false, "room not found");
        }
        private void HandleRequestConnectToRoom(ServerPlayerToController player, RequestConnectToRoomPacket requestConnectToRoomPacket)
        {
            var uid = requestConnectToRoomPacket.Uid;
            var password = requestConnectToRoomPacket.Password;
            if (_roomsManager.Rooms.TryGetValue(uid, out var room))
            {
                if (room.Model.IsLocked)
                {
                    player.NetPeer.SendResponse(false, "room is locked");
                    return;
                }

                Console.WriteLine($"Check count players {room.Players.Count} > {room.Model.MaxCountPlayers}");
                if (room.Players.Count >= room.Model.MaxCountPlayers)
                {
                    player.NetPeer.SendResponse(false, "players in room is max");
                    return;
                }
                if (room.Model.Password.Equals(password))
                {
                    player.NetPeer.SendResponse(true);
                    _roomsManager.AddPlayerToRoom(player ,room);
                }
                else
                {
                    player.NetPeer.SendResponse(false, "wrong password");
                }
            }   
            else
            {
                player.NetPeer.SendResponse(false, "room not found");
            }
        }
        private void CreateRoom(CreateRoomPacket createRoomPacket, ServerPlayerToController player)
        {
            player.NetPeer.SendResponse(true);
            var room = _roomsManager.CreateRoom(player.PlayerModel.PlayerId, createRoomPacket.IsPrivate, createRoomPacket.MaxCountPlayers);
            _roomsManager.AddPlayerToRoom(player, room);
        }
    }
}