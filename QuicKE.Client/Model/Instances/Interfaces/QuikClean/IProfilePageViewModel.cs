using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuicKE.Client
{
    public interface IProfilePageViewModel:IViewModel
    {
        ICommand ViewTicketCommand
        {
            get;
        }
         string Name { get; set; }
         string Phone { get; set; }
         string Email { get; set; }
    }
}
