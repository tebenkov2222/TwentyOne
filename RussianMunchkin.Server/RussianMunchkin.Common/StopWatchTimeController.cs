using System;
using System.Diagnostics;
using RussianMunchkin.Common.Time;

namespace RussianMunchkin.Common
{
    public class StopWatchTimeController: TimeController, IDisposable
    {
        private Stopwatch _stopwatch;

        public StopWatchTimeController()
        {
            _stopwatch = new Stopwatch();
        }

        public void Enable()
        {
            _stopwatch.Start();
        }

        public void Update()
        {
            UpdateTime(_stopwatch.ElapsedMilliseconds);
            _stopwatch.Restart();
        }
        public void Disable()
        {
            _stopwatch.Stop();
        }

        public void Dispose()
        {
            
        }
    }
}