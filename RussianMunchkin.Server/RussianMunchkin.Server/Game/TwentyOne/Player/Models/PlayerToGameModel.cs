using System.Collections.Generic;

namespace RussianMunchkin.Server.Game.TwentyOne.Player.Models
{
    public class PlayerToGameModel
    {
        public List<int> Numbers { get; set; }
        public int Sum { get; set; }
        public bool IsReadyShow { get; set; }
        public bool IsReadyRestart { get; set; }

        public PlayerToGameModel()
        {
            Numbers = new List<int>();
        }
    }
}