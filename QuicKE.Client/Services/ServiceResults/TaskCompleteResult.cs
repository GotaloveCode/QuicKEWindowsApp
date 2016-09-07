
namespace QuicKE.Client
{
    public class TaskCompleteResult: ErrorBucket
    {
        public string Message { get; private set; }
        public string Status { get; private set; }

        public TaskCompleteResult(string status,string message)
        {
            Status = status;
            Message = message;
        }

        internal TaskCompleteResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}
