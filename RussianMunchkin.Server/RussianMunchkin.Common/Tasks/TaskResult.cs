namespace RussianMunchkin.Common.Tasks
{
    public class TaskResult
    {
        public bool IsSuccess{ get; set; }
        public string Log { get; set; }

        public TaskResult(bool isSuccess, string log = "")
        {
            IsSuccess = isSuccess;
            Log = log;
        }
        /*public static explicit operator bool(TaskResult result)
        {
            return result.IsSuccess;
        }
        public static explicit operator TaskResult(bool value)
        {
            return new TaskResult(value);
        }*/
        public static implicit operator bool(TaskResult result)
        {
            return result.IsSuccess;
        }
        public static implicit operator TaskResult(bool value)
        {
            return new TaskResult(value);
        }
    }
    
}