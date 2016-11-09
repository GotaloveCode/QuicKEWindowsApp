using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyIoC;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.UI.Popups;

namespace QuicKE.Client
{
    public class ChargePageViewModel : ViewModel, IChargePageViewModel
    {


        public string Cost { get { return GetValue<string>(); } set { SetValue(value); } }

        public string Code { get { return GetValue<string>(); } set { SetValue(value); } }

        public ICommand ProceedCommand { get; private set; }

        public string SummaryText { get { return GetValue<string>(); } set { SetValue(value); } }

        ErrorBucket errors = new ErrorBucket();
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        ResourceLoader res = ResourceLoader.GetForCurrentView();


        public ChargePageViewModel()
        {
        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);

            if (MFundiRuntime.ServiceTypeID == 1)
            {
               SummaryText = res.GetString("StandardFee");                
            }
            else
            {
                SummaryText = res.GetString("FinderFee");               
            }

            //prompt to pay and proceed to view maid
            ProceedCommand = new DelegateCommand((args) => Proceed(args as CommandExecutionContext));

        }

        public override async void Activated(object args)
        {
            base.Activated(args);

            var proxy = TinyIoCContainer.Current.Resolve<IGetChargesServiceProxy>();

            using (EnterBusy())
            {
                var result = await proxy.GetServicesAsync();

                if (!(result.HasErrors))
                {
                    foreach (var item in result.Services)
                    {
                        if (item.Id == MFundiRuntime.ServiceTypeID)
                        {
                            Cost = item.Cost.ToString();
                            return;
                        }
                    }
                }
                else
                    errors.CopyFrom(result);

                if (errors.HasErrors)
                    await Host.ShowAlertAsync(errors);

            }

        }

        //prompt to pay and proceed to view maid
        private async void Proceed(CommandExecutionContext context)
        {
            if (context == null)
                context = new CommandExecutionContext();

            string message = string.Empty;

            if (MFundiRuntime.ServiceTypeID == 2)
            {
                message = string.Format(res.GetString("Finders"), Cost);
            }
            else
            {
                message = string.Format(res.GetString("ServiceCharge"), Cost);
            }

            MessageDialog dialog = new MessageDialog(message);
            dialog.Commands.Add(new UICommand(res.GetString("Cancel/Content")));
            dialog.Commands.Add(new UICommand(res.GetString("OK"), async delegate (IUICommand command)
            {
                await RequestPay();
            }));

            
            await dialog.ShowAsync();

        }


        private async Task RequestPay()
        {
            var proxy = TinyIoCContainer.Current.Resolve<IRequestPaymentServiceProxy>();

            using (EnterBusy())
            {
                await Host.ToggleProgressBar(true, res.GetString("RequestingPayment"));
                
                var result = await proxy.RequestPaymentAsync(MFundiRuntime.ServiceTypeID);


                if (!(result.HasErrors))
                {
                    Code = result.Data;
                    //TODO remove after debug
                    System.Diagnostics.Debug.WriteLine(Code);
                    if (!string.IsNullOrEmpty(Code))
                        await delay5();
                }
                else
                {
                    errors.CopyFrom(result);
                }

                if (errors.HasErrors)
                    await Host.ShowAlertAsync(errors);

                await Host.ToggleProgressBar(false);

            }
        }


        private async Task delay5()
        {

            await Task.Delay(5000);

            await ConfirmPayment();
        }

        //confirm mpesa received
        private async Task ConfirmPayment()
        {
            var proxy = TinyIoCContainer.Current.Resolve<IConfirmPaymentServiceProxy>();

            using (EnterBusy())
            {
                //Code = "925TDFUZ";
                await Host.ToggleProgressBar(true, res.GetString("VerifyingPayment"));

                var result = await proxy.ConfirmPaymentAsync(Code);

                if (!(result.HasErrors))
                {

                    switch (result.Status)
                    {
                        case "canceled":
                            await Host.ShowAlertAsync(res.GetString("ProceedTransaction"));
                            break;
                        case "error":
                            await Host.ShowAlertAsync(res.GetString("ErrorRequest."));
                            break;
                        case "waiting":
                            await delay5();
                            break;
                        case "success":
                            var toast = new ToastNotificationBuilder(new string[] { res.GetString("PaymentReceived"), res.GetString("WeReceived") });
                            // toast.ImageUri = "ms-appx:///Assets/Toast.jpg";
                            toast.Update();
                            localSettings.Values["Code"] = Code;
                            Host.ShowView(typeof(IViewMaidPageViewModel));
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    errors.CopyFrom(result);
                }

                if (errors.HasErrors)
                    await Host.ShowAlertAsync(errors);

                await Host.ToggleProgressBar(false);
            }
        }




    }

}

