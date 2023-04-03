using UnityEngine;
using View.Views.Window.Core;

namespace View.Views.Base
{
    public abstract class WindowViewActionBase: MonoBehaviour, IWindow
    {
        public abstract void Show();
        public abstract void Hide();
    }
}