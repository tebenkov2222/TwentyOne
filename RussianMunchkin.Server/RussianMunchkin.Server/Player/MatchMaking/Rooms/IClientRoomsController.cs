using RussianMunchkin.Common.Models;
using RussianMunchkin.Server.Core.Player.Interfaces;
using RussianMunchkin.Server.Game.Player;
using RussianMunchkin.Server.Game.TwentyOne.Player;

namespace RussianMunchkin.Server.Player.MatchMaking.Rooms
{
    public interface IClientRoomsController: IClientRooms
    {
        public ClientRoomsModel RoomsModel { get; }
        
        public void EnterToRoom(RoomInfoModel roomInfoModel); 
        public void PlayerEnteredToRoom(PlayerControllerBase player);
        public void PlayerLeftFromRoom(PlayerControllerBase player); 
        public void ExitFromRoom();
        public void ChangeAdmin(PlayerControllerBase player);
        public void ChangeStatusReady(bool isReady);
        public void ChangeStatusReadyPlayer(int playerId, bool isReady);
        public void ChangeStatusStartGame(bool isReady);
    }
}