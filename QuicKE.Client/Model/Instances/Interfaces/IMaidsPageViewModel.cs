using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace QuicKE.Client
{
    public interface IMaidsPageViewModel : IViewModel
    {
        ICommand ViewMaidCommand { get;}
        ObservableCollection<MaidItem> Maids { get; }
               
    }
}
