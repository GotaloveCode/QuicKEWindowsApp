using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyIoC;
using Windows.UI.Popups;

namespace QuicKE.Client
{
    public class ChargePageViewModel : ViewModel, IChargePageViewModel
    {
        
        public ICommand BackCommand
        {
            get;private set;
        }

        public string Cost { get { return GetValue<string>(); } set { SetValue(value); } }
        
        public string Status { get { return GetValue<string>(); } set { SetValue(value); } }

        public string Code { get { return GetValue<string>(); } set { SetValue(value); } }

        public ICommand ProceedCommand
        {
            get; private set;
        }

        public ICommand MaidsCommand
        {
            get; private set;
        }

        public string SummaryText { get { return GetValue<string>(); } set { SetValue(value); } }

        ErrorBucket errors = new ErrorBucket();

        public ChargePageViewModel()
        {

        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);

            //Cost = "0";

            if (MFundiRuntime.ServiceTypeID == 1)
                SummaryText = "A minimum standard fee will be charged on your account";
            else
                SummaryText = "A minimum finder's fee will be charged on your account";

            ProceedCommand = new DelegateCommand((args) => Proceed(args as CommandExecutionContext));
            MaidsCommand = new NavigateCommand<IViewMaidPageViewModel>(host);


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
                        if(item.NativeId == MFundiRuntime.ServiceTypeID)
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

        private async void Proceed(CommandExecutionContext context)
        {
            if (context == null)
                context = new CommandExecutionContext();

            string message = string.Empty;

            if (MFundiRuntime.ServiceTypeID ==2)
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


        async Task RequestPay()
        {
            Status = "Requesting Payment..";
            var proxy = TinyIoCContainer.Current.Resolve<IRequestPaymentServiceProxy>();           
            
            using (EnterBusy())
            {
                
                var result = await proxy.RequestPaymentAsync(MFundiRuntime.ServiceTypeID);
                

                if (!(result.HasErrors))
                {
                    Code = result.Data;
                    if (!string.IsNullOrEmpty(Code))
                        await delay5();
                }
                else
                    errors.CopyFrom(result);
                if (errors.HasErrors)
                    await Host.ShowAlertAsync(errors);
            }
            Status = string.Empty;
        }

        //async Task delay15()
        //{
        //    await Task.Delay(15000);
        //    await ConfirmPayment();
        //}
        async Task delay5()
        {
            await Task.Delay(5000);
            await ConfirmPayment();
        }

        async Task ConfirmPayment()
        {
            var proxy = TinyIoCContainer.Current.Resolve<IConfirmPaymentServiceProxy>();

            using (EnterBusy())
            {
                //Code = "R968FZWC";
                var result = await proxy.ConfirmPaymentAsync(Code);
                Status = "Verifying Payment..";

                if (!(result.HasErrors))
                {

                    switch (result.Status)
                    {
                        case  "canceled":
                            await Host.ShowAlertAsync("To proceed with transaction click 'Proceed to view Full profile'");
                            break;
                        case "error":
                            await Host.ShowAlertAsync("An error occured while trying to complete your request.");
                            break;
                        case "waiting":
                            await delay5();
                            break;
                        case "success":
                            await Host.ShowAlertAsync("We have received your payment.\n Kindy [proceed to view full profile");
                            Host.ShowView(typeof(IViewMaidPageViewModel));
                            break;
                        default:
                            break;
                    }
                }
                else
                    errors.CopyFrom(result);
                if (errors.HasErrors)
                    await Host.ShowAlertAsync(errors);
            }
            Status = string.Empty;
        }
    }


    }

