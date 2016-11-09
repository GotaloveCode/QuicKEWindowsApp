using System.Windows.Input;
using TinyIoC;
using Windows.ApplicationModel.Resources;
using Windows.Storage;

namespace QuicKE.Client
{
    public class ForgotPassPageViewModel : ViewModel, IForgotPassPageViewModel
    {
        ResourceLoader res = ResourceLoader.GetForCurrentView();        
        public ICommand ForgotCommand { get; private set; }
        public string PhoneNumber { get { return phoneNumber; } set { phoneNumber = value; } }
        private string phoneNumber = "254";
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public ForgotPassPageViewModel()
        {
        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);

            ForgotCommand = new DelegateCommand((args) => ForgotPassword(args as CommandExecutionContext));
            
        }

        private async void ForgotPassword(CommandExecutionContext context)
        {
            if (context == null)
                context = new CommandExecutionContext();

            if (string.IsNullOrEmpty(PhoneNumber) || PhoneNumber.Length < 12)
            {
                await Host.ShowAlertAsync(res.GetString("InvalidPhone"));                
            }
            else
            {
                var proxy = TinyIoCContainer.Current.Resolve<IForgotPassServiceProxy>();


                using (EnterBusy())
                {
                    var result = await proxy.VerifyAsync(PhoneNumber);
                    ErrorBucket errors = new ErrorBucket();
                    // ok?
                    if (!(result.HasErrors))
                    {
                        if (result.Status != "success")
                        {
                            errors.CopyFrom(result);
                        }                           
                        else
                        {
                            localSettings.Values["VerifyPhone"] = PhoneNumber;
                            await Host.ShowAlertAsync(result.Message);
                            Host.ShowView(typeof(INewPassPageViewModel));
                        }                         
                    }
                    else
                        errors.CopyFrom(result);

                    if (errors.HasErrors)
                        await Host.ShowAlertAsync(errors.GetErrorsAsString());
                }
            }
        }

    }
}
