using System;
using System.Collections.Generic;
using RussianMunchkin.Common.Models;
using RussianMunchkin.Common.Packets;
using RussianMunchkin.Common.Packets.Game;
using RussianMunchkin.Server.Core.Player.Models;
using RussianMunchkin.Server.Game.Player;
using RussianMunchkin.Server.Game.Player.Interfaces;
using RussianMunchkin.Server.Game.TwentyOne.Player.Models;
using RussianMunchkin.Server.Server;

namespace RussianMunchkin.Server.Game.TwentyOne.Player.Server.Controllers
{
    public class ServerPlayerGameToController: IPlayerGameController
    {
        private readonly NetPeer _netPeer;
        private readonly PlayerModel _playerModel;
        private readonly PlayerToGameModel _gameModel;

        public PlayerModel PlayerModel => _playerModel;
        public PlayerToGameModel GameModel => _gameModel;

        public ServerPlayerGameToController(NetPeer netPeer, PlayerModel playerModel, PlayerToGameModel gameModel)
        {
            _netPeer = netPeer;
            _playerModel = playerModel;
            _gameModel = gameModel;
        }

        public event Action<PlayerModel> NumberTaken;
        public event Action<PlayerModel> PlayerReadyShow;
        public event Action<PlayerModel> PlayerReadyToRestart;

        public async void StartGame()
        {
            var res = await _netPeer.SendPacket(new ChangeStatusGamePacket(){IsStartGame = true});
            //todo: implement handle result
        }

        public async void CloseGame()
        {
            var res = await _netPeer.SendPacket(new ChangeStatusGamePacket(){IsStartGame = false});
            //todo: implement handle result
        }

        public void TakeNumber()
        {
            NumberTaken?.Invoke(_playerModel);
        }

        public void PlayerReadyToShow()
        {
            _gameModel.IsReadyShow = true;
            PlayerReadyShow?.Invoke(_playerModel);
        }

        public async void PlayerReceivedNumber(PlayerModel playerModel)
        {
            var res = await _netPeer.SendPacket(new PlayerTokeNumberPacket(){PlayerId = playerModel.PlayerId});
            //todo: implement handle result
        }

        public async void ReceiveNumber(int number)
        {
            var res = await _netPeer.SendPacket(new PlayerReceivingNumberPacket(){Number = number});
            //todo: implement handle result
        }

        public async void ShowResults(List<GamePlayerInfoModel> results)
        {
            var res = await _netPeer.SendPacket(new ShowResultsPacket(){PlayerInfoModels = results});
            //todo: implement handle result
        }

        public async void RestartGame()
        {
            _gameModel.IsReadyShow = false;
            _gameModel.IsReadyRestart = false;

            var res = await _netPeer.SendPacket(new RestartSessionPacket());
            //todo: implement handle result
        }

        public async void OpponentReady(PlayerModel playerModel)
        {
            var res = await _netPeer.SendPacket(new PlayerReadyGamePacket(){PlayerId = playerModel.PlayerId});
            //todo: implement handle result
        }

        public void ReadyToRestart()
        {
            _gameModel.IsReadyRestart = true;
            PlayerReadyToRestart?.Invoke(_playerModel);
        }
    }
}