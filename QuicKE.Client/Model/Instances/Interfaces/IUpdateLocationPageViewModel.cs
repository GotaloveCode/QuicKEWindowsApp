using System.Collections.Generic;
using System.Windows.Input;

namespace QuicKE.Client
{
    public interface IUpdateLocationPageViewModel : IViewModel
    {
        ICommand SubmitCommand
        {
            get;
        }
        List<string> Locations
        {
            get;
        }
        string SelectedLocation { get; }
    }
}