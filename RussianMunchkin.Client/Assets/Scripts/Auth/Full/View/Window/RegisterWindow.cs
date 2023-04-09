using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using View.Views.Window;

namespace Auth.Full.View.Window
{
    public class RegisterWindow: WindowBase, IRegistrateView
    {
        public event Action RegistrateSuccessed;
        public event Action OpenLogin;
        public event RegisterHandler Register;
        public event CheckBusyLoginHandler CheckBusyLogin;

        private const int _minLenghtLogin = 3;
        private const int _maxLenghtLogin = 16;
        
        private const int _minLenghtPassword = 8;
        private const int _maxLenghtPassword = 32;
        [SerializeField] private TMP_InputField _loginInputField;
        [SerializeField] private TMP_InputField _passwordInputField;
        [SerializeField] private Button _submitButton;
        [SerializeField] private Button _openLoginButton;

        [SerializeField] private Text _loginIsBusyLabel;
        

        private bool _isCorrectLogin;
        private bool _isCorrectPassword;
        private bool _isBusyLogin;
        
        private string _login;
        private string _password;

        private void OnEnable()
        {
            _openLoginButton.onClick.AddListener(OpenLoginButtonOnClick);
            
            _loginInputField.onEndEdit.AddListener(LoginInputFieldOnSubmit);
            _passwordInputField.onEndEdit.AddListener(PasswordInputFieldOnSubmit);
            _submitButton.onClick.AddListener(LoginButtonOnClick);
            ResetView();
        }

        private void OnDisable()
        {
            _openLoginButton.onClick.RemoveListener(OpenLoginButtonOnClick);

            _loginInputField.onEndEdit.RemoveListener(LoginInputFieldOnSubmit);
            _passwordInputField.onEndEdit.RemoveListener(PasswordInputFieldOnSubmit);
            _submitButton.onClick.RemoveListener(LoginButtonOnClick);
        }

        private void OpenLoginButtonOnClick()
        {
            Hide();
            OpenLogin?.Invoke();
        }

        private void ResetView()
        {
            _loginInputField.text = "";
            _loginInputField.caretPosition = 0;
            _passwordInputField.text = "";
            _passwordInputField.caretPosition = 0;
            SetActiveLoginIsBusyLabel(false);
            _isCorrectLogin = false;
            _isCorrectPassword = false;
            _isBusyLogin = true;
        }

        public void SetLoginBusy(bool isBusy)
        {
            SetActiveLoginIsBusyLabel(isBusy);
            _isBusyLogin = isBusy;
        }

        private void LoginInputFieldOnSubmit(string login)
        {
            SetActiveLoginIsBusyLabel(false);
            _login = login;

            _isCorrectLogin = login.Length >= _minLenghtLogin && login.Length < _maxLenghtLogin;
            if (_isCorrectLogin)
            {
                _isBusyLogin = true;
                CheckBusyLogin?.Invoke(login);
            }
            CheckInteractionButton();
        }

        private void PasswordInputFieldOnSubmit(string password)
        {
            SetActiveLoginIsBusyLabel(false);
            _password = password;

            _isCorrectPassword = password.Length >= _minLenghtPassword && password.Length < _maxLenghtPassword;
            CheckInteractionButton();
        }

        private void CheckInteractionButton()
        {
            _submitButton.interactable = _isCorrectLogin && !_isBusyLogin && _isCorrectPassword;
        }

        private void LoginButtonOnClick()
        {
            Register?.Invoke(new RegistrationViewModel()
            {
                Login = _login,
                Password = _password,
            });
        }

        private void SetActiveLoginIsBusyLabel(bool isBusy)
        {
            _loginIsBusyLabel.gameObject.SetActive(isBusy);
        }

        public void SuccessRegistrate()
        {
            RegistrateSuccessed?.Invoke();
        }
    }
}