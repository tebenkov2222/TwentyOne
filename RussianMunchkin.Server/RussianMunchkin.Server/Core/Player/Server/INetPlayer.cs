using RussianMunchkin.Server.Server;

namespace RussianMunchkin.Server.Core.Player.Server
{
    public interface INetPlayer
    {
        public Peer Peer { get; }
    }
}