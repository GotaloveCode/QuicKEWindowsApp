using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyIoC;
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


        public ChargePageViewModel()
        {
        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);

            if (MFundiRuntime.ServiceTypeID == 1)
                SummaryText = "A minimum standard fee will be charged on your account";
            else
                SummaryText = "A minimum finder's fee will be charged on your account";

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
                message = string.Format("A finders fee of KES {0} Will be charged from your MPESA to view the expert's full profile", Cost);
            else
                message = string.Format("A Service charge of KES {0} Will be charged from your MPESA.Click below to proceed", Cost);

            MessageDialog dialog = new MessageDialog(message);

            dialog.Commands.Add(new UICommand("Cancel"));

            dialog.Commands.Add(new UICommand("OK", async delegate (IUICommand command)
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
                await Host.ToggleProgressBar(true, "Requesting Payment ...");

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
            //TODO remove after debug
            System.Diagnostics.Debug.WriteLine("About to wait 5 seconds");
            await Task.Delay(5000);
            await ConfirmPayment();
        }

        //confirm mpesa received
        private async Task ConfirmPayment()
        {
            var proxy = TinyIoCContainer.Current.Resolve<IConfirmPaymentServiceProxy>();

            using (EnterBusy())
            {
                Code = "925TDFUZ";
                await Host.ToggleProgressBar(true, "Verifying Payment....");
                //TODO remove after debug
                System.Diagnostics.Debug.WriteLine("now about to verify payment");

                var result = await proxy.ConfirmPaymentAsync(Code);

                if (!(result.HasErrors))
                {
                    //TODO remove after debug
                    System.Diagnostics.Debug.WriteLine("result returned Status: " + result.Status);

                    switch (result.Status)
                    {
                        case "canceled":
                            await Host.ShowAlertAsync("To proceed with transaction click 'Proceed to view Full profile'");
                            break;
                        case "error":
                            await Host.ShowAlertAsync("An error occured while trying to complete your request.");
                            break;
                        case "waiting":
                            await delay5();
                            break;
                        case "success":
                            await Host.ToggleProgressBar(true, "We have received your payment.\n Kindy proceed to view full profile");
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

