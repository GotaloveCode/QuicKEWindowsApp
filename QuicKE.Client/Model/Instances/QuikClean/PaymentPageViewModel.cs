using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace QuicKE.Client.Model.Instances
{
    public class PaymentPageViewModel : ViewModel, IPaymentPageViewModel
    {
        public Uri MyUri { get {return this.GetValue<Uri>(); } set { this.SetValue(value); } }
        public string TicketID { get { return this.GetValue<string>(); } set { this.SetValue(value); } }
        public string TicketCost { get { return this.GetValue<string>(); } set { this.SetValue(value); } }
        public string Token { get { return this.GetValue<string>(); } set { this.SetValue(value); } }
        
        public PaymentPageViewModel()
        {

        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);
            //this.TicketID = (string)ApplicationData.Current.LocalSettings.Values["TicketID"];
            //this.TicketCost = (string)ApplicationData.Current.LocalSettings.Values["TicketCost"];
            //this.Token = (string)ApplicationData.Current.LocalSettings.Values["Token"];
            //this.MyUri = new Uri("http://139.59.186.10/mfundi/public/api/pay?ticket=" + this.TicketID + "&amount=" + this.TicketCost + "&token=" + this.Token);
            //http://139.59.186.10/mfundi/public/api?ticket=51&amount=40&token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOjEwNywiaXNzIjoiaHR0cDpcL1wvMTM5LjU5LjE4Ni4xMFwvbWZ1bmRpXC9wdWJsaWNcL2FwaVwvcmVnaXN0ZXIiLCJpYXQiOjE0NjYwNzY3OTUsImV4cCI6MTQ5NzYxMjc5NSwibmJmIjoxNDY2MDc2Nzk1LCJqdGkiOiIyN2EyN2Y3ZTZhYzE3OTA3Y2E4YzgxM2NkNzU3NmFkMiJ9.Q2svzGk_m0qIHN_JyrQamPRM2CETCso2VECsXzl7ZTQ");

        }
        //public override async void Activated(object args)
        //{
        //    base.Activated(args);
        //    await DoRefresh();

            
        //}
        private async Task DoRefresh()
        {
            // run...
            using (this.EnterBusy())
            {
                this.TicketID = (string)ApplicationData.Current.LocalSettings.Values["TicketID"];
                this.TicketCost = (string)ApplicationData.Current.LocalSettings.Values["TicketCost"];
//await UserItem.GetValueAsync("TicketID");
                this.TicketCost = await UserItem.GetValueAsync("TicketCost");
                this.Token = MFundiRuntime.LogonToken;
                this.MyUri = new Uri("http://139.59.186.10/mfundi/public/api/pay?ticket=" + this.TicketID + "&amount=" + this.TicketCost + "&token=" + this.Token);
                this.OnPropertyChanged("MyUri");
            }
        }
    }
}
