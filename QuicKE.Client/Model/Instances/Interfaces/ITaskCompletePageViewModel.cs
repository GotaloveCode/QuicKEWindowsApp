using System.Windows.Input;

namespace QuicKE.Client
{
    public interface ITaskCompletePageViewModel : IViewModel
    {
        ICommand SubmitCommand
        {
            get;
        }
        int TicketID 
        {
            get;
        }
        
    }
}
