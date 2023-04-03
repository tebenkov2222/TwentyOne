using RussianMunchkin.Common.Packets;

namespace Core.PacketHandlers
{
    public interface IPacketsHandler
    {
        public void Handle(Packet packet);
    }
}