using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuicKE.Client
{
    // base class for view-model implementations...
    public interface IViewModel : INotifyPropertyChanged
    {
        void Initialize(IViewModelHost host);
        // shared busy flag...
        bool IsBusy { get; }

        // called when the view is activated...
        void Activated(object args);
        
    }
}
