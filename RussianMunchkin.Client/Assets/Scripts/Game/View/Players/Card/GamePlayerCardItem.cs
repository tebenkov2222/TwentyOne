using UnityEngine;
using UnityEngine.UI;

namespace Game.View.Players.Card
{
    public class GamePlayerCardItem: MonoBehaviour
    {
        [SerializeField] private Text _cardText;
        
        public void SetNumber(int number)
        {
            ShowValue(number.ToString());
        }

        public void ShowValue(string value)
        {
            _cardText.text = value;
        }
    }
}