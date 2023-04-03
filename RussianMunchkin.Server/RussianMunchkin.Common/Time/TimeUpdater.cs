using System;

namespace RussianMunchkin.Common.Time
{
    public abstract class TimeUpdater: IDisposable
    {
        protected bool _isEnabled;
        protected bool _isAutoDispose;
        protected long _millis;

        public bool IsAutoDispose => _isAutoDispose;
        public bool IsEnabled => _isEnabled;
        public long Millis => _millis;
        public TimeSpan TimeSpan => TimeSpan.FromMilliseconds(_millis);
        public event Action Disposed;
        
        public void UpdateTime(long deltaTime)
        {
            if(!_isEnabled) return;
            _millis += deltaTime;
            UpdateTimeBody();
        }

        protected abstract void UpdateTimeBody();
        
        public void Start()
        {
            SetActive(true);
        }

        public void Stop()
        {
            SetActive(false);
            if(_isAutoDispose) Dispose();
        }

        public void Restart()
        {
            Reset();
            Start();
        }

        public void SetActive(bool value)
        {
            _isEnabled = value;
        }
        public void SetAutoDispose(bool value)
        {
            _isAutoDispose = value;
        }
        public void Reset()
        {
            _millis = 0;
        }

        public virtual void Dispose()
        {
            Disposed?.Invoke();
        }
    }
}