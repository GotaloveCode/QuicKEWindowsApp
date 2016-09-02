using System.Windows.Input;

namespace QuicKE.Client
{
    public interface IChargePageViewModel : IViewModel
    {
        ICommand ProceedCommand
        {
            get;
        }

        ICommand BackCommand { get; }

        string Cost
        {
            get;
        }
       
        
    }
}
