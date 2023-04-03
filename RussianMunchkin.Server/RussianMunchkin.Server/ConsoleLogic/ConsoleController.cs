using System;
using System.Threading;
using RussianMunchkin.Server.ConsoleLogic.Commands;

namespace RussianMunchkin.Server.ConsoleLogic
{
    public class ConsoleController
    {
        public event Action ProgramCanceled;
        
        private ICommand _commands;
        private Thread _updateTask;
        private bool _isEnabled;

        public ConsoleController(ICommand commands)
        {
            _commands = commands;
        }

        public void Enable()
        {
            System.Console.CancelKeyPress+=(sender, args) => {            
                ProgramCanceled?.Invoke();
            };
            _isEnabled = true;
            _updateTask = new Thread(UpdateTask);
            _updateTask.Start();
        }

        private void UpdateTask()
        {
            while (_isEnabled)
            {
                Thread.Sleep(20);
                Update();
            }
        } 
        public void Update()
        {
            CheckInputConsole();
        }
        public void Disable()
        {
            _isEnabled = false;
        }

        private void CheckInputConsole()
        {
            if(Console.In.Peek() == -1) return;
            var input = System.Console.ReadLine();
            _commands.CheckInput(input);
        }

    }
}