using RussianMunchkin.Common.Models;
using RussianMunchkin.Server.Game.TwentyOne.Player;

namespace RussianMunchkin.Server.Game.Player.Static
{
    public static class CommonModelCreatorStatic
    {
        public static PlayerInfoModel GetPlayerInfoByPlayer(PlayerControllerBase player)
        {
            return new PlayerInfoModel()
            {
                Username = player.AuthController.AuthModel.Username,
                PlayerId = player.PlayerModel.PlayerId,
                IsReady = player.RoomsController.RoomsModel.IsReadyStartGame
            };
        }
    }
}