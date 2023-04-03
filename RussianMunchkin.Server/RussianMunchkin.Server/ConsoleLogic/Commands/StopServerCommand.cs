namespace RussianMunchkin.Server.ConsoleLogic.Commands
{
    public class StopServerCommand: CommandDecorator
    {
        private readonly EntryPoint _entryPoint;

        public StopServerCommand(EntryPoint entryPoint, ICommand previewCommand) : base(previewCommand)
        {
            _entryPoint = entryPoint;
        }

        protected override bool CheckCommandBody(string input)
        {
            if (input != "stop") return false;
            System.Console.WriteLine("Server stopped");
            _entryPoint.Disable();
            return true;
        }
    }
}