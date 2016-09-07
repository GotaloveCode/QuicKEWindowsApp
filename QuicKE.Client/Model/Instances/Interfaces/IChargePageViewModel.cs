using System.Windows.Input;

namespace QuicKE.Client
{
    public interface IChargePageViewModel : IViewModel
    {
        ICommand ProceedCommand{get;}

        string Cost {get;}

        string Code {get;}

        string SummaryText { get; }


    }
}
