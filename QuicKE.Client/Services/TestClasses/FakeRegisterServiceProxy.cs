using System.Threading.Tasks;

namespace QuicKE.Client.Tests
{
    public class FakeRegisterServiceProxy : IRegisterServiceProxy
    {
        public Task<RegisterResult> RegisterAsync(string fullname, string phone_number, string email, string password)
        {
            if (phone_number == "254712704404")
                return Task.FromResult<RegisterResult>(new RegisterResult("tokenrandom", "success"));
            else
            {
                var error = new RegisterResult(new ErrorBucket());
                error.AddError("Invalid username.");
                return Task.FromResult<RegisterResult>(error);
            }
        }
    }

}
