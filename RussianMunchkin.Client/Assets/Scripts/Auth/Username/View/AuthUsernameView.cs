using System;
using UnityEngine;
using View.Views.Window;

namespace Auth.Username.View
{
    public class AuthUsernameView: WindowBase, IAuthUsernameView
    {
        [SerializeField] private EnterUsernameWindow _enterUsernameWindow;
        
        public event Action<string> JoinToServer;

        private void OnEnable()
        {
            _enterUsernameWindow.UsernameEntered+=EnterUsernameWindowOnUsernameEntered;
        }

        private void OnDisable()
        {
            _enterUsernameWindow.UsernameEntered-=EnterUsernameWindowOnUsernameEntered;
        }

        private void EnterUsernameWindowOnUsernameEntered(string username)
        {
            JoinToServer?.Invoke(username);
        }
        public void SuccessJoin()
        {
            CloseWindow();
        }
    }
}