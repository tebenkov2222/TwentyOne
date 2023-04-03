using System;
using Controllers;
using Core;
using Core.PacketHandlers;
using PacketHandlers.Core;
using RussianMunchkin.Common.Packets;
using ServerFramework;
using UnityEngine;

namespace PacketHandlers.Handlers
{
    public class RoomPacketsHandler: PacketHandlerDecorator
    {
        private readonly RoomsController _roomsController;

        public RoomPacketsHandler(IPacketsHandler previewHandler, NetPeer netPeer, RoomsController roomsController) : base(previewHandler, netPeer)
        {
            _roomsController = roomsController;
        }

        protected override bool TryHandle(Packet packet)
        {
            switch (packet)
            {
                case ConnectToRoomPacket connectToRoomPacket:
                    _roomsController.ConnectToRoom(connectToRoomPacket);

                    break;
                case ChangeConnectionToRoomPacket connectionToRoomPacket:
                    if (connectionToRoomPacket.IsConnected)
                    {
                        _roomsController.EnterPlayerToRoom(connectionToRoomPacket.PlayerInfoModel);
                    }
                    else
                    {
                        _roomsController.LeftPlayerFromRoom(connectionToRoomPacket.PlayerInfoModel);
                    }
                    
                    break;
                case ChangeAdminRoomPacket changeAdminRoomPacket:
                    _roomsController.ChangeAdmin(changeAdminRoomPacket.PlayerInfoModel);
                    
                    break;
                case ChangeStatusReadyPlayerPacket changeStatusReadyPlayerPacket:
                    _roomsController.ChangeStatusReady(changeStatusReadyPlayerPacket.PlayerId, changeStatusReadyPlayerPacket.IsReady);

                    break;
                case ChangeStatusStartGamePacket changeStatusStartGamePacket:
                    _roomsController.ChangeStatusStartGame(changeStatusStartGamePacket.IsReady);

                    break;
                case ExitFromRoomPacket:
                    _roomsController.LeaveRoom();
                    
                    break;
                case SendListPublicRooms sendListPublicRooms:
                    _roomsController.ShowListPublicRooms(sendListPublicRooms.Rooms);
                    
                    break;
                default: return false;
            }
            
            NetPeer.SendResponse(true);
            return true;
        }
    }
}