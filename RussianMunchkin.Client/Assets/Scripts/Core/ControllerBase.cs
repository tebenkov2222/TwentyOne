namespace Core
{
    public abstract class ControllerBase
    {
        protected NetPeer NetPeer;

        protected ControllerBase(NetPeer netPeer)
        {
            NetPeer = netPeer;
        }

        public abstract void Enable();
        public abstract void Disable();
    }
}