using System.Threading.Tasks;

namespace QuicKE.Client
{

    public interface IEvaluateServiceProxy : IServiceProxy
    {
        Task<EvaluateResult> EvaluateAsync(double rating, string review, string TicketID);
    }
}
