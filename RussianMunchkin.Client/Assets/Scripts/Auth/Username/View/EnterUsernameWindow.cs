using System;
using UnityEngine;
using UnityEngine.UI;
using View.Views.Window;

namespace Auth.Username.View
{
    public class EnterUsernameWindow: WindowBase
    {
        [SerializeField] private InputField _usernameInputField;
        [SerializeField] private Button _joinButton;
        [Space] 
        [SerializeField] private int _minLenghtUsername = 8;
        

        public event Action<string> UsernameEntered;

        private void OnEnable()
        {
            _joinButton.onClick.AddListener(JoinButtonOnClick);
            _usernameInputField.onValueChanged.AddListener(UsernameOnChanged);
        }

        private void UsernameOnChanged(string username)
        {
            _joinButton.interactable = username.Length >= _minLenghtUsername;
        }

        private void OnDisable()
        {
            _usernameInputField.onValueChanged.RemoveListener(UsernameOnChanged);
            _joinButton.onClick.RemoveListener(JoinButtonOnClick);
        }

        private void JoinButtonOnClick()
        {
            UsernameEntered?.Invoke(_usernameInputField.text);
        }
    }
}