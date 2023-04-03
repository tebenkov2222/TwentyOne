using System.Collections.Generic;
using RussianMunchkin.Server.Core.Player.Interfaces;

namespace RussianMunchkin.Server.Player
{
    public class PlayersController<TPlayer> where TPlayer: IPlayer
    {
        private Dictionary<int, TPlayer> _players;

        public Dictionary<int, TPlayer> Players => _players;
        private Queue<int> _freeIdPlayers;

        public PlayersController()
        {
            _players = new Dictionary<int, TPlayer>();
            _freeIdPlayers = new Queue<int>();
        }

        public void AddPlayer(TPlayer playerOld)
        {
            int playerId = _players.Count;
            if (_freeIdPlayers.TryDequeue(out var freeId))
            {
                playerId = freeId;
            }
            _players.Add(playerId, playerOld);
            playerOld.SetPlayerId(playerId);
        }

        public void RemovePlayer(TPlayer playerOld)
        {
            _players.Remove(playerOld.PlayerModel.PlayerId);
            _freeIdPlayers.Enqueue(playerOld.PlayerModel.PlayerId);
        }
    }
}