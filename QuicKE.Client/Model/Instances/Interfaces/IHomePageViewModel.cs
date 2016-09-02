using System.Windows.Input;

namespace QuicKE.Client
{
    public interface IHomePageViewModel : IViewModel
    {
        //ICommand ProfileCommand
        //{
        //    get;
        //}

        ICommand LogoutCommand { get; }

        ICommand ViewDayServiceCommand
        {
            get;
        }
        ICommand ViewMonthlyServiceCommand
        {
            get;
        }
        ICommand ChangeLocationCommand { get; }
        ICommand TaskDoneCommand
        {
            get;
        }
        int TicketId
        {
            get;
        }
        bool HasPendingTask { get; }
    }
}
