using System.Windows.Input;

namespace QuicKE.Client
{
    public interface IHistoryPageViewModel : IViewModel
    {
        ICommand HireCommand { get; }
        ICommand CallCommand { get; }
        ICommand CancelCommand { get; }
        ICommand RequestOtherCommand { get; }
        string Code
        {
            get;
        }
        string name { get; }
        string age { get; }
        string id_number { get; }
        string badge { get; }
        string phone { get; }
        int id { get; }
        int ticketID
        {
            get;
        }
        int Count { get; }

    }
}
