using System.ComponentModel;

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
