using Core;
using Core.PacketHandlers;
using MatchMaking.Rooms;
using PacketHandlers.Core;
using RussianMunchkin.Common.Packets;
using RussianMunchkin.Common.Packets.Game;
using UnityEngine;

namespace Game
{
    public class GamePacketsHandler: PacketHandlerDecorator
    {
        private readonly GameController _gameController;
        private readonly RoomModel _roomModel;

        public GamePacketsHandler(IPacketsHandler previewHandler, NetPeer netPeer, GameController gameController, RoomModel roomModel) : base(previewHandler, netPeer)
        {
            _gameController = gameController;
            _roomModel = roomModel;
        }

        protected override bool TryHandle(Packet packet)
        {
            switch (packet)
            {
                case ChangeStatusGamePacket  changeStatusStartGamePacket:
                    if(changeStatusStartGamePacket.IsStartGame)
                        _gameController.StartGame(_roomModel.Players);
                    else 
                        _gameController.StopGame();
                    break;
                case RestartSessionPacket:
                    _gameController.RestartGame();
                    break;
                case PlayerTokeNumberPacket playerTokeNumberPacket:
                    _gameController.View.PlayerTokedNumber(playerTokeNumberPacket.PlayerId);
                    break;
                case PlayerReceivingNumberPacket playerReceivingNumberPacket:
                    _gameController.ReceiveNumber(playerReceivingNumberPacket.Number);
                    break;
                case ShowResultsPacket showResultsPacket:
                    Debug.Log("Handle Show Results");
                    _gameController.View.ShowResults(showResultsPacket.PlayerInfoModels);
                    break;
                case PlayerReadyGamePacket playerReadyGamePacket:
                    _gameController.View.PlayerReadyToShow(playerReadyGamePacket.PlayerId);
                    break;
                default: return false;
            }
            NetPeer.SendResponse(true);
            return true;
        }
    }
}