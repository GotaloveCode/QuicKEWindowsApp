namespace QuicKE.Client
{
    public class EvaluateResult : ErrorBucket
    {

        public string Status { get; private set; }
        public string Message { get; private set; }

        public EvaluateResult(string message,string status)
        {
            Message = message;
            Status = status;
        }

        internal EvaluateResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
}
