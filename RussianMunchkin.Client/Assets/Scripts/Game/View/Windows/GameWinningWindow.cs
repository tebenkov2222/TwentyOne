using System.Collections.Generic;
using System.Linq;
using RussianMunchkin.Common.Models;
using UnityEngine;
using UnityEngine.UI;
using View.Views.Window;

namespace Game.View.Windows
{
    public class GameWinningWindow: WindowBase
    {
        [SerializeField] private Text _winnerText;
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _drawPanel;
        
        private Dictionary<string, PlayerInfoModel> _players;

        public void Init(List<PlayerInfoModel> players)
        {
            _players = new Dictionary<string, PlayerInfoModel>();
            foreach (var player in players)
            {
                _players.Add(player.Login, player);
            }
        }
        public void ShowResults(List<GamePlayerInfoModel> results)
        {
            var winner = results.FirstOrDefault(r => r.IsWinner);
            var isWinner = winner != default;
            _winPanel.SetActive(isWinner);
            _drawPanel.SetActive(!isWinner);
            if (isWinner)
            {
                _winnerText.text = _players[winner.Login].Login;
            }
        }
    }
}