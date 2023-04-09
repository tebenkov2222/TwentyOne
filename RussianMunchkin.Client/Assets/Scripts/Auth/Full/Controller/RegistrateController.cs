using Auth.Full.View;
using AutoMapper;
using Core;
using RussianMunchkin.Common.Packets.Auth;
using UnityEngine;

namespace Auth.Full.Controller
{
    public class RegistrateController: ControllerBase
    {
        private readonly IRegistrateView _view;
        private readonly Mapper _mapper;

        public RegistrateController(Peer peer, Mapper mapper, IRegistrateView view) : base(peer)
        {
            _view = view;
            _mapper = mapper;
        }

        public override void Enable()
        {
            _view.Register+=ViewOnRegister;
            _view.CheckBusyLogin+=ViewOnCheckBusyLogin;
        }

        public override void Disable()
        {
            _view.Register-=ViewOnRegister;
            _view.CheckBusyLogin-=ViewOnCheckBusyLogin;
        }

        private async void ViewOnCheckBusyLogin(string login)
        {
            var res = await _peer.SendPacket(new CheckLoginPacket()
            {
                Login = login
            });
            _view.SetLoginBusy(res);
        }

        private async void ViewOnRegister(RegistrationViewModel registrationViewModel)
        {
            var res = await _peer.SendPacket(_mapper.Map<RegistrationPacket>(registrationViewModel));
            if (res)
            {
                _view.SuccessRegistrate();
            }
            else Debug.LogError("Registration failed");
        }
    }
}