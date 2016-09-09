using System.Threading.Tasks;

namespace QuicKE.Client
{
     public interface ILoadHistoryServiceProxy : IServiceProxy
    {
        Task<GetMaidResult> GetMaidAsync();
    }
}
