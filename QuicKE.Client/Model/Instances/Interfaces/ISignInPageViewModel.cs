using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuicKE.Client
{
    // exposes the map of public binding properties on RegisterPage's view-model...
    public interface ISignInPageViewModel:IViewModel
    {
        string Username
        {
            get;
            set;
        }


        string Password
        {
            get;
            set;
        }

        ICommand SignInCommand
        {
            get;
        }
    }
}
