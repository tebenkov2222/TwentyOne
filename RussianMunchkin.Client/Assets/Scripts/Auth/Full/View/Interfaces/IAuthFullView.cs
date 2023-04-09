namespace Auth.Full.View.Interfaces
{
    
    public interface IAuthFullView
    {
        public ILoginView LoginView { get; }
        public IRegistrateView RegistrateView { get; }
    }
}