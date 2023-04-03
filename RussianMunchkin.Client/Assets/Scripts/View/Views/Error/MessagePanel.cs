using System;
using UnityEngine;
using UnityEngine.UI;
using View.Views.Window;

namespace View.Views.Error
{
    public class MessagePanel: WindowBase
    {
        [SerializeField] private Text _logText;
        [SerializeField] private Image _backGroudImage;
        [Space]
        [SerializeField] private Button _submitButton;
        [Space] 
        [SerializeField] private Color _infoColor;
        [SerializeField] private Color _errorColor;
        

        private void OnEnable()
        {
            _submitButton.onClick.AddListener(SubmitButtonOnClick);
        }

        private void OnDisable()
        {
            _submitButton.onClick.RemoveListener(SubmitButtonOnClick);
        }

        private void SubmitButtonOnClick()
        {
            Hide();
        }
        public void Show(string log, Color color)
        {
            Show();
            var textOutput = log switch
            {
                "room not found" => "Комната не найдена",
                "not all ready to start" => "Не все готовы начать",
                "room is locked" => "Комната закрыта",
                "players in room is max" => "Комната заполнена",
                "wrong password" => "Не верный пароль",
                "close room" => "Комната была расформирована",
                _ => log
            };
            _backGroudImage.color = color;
            _logText.text = textOutput;
        }
        public void ShowInfo(string log)
        {
            Show(log, _infoColor);
        }
        public void ShowFailed(string log)
        {
            Show(log, _errorColor);
        }
    }
}