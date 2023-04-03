using UnityEngine;
using UnityEngine.UI;

namespace MatchMaking.Rooms.View.Windows.Lobby
{
    public class LobbyPlayerItem: MonoBehaviour
    {
        [SerializeField] private Text _usernameText;
        [SerializeField] private Toggle _isAdminToggle;
        [SerializeField] private Toggle _isReadyToggle;

        public void Init(string username, bool isAdmin, bool isReady)
        {
            SetUsername(username);
            SetAdmin(isAdmin);
            ChangeStatusReady(isReady);
        }
        public void SetUsername(string username)
        {
            _usernameText.text = username;
        }

        public void SetAdmin(bool isAdmin)
        {
            _isAdminToggle.SetIsOnWithoutNotify(isAdmin);
        }

        public void ChangeStatusReady(bool isReady)
        {
            _isReadyToggle.SetIsOnWithoutNotify(isReady);
        }
    }
}