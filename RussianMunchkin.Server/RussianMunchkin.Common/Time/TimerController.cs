namespace RussianMunchkin.Common.Time
{
    public delegate void TimerElapsedHandler();
    public class TimerController: TimeUpdater
    {
        private bool _isAutoRestart;

        public bool IsAutoRestart => _isAutoRestart;
        public event TimerElapsedHandler Elapsed;
        private readonly int _interval;

        public int Interval => _interval;

        public TimerController(int interval)
        {
            _interval = interval;
        }

        public void SetAutoRestart(bool value)
        {
            _isAutoRestart = value;
        }
        protected override void UpdateTimeBody()
        {
            if (_millis <= _interval) return;
            if (_isAutoRestart)
            {
                Reset();
                Elapsed?.Invoke();
            }
            else
            {
                Stop();
                Reset();
                Elapsed?.Invoke();
            }
        }
    }
}