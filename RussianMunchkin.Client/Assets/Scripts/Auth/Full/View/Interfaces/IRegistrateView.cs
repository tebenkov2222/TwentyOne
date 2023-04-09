namespace Auth.Full.View
{
    public delegate void RegisterHandler(RegistrationViewModel registrationViewModel);
    public delegate void CheckBusyLoginHandler(string login);
    public interface IRegistrateView
    {
        public event RegisterHandler Register;
        public event CheckBusyLoginHandler CheckBusyLogin;
        public void SetLoginBusy(bool isBusy);

        public void SuccessRegistrate();
    }
}