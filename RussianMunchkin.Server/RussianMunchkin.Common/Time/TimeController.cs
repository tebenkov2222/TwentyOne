using System;

namespace RussianMunchkin.Common.Time
{
    public delegate void TimeUpdatedHandler(long deltaTime);
    public abstract class TimeController
    {
        private long _millis;

        public long Millis => _millis;
        public TimeSpan TimeSpan => System.TimeSpan.FromMilliseconds(_millis);

        public event TimeUpdatedHandler TimeUpdated;

        protected void UpdateTime(long deltaTime)
        {
            _millis += deltaTime;
            TimeUpdated?.Invoke(deltaTime);
        }
    }
}