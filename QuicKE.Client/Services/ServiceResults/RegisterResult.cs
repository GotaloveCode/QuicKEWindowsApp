namespace QuicKE.Client
{
    public class RegisterResult : ErrorBucket
    {
        public string status { get; private set; }
        public string token { get; private set; }

        public RegisterResult(string token,string status)
        {
            this.token = token;
            this.status = status;
        }

        internal RegisterResult(ErrorBucket bucket)
            : base(bucket)
        {
        }
    }
   
}
