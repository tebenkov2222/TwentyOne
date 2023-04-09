using System.IO;
using System.Net.NetworkInformation;
using Auth.Full.Controller;
using Auth.Full.Model;
using Auth.Full.View.Interfaces;
using AutoMapper;
using Core;
using Models;
using UnityEngine;

namespace Auth.Full
{
    public class AuthFullController: ControllerBase
    {
        private LoginController _loginController;
        private RegistrateController _registrateController;
        private IAuthFullView _view;
        private readonly PlayerModel _playerModel;
        private AuthorizationUserModel _authorizationUserModel;

        public AuthFullController(Peer peer, IAuthFullView view, PlayerModel playerModel, Mapper mapper) : base(peer)
        {
            _loginController = new LoginController(peer, mapper, view.LoginView);
            _registrateController = new RegistrateController(peer, mapper, view.RegistrateView);
            _view = view;
            _playerModel = playerModel;
        }
        
        public override void Enable()   
        {

            _loginController.Enable();
            _registrateController.Enable();
            
            string path = Application.persistentDataPath + "/token.cfg";
            if (File.Exists(path))
            {
                var text = File.ReadAllText(path);
                Debug.Log($"Token = {text}");
            }
        }

        public override void Disable()
        {
            _loginController.Disable();
            _registrateController.Disable();
        }

        public void Authorize(AuthorizationUserModel authorizationUserModel)
        {
            _authorizationUserModel = authorizationUserModel;
            _playerModel.Login = authorizationUserModel.Login;
        }
    }
}