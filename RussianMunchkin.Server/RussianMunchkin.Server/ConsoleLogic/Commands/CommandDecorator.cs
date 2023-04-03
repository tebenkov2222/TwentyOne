namespace RussianMunchkin.Server.ConsoleLogic.Commands
{
    public abstract class CommandDecorator:ICommand
    {
        private ICommand _previewCommand;

        protected CommandDecorator(ICommand previewCommand)
        {
            _previewCommand = previewCommand;
        }

        public void CheckInput(string input)
        {
            if(!CheckCommandBody(input)) _previewCommand.CheckInput(input);
        }

        protected abstract bool CheckCommandBody(string input);
    }
}