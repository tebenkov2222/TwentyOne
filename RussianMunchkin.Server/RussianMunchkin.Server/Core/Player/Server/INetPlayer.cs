using RussianMunchkin.Server.Core.Player.Server.Models;
using RussianMunchkin.Server.Server;

namespace RussianMunchkin.Server.Core.Player.Server
{
    public interface INetPlayer
    {
        public NetPlayerModel NetModel { get; }
        public NetPeer NetPeer { get; }
    }
}