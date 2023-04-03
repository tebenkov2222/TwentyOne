using System.Collections.Generic;

namespace RussianMunchkin.Common.Time
{
    public class TimersManager
    {
        private readonly TimeController _timeController;
        private static List<TimeUpdater> _timers;
        

        public TimersManager(TimeController timeController)
        {
            _timeController = timeController;
            _timers = new List<TimeUpdater>();
            _timeController.TimeUpdated+=TimeUpdatedControllerOnTimeUpdated;
        }

        private void TimeUpdatedControllerOnTimeUpdated(long deltaTime)
        {
            foreach (var timer in _timers)
            {
                timer.UpdateTime(deltaTime);
            }
        }

        public static TimerController GenerateTimer(int intervalMillis, bool isAutoDispose = false, bool isAutoRestart = false)
        {
            var timerController = new TimerController(intervalMillis);
            timerController.SetAutoDispose(isAutoDispose);
            timerController.SetAutoRestart(isAutoRestart);
            _timers.Add(timerController);
            timerController.Disposed += () =>
            {
                _timers.Remove(timerController);
            };
            return timerController;
        }
        public static StopWatchController GenerateStopWatch()
        {
            var stopWatch = new StopWatchController();
            _timers.Add(stopWatch);
            stopWatch.Disposed += () =>
            {
                _timers.Remove(stopWatch);
            };
            return stopWatch;
        }
        
    }
}