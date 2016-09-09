using System.Collections.ObjectModel;
using System.Windows.Input;

namespace QuicKE.Client
{
      public interface IPendingTicketsPageViewModel : IViewModel
    {
        //string Name { get; }

        //int TicketID { get; }
        TicketItem SelectedTicket { get; set; }

        ObservableCollection<TicketItem> Items { get; }

        ICommand SubmitCommand { get; }

    }
}
