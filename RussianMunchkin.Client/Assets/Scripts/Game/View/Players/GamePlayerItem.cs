using System.Threading.Tasks;
using Game.View.Players.Card;
using RussianMunchkin.Common.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Game.View.Players
{
    public class GamePlayerItem: MonoBehaviour
    {
        [SerializeField] private Text _usernameText;
        [SerializeField] private Text _sumText;
        [SerializeField] private GameObject _readyShowMark;
        [Space]
        [SerializeField] private GamePlayerCardItem _cardPrefab;
        [SerializeField] private GamePlayerCardItemsGroupView _itemsGroupView;
        
        private int _sum = 0;

        public void Show(PlayerInfoModel playerInfoModel)
        {
            _usernameText.text = playerInfoModel.Login;
            _sumText.text = "-";
        }

        public GamePlayerCardItem AddCard()
        {
            return _itemsGroupView.Add(_cardPrefab);
        }

        public void TakenNumber()
        {
            var cardItem = AddCard();
            cardItem.ShowValue("");
        }
        public void TakenNumber(int number)
        {
            var cardItem = AddCard();
            cardItem.SetNumber(number);
            _sum += number;
            ShowSum(_sum);
        }

        private void RemoveAllCards()
        {
            _itemsGroupView.RemoveAll();
        }

        public void Reset()
        {
            RemoveAllCards();
            _sumText.text = "-";
            _sum = 0;
            _readyShowMark.SetActive(false);
        }

        public void ShowSum(int sum)
        {
            _sumText.text = sum.ToString();
        }

        public async Task ShowResult(GamePlayerInfoModel result)
        {
             for (int i = 0; i < _itemsGroupView.Count; i++)
             {
                 await Task.Delay(250);
                 _itemsGroupView[i].SetNumber(result.Numbers[i]);
                ShowSum(result.Sum);
            }
             await Task.Delay(1000);
        }

        public void ReadyShow()
        {
            _readyShowMark.SetActive(true);
        }
    }
}