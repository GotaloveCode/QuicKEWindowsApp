using System.Collections.ObjectModel;
using System.Windows.Input;

namespace QuicKE.Client
{
    public interface IServicesPageViewModel : IViewModel
    {
        ICommand ContinueCommand { get; }
        ICommand ItemClickedCommand { get; }
       
        
        ObservableCollection<ServiceItem> Items
        {
            get;
        }

    }
}
