
namespace QuicKE.Client
{
    public class TaskCompleteResult: ErrorBucket
    {
        public string Status { get; private set; }

        public TaskCompleteResult(string status)
        {

            this.Status = status;
        }

        internal TaskCompleteResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}
