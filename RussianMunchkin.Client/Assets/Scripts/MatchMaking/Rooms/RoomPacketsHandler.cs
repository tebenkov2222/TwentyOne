using System;
using Controllers;
using Core;
using Core.PacketHandlers;
using PacketHandlers.Core;
using RussianMunchkin.Common.Packets;
using RussianMunchkin.Common.Packets.Room;
using ServerFramework;
using UnityEngine;

namespace PacketHandlers.Handlers
{
    public class RoomPacketsHandler: PacketHandlerDecorator
    {
        private readonly RoomsController _roomsController;

        public RoomPacketsHandler(IPacketsHandler previewHandler, Peer peer, RoomsController roomsController) : base(previewHandler, peer)
        {
            _roomsController = roomsController;
        }

        protected override bool TryHandle(Packet packet)
        {
            switch (packet)
            {
                case ConnectToRoomPacket connectToRoomPacket:
                    _roomsController.ConnectToRoom(connectToRoomPacket);
                    _peer.SendResponse(true);

                    break;
                case ChangeConnectionToRoomPacket connectionToRoomPacket:
                    Debug.Log($"Change connection {connectionToRoomPacket.PlayerInfoModel.Login}");
                    if (connectionToRoomPacket.IsConnected)
                    {
                        _roomsController.EnterPlayerToRoom(connectionToRoomPacket.PlayerInfoModel);
                    }
                    else
                    {
                        _roomsController.LeftPlayerFromRoom(connectionToRoomPacket.PlayerInfoModel);
                    }
                    _peer.SendResponse(true);

                    break;
                case ChangeAdminRoomPacket changeAdminRoomPacket:
                    _roomsController.ChangeAdmin(changeAdminRoomPacket.PlayerInfoModel);
                    _peer.SendResponse(true);

                    break;
                case ChangeStatusReadyPlayerPacket changeStatusReadyPlayerPacket:
                    _peer.SendResponse(true);
                    _roomsController.ChangeStatusReady(changeStatusReadyPlayerPacket.PlayerLogin, changeStatusReadyPlayerPacket.IsReady);

                    break;
                case ChangeStatusStartGamePacket changeStatusStartGamePacket:
                    _roomsController.ChangeStatusStartGame(changeStatusStartGamePacket.IsReady);
                    _peer.SendResponse(true);

                    break;
                case ExitFromRoomPacket:
                    _roomsController.LeaveRoom();
                    _peer.SendResponse(true);

                    break;
                case SendListPublicRooms sendListPublicRooms:
                    _roomsController.ShowListPublicRooms(sendListPublicRooms.Rooms);
                    _peer.SendResponse(true);

                    break;
                default: return false;
            }
            
            return true;
        }
    }
}