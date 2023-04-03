using System;
using System.Threading.Tasks;
using Auth.Username.View;
using Controllers;
using Core;
using Models;
using RussianMunchkin.Common.Packets;
using RussianMunchkin.Common.Tasks;

namespace Auth.Username
{
    public class AuthUsernameController: ControllerBase
    {
        private IAuthUsernameView _view;
        private readonly PlayerModel _playerModel;

        public AuthUsernameController(NetPeer netPeer, IAuthUsernameView view, PlayerModel playerModel) : base(netPeer)
        {
            _view = view;
            _playerModel = playerModel;
        }

        public override void Enable()
        {
            _view.JoinToServer+=ViewOnJoinToServer;
        }

        public override void Disable()
        {
            _view.JoinToServer+=ViewOnJoinToServer;
        }

        private async void ViewOnJoinToServer(string username)
        {
            var res = await NetPeer.SendPacket(new JoinPacket() { Username = username });
            if (res)
            {
                _playerModel.Username = username;
                _playerModel.PlayerId = Int32.Parse(res.Log);
                _view.SuccessJoin();
            }
        }
    }
}