using System;
using Auth.Full.View.Interfaces;
using Auth.Full.View.Window;
using UnityEngine;
using View.Views.Window;

namespace Auth.Full.View
{
    public class AuthFullView: WindowBase, IAuthFullView
    {
        [SerializeField] private LogInWindow _logInWindow;
        [SerializeField] private RegisterWindow _registerWindow;
        
        public event Action LoginSuccessed;

        public ILoginView LoginView => _logInWindow;
        public IRegistrateView RegistrateView => _registerWindow;

        private void OnEnable()
        {
            Show();
            _logInWindow.OpenRegistrate+=LogInWindowOnOpenRegistrate;
            _logInWindow.LoginSuccessed+=LogInWindowOnLoginSuccessed;
            
            _registerWindow.OpenLogin+=RegisterWindowOnOpenLogin;
            _registerWindow.RegistrateSuccessed+=RegisterWindowOnRegistrateSuccessed;
        }
        private void OnDisable()
        {
            _logInWindow.OpenRegistrate-=LogInWindowOnOpenRegistrate;
            _logInWindow.LoginSuccessed-=LogInWindowOnLoginSuccessed;
            
            _registerWindow.OpenLogin-=RegisterWindowOnOpenLogin;
            _registerWindow.RegistrateSuccessed-=RegisterWindowOnRegistrateSuccessed;
        }

        private void RegisterWindowOnRegistrateSuccessed()
        {
            LoginSuccessed?.Invoke();
        }

        private void LogInWindowOnLoginSuccessed()
        {
            LoginSuccessed?.Invoke();
        }

        private void RegisterWindowOnOpenLogin()
        {
            _logInWindow.Show();
        }
        
        private void LogInWindowOnOpenRegistrate()
        {
            _registerWindow.Show();
        }

        public override void Show()
        {
            _logInWindow.Show();
            _registerWindow.Hide();
        }

        public override void Hide()
        {
            _logInWindow.Hide();
            _registerWindow.Hide();
        }
    }
}