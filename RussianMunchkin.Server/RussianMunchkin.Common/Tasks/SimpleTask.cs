using System.Threading.Tasks;

namespace RussianMunchkin.Common.Tasks
{
    public class SimpleTask<T>
    {
        private TaskCompletionSource<T> _completionSource;
        public bool IsCompleted => _isComplete;
        private bool _isComplete = true;
        public SimpleTask()
        {
            _completionSource = new TaskCompletionSource<T>();
        }
        public virtual void Complete(T result)
        {
            CompleteSource(result);
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
}