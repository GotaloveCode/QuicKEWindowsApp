

using System.Collections.ObjectModel;
using System.Windows.Input;

namespace QuicKE.Client
{
    public interface ILocationPageViewModel : IViewModel
    {
        ICommand SubmitCommand{ get;}
        ObservableCollection<string> Locations{ get;}
        string SelectedLocation { get;}
    }
}
