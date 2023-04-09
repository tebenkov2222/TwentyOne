namespace Core
{
    public abstract class ControllerBase
    {
        protected Peer _peer;

        protected ControllerBase(Peer peer)
        {
            _peer = peer;
        }

        public abstract void Enable();
        public abstract void Disable();
    }
}