using System;
using System.Threading;
using Prometheus;
using RussianMunchkin.Common;
using RussianMunchkin.Common.Time;
using RussianMunchkin.Server.ConsoleLogic;
using RussianMunchkin.Server.ConsoleLogic.Commands;
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
        private ConsoleController _consoleController;
        private bool _isEnabled;
        private ServerController _serverController;

        public EntryPoint()
        {
            _metrics = new MetricsController();
            _timeController = new StopWatchTimeController();
            _timersManager = new TimersManager(_timeController);
            _serverController = new ServerController();
            
            InitConsole();
            Init();
        }

        private void InitConsole()
        {
            var endCommand = new EndCommand();
            var stopServerCommand = new StopServerCommand(this, endCommand);
            _consoleController = new ConsoleController(stopServerCommand);
            _consoleController.ProgramCanceled+=ConsoleControllerOnProgramCanceled;
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
            _consoleController.Enable();
            _serverController.Enable();
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
            _metrics.Disable();
            _isEnabled = false;
            _timeController.Disable();
            _consoleController.Disable();
            _consoleController.ProgramCanceled-=ConsoleControllerOnProgramCanceled;
            _serverController.Disable();
        }

        private void ConsoleControllerOnProgramCanceled()
        {
            Disable();
        }
    }
}