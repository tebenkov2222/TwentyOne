using System;
using System.Collections;
using System.Net.NetworkInformation;
using Auth.Full.View.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using View.Views.Window;

namespace Auth.Full.View.Window
{
    public class LogInWindow: WindowBase, ILoginView
    {
        public event Action LoginSuccessed;
        public event Action OpenRegistrate;
        public event AuthorizationHandler LogIn;
        
        private const int _minLenghtLogin = 3;
        private const int _maxLenghtLogin = 16;
        
        private const int _minLenghtPassword = 8;
        private const int _maxLenghtPassword = 32;
        [SerializeField] private TMP_InputField _loginInputField;
        [SerializeField] private TMP_InputField _passwordInputField;
        [SerializeField] private Button _submitButton;
        [SerializeField] private Button _openRegistrateButton;

        [SerializeField] private Text _accessDeniedLabel;
        [SerializeField] private Text _incorrectLoginLabel;
        [SerializeField] private Text _incorrectPasswordLabel;
        
        [SerializeField] private Text _macAdressText;

        private bool _isCorrectLogin;
        private bool _isCorrectPassword;
        
        private string _login;
        private string _password;

        static string GetMacAddress()
        {
            string macAddresses = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces, thereby ignoring any
                // loopback devices etc.
                if (nic.NetworkInterfaceType != NetworkInterfaceType.Ethernet) continue;
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddresses;
        }
        private void OnEnable()
        {
            _macAdressText.text = GetMacAddress();
            _openRegistrateButton.onClick.AddListener(OpenRegistrateButtonOnClick);
            _loginInputField.onEndEdit.AddListener(LoginInputFieldOnSubmit);
            _passwordInputField.onEndEdit.AddListener(PasswordInputFieldOnSubmit);
            _submitButton.onClick.AddListener(LoginButtonOnClick);
            ResetView();
        }

        private void OnDisable()
        {
            _openRegistrateButton.onClick.RemoveListener(OpenRegistrateButtonOnClick);
            _loginInputField.onEndEdit.RemoveListener(LoginInputFieldOnSubmit);
            _passwordInputField.onEndEdit.RemoveListener(PasswordInputFieldOnSubmit);
            _submitButton.onClick.RemoveListener(LoginButtonOnClick);
        }

        private void ResetView()
        {
            _loginInputField.text = "";
            _loginInputField.caretPosition = 0;
            _passwordInputField.text = "";
            _passwordInputField.caretPosition = 0;

            _isCorrectLogin = false;
            _isCorrectPassword = false;
            HideErrorLabels();
        }

        private void OpenRegistrateButtonOnClick()
        {
            Hide();
            OpenRegistrate?.Invoke();
        }

        private void LoginInputFieldOnSubmit(string login)
        {
            HideErrorLabels();
            _login = login;
            _isCorrectLogin = login.Length >= _minLenghtLogin && login.Length <= _maxLenghtLogin;
            CheckInteractionButton();
        }

        private void PasswordInputFieldOnSubmit(string password)
        {
            HideErrorLabels();
            _password = password;
            _isCorrectPassword = password.Length >= _minLenghtPassword && password.Length <= _maxLenghtPassword;

            CheckInteractionButton();
        }

        private void CheckInteractionButton()
        {
            _submitButton.interactable = _isCorrectLogin && _isCorrectPassword;
        }
        private void LoginButtonOnClick()
        {
            LogIn?.Invoke(new LogInViewModel()
            {
                Login = _login,
                Password = _password,
            });
        }
        public void LoginIncorrect()
        {
            _incorrectLoginLabel.gameObject.SetActive(true);
        }

        public void PasswordIncorrect()
        {
            _incorrectPasswordLabel.gameObject.SetActive(true);
        }

        private void HideErrorLabels()
        {
            _accessDeniedLabel.gameObject.SetActive(false);
            _incorrectLoginLabel.gameObject.SetActive(false);
            _incorrectPasswordLabel.gameObject.SetActive(false);
        }

        public void SuccessLogin()
        {
            LoginSuccessed?.Invoke(); 
        }

        public void AccessDenied()
        {
            _accessDeniedLabel.gameObject.SetActive(true);
        }
    }
}