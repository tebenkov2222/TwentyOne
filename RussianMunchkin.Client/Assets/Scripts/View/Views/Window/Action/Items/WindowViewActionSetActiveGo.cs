using UnityEngine;
using View.Views.Base;

namespace View.Views.Window.View.Items
{
    public class WindowViewActionSetActiveGo: WindowViewActionBase
    {
        [SerializeField] private bool _isReverse;
        
        public override void Show()
        {
            gameObject.SetActive(!_isReverse);
        }

        public override void Hide()
        {
            gameObject.SetActive(_isReverse);
        }
    }
}