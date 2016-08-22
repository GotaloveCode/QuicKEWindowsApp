using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuicKE.Client
{
    // exposes the map of public binding properties on RegisterPage's view-model...
    public interface IRegisterPageViewModel : IViewModel
    {
        string FullName{get;set;}

        string Email{get;set;}

        string Password { get; set; }

        string Confirm {get;set;}
        string PhoneNumber { get; set; }

        ICommand SignUpCommand{get;}
        ICommand SignInCommand { get; }

        //ICommand Verify { get; }

        
    }
}
