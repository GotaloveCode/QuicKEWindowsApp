using System.Windows.Input;
using TinyIoC;
using Windows.ApplicationModel.Resources;
using Windows.Storage;

namespace QuicKE.Client
{
    class NewPassPageViewModel : ViewModel, INewPassPageViewModel
    {
        public string Code { get; set; }
       
        public string Confirm { get; set; }

        public ICommand SubmitCommand { get; set; }

        public string Password { get; set; }

        public string PhoneNumber
        {
            get
            {
                if (localSettings.Values.ContainsKey("VerifyPhone"))
                    return localSettings.Values["VerifyPhone"].ToString();
                else
                    return "";
            }

            set { SetValue(value); }
        }

        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        ResourceLoader res = ResourceLoader.GetForCurrentView();


        public NewPassPageViewModel()
        {
        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);

            SubmitCommand = new DelegateCommand((args) => Submit(args as CommandExecutionContext));
            //if (localSettings.Values.ContainsKey("VerifyPhone"))
            //    PhoneNumber = localSettings.Values["VerifyPhone"].ToString();
        }

        private async void Submit(CommandExecutionContext context)
        {
            if (context == null)
                context = new CommandExecutionContext();
            ErrorBucket errors = new ErrorBucket();

            Validate(errors);

            if (!(errors.HasErrors))
            {
                var proxy = TinyIoCContainer.Current.Resolve<INewPassServiceProxy>();


                using (EnterBusy())
                {
                    var result = await proxy.VerifyAsync(PhoneNumber,Password,Confirm,Code);

                    // ok?
                    if (!(result.HasErrors))
                    {
                        if (result.Status != "success")
                        {
                            errors.CopyFrom(result);
                        }
                        else
                        {
                            await Host.ShowAlertAsync(result.Message);
                            Host.ShowView(typeof(IRegisterPageViewModel));
                        }
                           
                    }
                    else
                        errors.CopyFrom(result);

                    if (errors.HasErrors)
                        await Host.ShowAlertAsync(errors.GetErrorsAsString());
                }
            }
            else
            {
                await Host.ShowAlertAsync(errors.GetErrorsAsString());
            }
        }

        //validate 
        private void Validate(ErrorBucket errors)
        {
            // do basic data presence validation...
            if (string.IsNullOrEmpty(PhoneNumber))
            {
                errors.AddError(res.GetString("RequiredPhone"));
            }

            if (string.IsNullOrEmpty(Password))
            {
                errors.AddError(res.GetString("RequiredPassword"));
            }

            if (string.IsNullOrEmpty(Confirm))
            {
                errors.AddError(res.GetString("txtConfirm/Text"));
            }

            if (!(string.IsNullOrEmpty(Password)) && Password.Length < 6)
            {
                errors.AddError(res.GetString("ValidPassword"));
            }
            // check the passwords...
            if (!(string.IsNullOrEmpty(Password)) && !(string.IsNullOrEmpty(Confirm)) && Password != Confirm)
            {
                errors.AddError(res.GetString("MatchPassword"));
            }

            if (string.IsNullOrEmpty(Code))
            {
                errors.AddError(res.GetString("RequiredCode"));
            }

        }

    }
}

