using System.Threading;
using System.Threading.Tasks;

namespace RussianMunchkin.Common.Tasks
{
    public class ResultSimpleTask: SimpleTask<TaskResult>
    {
        private int _timeOutMilliseconds;

        public ResultSimpleTask(int timeOutMilliseconds = 10000): base()
        {
            _timeOutMilliseconds = timeOutMilliseconds;
        }
        public override async Task<TaskResult> Wait()
        {
            return await WaitWithTimeOut();
        }

        private async Task<TaskResult> WaitWithTimeOut()
        {
            var innerTask = Task.Factory.StartNew(TimeOutTask);
            return await base.Wait();
        }

        private async void TimeOutTask()
        {
            await Task.Delay(_timeOutMilliseconds);
            Complete(new TaskResult(false, "time out"));
        }
    }
}