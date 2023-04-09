namespace Auth.Full.View.Interfaces
{
    public delegate void AuthorizationHandler(LogInViewModel logInViewModel);

    public interface ILoginView
    {
        public event AuthorizationHandler LogIn;
        public void LoginIncorrect();
        public void PasswordIncorrect();
        public void SuccessLogin();
        public void AccessDenied();
    }
}