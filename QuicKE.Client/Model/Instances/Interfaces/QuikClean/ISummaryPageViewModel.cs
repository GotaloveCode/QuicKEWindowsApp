using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuicKE.Client
{
    public interface ISummaryPageViewModel : IViewModel
    {
        ICommand ConfirmCommand { get; }
        string Name { get; }
        int Age { get; }
        string IDNumber { get; }
        string Badge { get; }
        string Location { get; }
        int ExpertID { get; }
        ObservableCollection<ServiceItem> Services
        {
            get;
        }
    }
}
