using System;
using Auth.Full.View;
using Auth.Full.View.Interfaces;
using AutoMapper;
using Core;
using RussianMunchkin.Common.Packets.Auth;

namespace Auth.Full.Controller
{
    public class LoginController: ControllerBase
    {
        private readonly ILoginView _view;
        private readonly Mapper _mapper;

        public LoginController(Peer peer, Mapper mapper, ILoginView view) : base(peer)
        {
            _view = view;
            _mapper = mapper;
        }

        public override void Enable()
        {
            _view.LogIn+=ViewOnLogIn;

        }

        public override void Disable()
        {
            _view.LogIn-=ViewOnLogIn;
        }

        private async void ViewOnLogIn(LogInViewModel loginViewModel)
        {
            var res = await _peer.SendPacket(_mapper.Map<AuthorizationPacket>(loginViewModel));
            if (res)
            {
                _view.SuccessLogin();
            }
            else
            {
                switch (res.Log)
                {
                    case "incorrect login":
                        _view.LoginIncorrect();
                        break;
                    case "incorrect password":
                        _view.PasswordIncorrect();
                        break;
                    case "access denied":
                        _view.AccessDenied();
                        break;
                }
            }
        }
    }
}