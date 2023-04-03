using System;
using UnityEngine;
using View.Views.Base;
using View.Views.Window.Core;

namespace View.Views.Window
{
    public abstract class WindowBase: MonoBehaviour, IWindow
    {
        [SerializeField] private WindowViewActionBase windowViewAction;
        public event Action WindowClosed;
        public virtual void Show()
        {
            windowViewAction.Show();
        }
        public virtual void Hide()
        {
            windowViewAction.Hide();
        }
        protected void CloseWindow()
        {
            Hide();
            WindowClosed?.Invoke();
        }
    }
}