using System.Collections.Generic;
using RussianMunchkin.Server.Core.Player.Interfaces;
using RussianMunchkin.Server.Core.Player.Server;
using RussianMunchkin.Server.Game.TwentyOne.Player.Server;
using ServerFramework;

namespace RussianMunchkin.Server.Server
{
    public class ServerModel<TPlayer> where TPlayer: INetPlayer
    {
        public Dictionary<int, TPlayer> Clients;
        

        public ServerModel()
        {
            Clients = new Dictionary<int, TPlayer>();
        }
    }
}