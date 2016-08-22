using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuicKE.Client
{
    public interface ILocateUserPageViewModel: IViewModel
    {
        ICommand GetPositionCommand { get; }
        string LatLong { get; }
        ObservableCollection<string> Locations { get; }
    }
}
