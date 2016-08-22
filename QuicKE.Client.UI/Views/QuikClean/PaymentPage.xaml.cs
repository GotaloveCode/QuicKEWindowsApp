using QuicKE.Client.UI.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace QuicKE.Client.UI
{
   [ViewModel(typeof(IPaymentPageViewModel))]
    public sealed partial class PaymentPage : MFundiPage
    {

        public PaymentPage()
        {
            this.InitializeComponent();
            string TicketID = (string)ApplicationData.Current.LocalSettings.Values["TicketID"];
            string TicketCost = (string)ApplicationData.Current.LocalSettings.Values["TicketCost"];
            string Token = (string)ApplicationData.Current.LocalSettings.Values["Token"];

            if (string.IsNullOrEmpty(TicketID))
            {
                var toast = new ToastNotificationBuilder(new string[] { "No pending tickets" });
                toast.Update();
                ShowView(typeof(IHomePageViewModel));
            }
            Uri MyUri = new Uri("http://139.59.186.10/mfundi/public/api/pay?ticket=" + TicketID + "&amount=" + TicketCost + "&token=" + Token);
            MyWebView.Navigate(MyUri);
            this.InitializeViewModel();
            
        }

       

    }
}
