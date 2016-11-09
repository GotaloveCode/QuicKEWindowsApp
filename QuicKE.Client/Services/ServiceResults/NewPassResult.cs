namespace QuicKE.Client
{

    public class NewPassResult : ErrorBucket
    {
        public string Status { get; private set; }
        public string Message { get; private set; }

        public NewPassResult(string status, string message)
        {
            Status = status;
            Message = message;
        }

        internal NewPassResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}