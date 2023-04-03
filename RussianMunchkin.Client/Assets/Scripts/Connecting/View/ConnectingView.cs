using System;
using UnityEngine;
using UnityEngine.UI;

namespace Connecting.View
{
    public class ConnectingView: MonoBehaviour, IConnectingView
    {
        [SerializeField] private GameObject _connectingWindow;
        [SerializeField] private GameObject _tryConnectWindow;
        [SerializeField] private Button _tryConnectButton;
        public event Action TryConnect;

        private void OnEnable()
        {
            _tryConnectButton.onClick.AddListener(TryConnectButtonOnClick);
        }
        private void OnDisable()
        {
            _tryConnectButton.onClick.RemoveListener(TryConnectButtonOnClick);
        }

        private void TryConnectButtonOnClick()
        {
            TryConnect?.Invoke();
        }

        public void ShowConnectingWindow()
        {
            _connectingWindow.SetActive(true);
            _tryConnectWindow.SetActive(false);
        }

        public void Connected()
        {
            _connectingWindow.SetActive(false);
            _tryConnectWindow.SetActive(false);
        }

        public void ErrorConnect()
        {
            _tryConnectWindow.SetActive(true);
            _connectingWindow.SetActive(false);
        }
    }
}