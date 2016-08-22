using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuicKE.Client
{
    public interface ISelectExpertPageViewModel : IViewModel
    {
        ICommand SelectExpertCommand { get; }
        ObservableCollection<ExpertItem> Experts
        {
            get;
        }
        ExpertItem SelectedExpert { get;}
    }
}
