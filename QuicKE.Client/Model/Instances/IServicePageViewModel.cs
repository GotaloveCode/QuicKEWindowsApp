using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MFundi.Client
{
    interface IServicePageViewModel : IViewModel
    {
        ICommand ContinueCommand { get; }

        ObservableCollection<ServiceItem> Items
        {
            get;
        }
      
    }
}
