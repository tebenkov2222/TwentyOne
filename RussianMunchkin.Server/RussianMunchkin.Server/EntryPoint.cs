using System;
using System.Threading;
using Prometheus;
using RussianMunchkin.Common;
using RussianMunchkin.Common.Time;
using RussianMunchkin.Server.Metrics;
using RussianMunchkin.Server.Server;

namespace RussianMunchkin.Server
{
    public class EntryPoint
    {
        private MetricsController _metrics;
        private Thread _updateTask;
        private TimersManager _timersManager;
        private StopWatchTimeController _timeController;
        private bool _isEnabled;
        private ServerController _serverController;

        public EntryPoint()
        {
            _metrics = new MetricsController();
            _timeController = new StopWatchTimeController();
            _timersManager = new TimersManager(_timeController);
            _serverController = new ServerController();
            
            Init();
        }

        private void Init()
        {
            Enable();
            _updateTask = new Thread(UpdateTask);
            _updateTask.Start();
        }

        public void Enable()
        {
            _metrics.Enable();
            _isEnabled = true;
            _timeController.Enable();
            _serverController.Enable();
            
            Console.CancelKeyPress += ConsoleControllerOnProgramCanceled;
        }

        private void UpdateTask()
        {
            while (true)
            {
                Thread.Sleep(20);
                if(!_isEnabled) return;
                Update();
            }
        }

        private void Update()
        {
            _timeController.Update();
            _serverController.Update();
        }

        public void Disable()
        {
            Console.CancelKeyPress -= ConsoleControllerOnProgramCanceled;

            _metrics.Disable();
            _isEnabled = false;
            _timeController.Disable();
            _serverController.Disable();
        }

        private void ConsoleControllerOnProgramCanceled(object sender, ConsoleCancelEventArgs e)
        {
            Disable();
        }
    }
}