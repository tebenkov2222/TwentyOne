/*using System.Threading.Tasks;

namespace RussianMunchkin.Common.Tasks
{
    public class SimpleTask<T>
    {
        private TaskCompletionSource<T> _completionSource;
        public bool IsSuccess;
        public bool IsCompleted => _isComplete;
        private bool _isComplete = true;
        public string Log { get; private set; }
        public SimpleTask()
        {
            _completionSource = new TaskCompletionSource<T>();
        }

        public virtual void Complete(T result, string log = "successful")
        {
            Log = log;
            IsSuccess = true;
            CompleteSource(result);
        }
        public virtual void Failure(string log = "failed")
        {
            Log = log;
            IsSuccess = false;
            CompleteSource(default);
        }

        private void CompleteSource(T result)
        {
            _isComplete = true;
            _completionSource.TrySetResult(result);
        }

        public virtual async Task<T> Wait()
        {
            _isComplete = false;
            return await _completionSource.Task;
        }
    }
}*/