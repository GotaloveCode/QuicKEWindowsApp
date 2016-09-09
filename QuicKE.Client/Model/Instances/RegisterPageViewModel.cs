using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyIoC;
using Windows.Storage;

namespace QuicKE.Client
{
    // concrete implementation of the RegisterPage's view-model...
    public class RegisterPageViewModel : ViewModel, IRegisterPageViewModel
    {

        public ICommand SignUpCommand { get; private set; }
        public ICommand SignInCommand { get; private set ; }
        public ICommand VerifyCommand { get; private set; }
        public ICommand UpdateListCommand { get; private set; }
        private int TicketID { get { return GetValue<int>(); } set { SetValue(value); } }
        private List<string> locations { get { return GetValue<List<string>>(); } set { SetValue(value); } }
        public List<string> Locations { get { return GetValue<List<string>>(); } set { SetValue(value); } }
        public string SelectedLocation { get { return GetValue<string>(); } set { SetValue(value); } }
        public string SignInText { get { return GetValue<string>(); } set { SetValue(value); } }
        public bool IsSelected {
            get {
               
                return GetValue<bool>();
            }
            set {
                if (value == true)
                    SignInText = "SIGN IN";
                else
                    SignInText = "SIGN UP";
                SetValue(value);
            }
        }
        public string FullName { get { return GetValue<string>(); } set { SetValue(value); } }        
        public string Password { get { return GetValue<string>(); } set { SetValue(value); } }
        public string Confirm { get { return GetValue<string>(); } set { SetValue(value); } }
        public string Email { get { return GetValue<string>(); } set { SetValue(value); } }
        public string PhoneNumber { get { return phoneNumber; } set { phoneNumber = value; } }
        private string phoneNumber = "254";
        public string Code { get { return GetValue<string>(); } set { SetValue(value); } }

        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        ErrorBucket errors = new ErrorBucket();


        public RegisterPageViewModel()
        {
        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);

            Locations = new List<string>();
            locations = new List<string>();
            SignUpCommand = new DelegateCommand((args) => DoRegister(args as CommandExecutionContext));
            SignInCommand = new DelegateCommand((args) => DoLogin(args as CommandExecutionContext));
            VerifyCommand = new DelegateCommand((args) => Verify(args as CommandExecutionContext));
            UpdateListCommand = new DelegateCommand((args) => Suggest(args as CommandExecutionContext));
            IsSelected = true;

           
        }

        //verify number
        private async void Verify(CommandExecutionContext context)
        {
            if (context == null)
                context = new CommandExecutionContext();

            if (string.IsNullOrEmpty(PhoneNumber) || PhoneNumber.Length < 12)
                await Host.ShowAlertAsync("Enter valid phone number");
            else
            {
                var proxy = TinyIoCContainer.Current.Resolve<IVerifyServiceProxy>();


                using (EnterBusy())
                {
                    var result = await proxy.VerifyAsync(PhoneNumber);

                    // ok?
                    if (!(result.HasErrors))
                    {
                        if (result.Status != "success")
                            errors.CopyFrom(result);
                    }
                    else
                        errors.CopyFrom(result);

                    if (errors.HasErrors)
                        await Host.ShowAlertAsync(errors.GetErrorsAsString());
                }
            }
            
        }

        //login
        private async void DoLogin(CommandExecutionContext context)
        {

            if (context == null)
                context = new CommandExecutionContext();

            ErrorBucket errors = new ErrorBucket();
            Validate(errors);

            // ok?
            if (!(errors.HasErrors))
            {
                // get a handler...
                var proxy = TinyIoCContainer.Current.Resolve<ISignInServiceProxy>();
                // call...
                using (EnterBusy())
                {
                    await Host.ToggleProgressBar(true, "Signing In ...");

                    var result = await proxy.SignInAsync(PhoneNumber, Password);

                    if (!(result.HasErrors))
                    {
                       
                        var values = ApplicationData.Current.LocalSettings.Values;
                        
                       //assign new token to global class
                        MFundiRuntime.LogonToken = result.token;

                        // save the logontoken
                        localSettings.Values["LogonToken"] = result.token;

                        // navigate to home page...
                        Host.ShowView(typeof(IHomePageViewModel));
                    }
                    else
                        errors.CopyFrom(result);
                   await Host.ToggleProgressBar(false);
                }
            }
            // errors?
            if (errors.HasErrors)
                await Host.ShowAlertAsync(errors);
        }

        //register
        private async void DoRegister(CommandExecutionContext context)
        {
            // if we don't have a context, create one...
            if (context == null)
                context = new CommandExecutionContext();

            // validate...

            ValidateSignUp(errors);

            // ok?
            if (!(errors.HasErrors))
            {
                // get a handler...
                var proxy = TinyIoCContainer.Current.Resolve<IRegisterServiceProxy>();


                using (EnterBusy())
                {
                    await Host.ToggleProgressBar(true, "Signing In...");

                    var result = await proxy.RegisterAsync(FullName, PhoneNumber, Password, SelectedLocation, Code, Email);

                    // ok?
                    if (!(result.HasErrors))
                    {                                                
                        if (result.status == "success")
                        {
                            await Host.ToggleProgressBar(true, string.Format("{0} your account has been registered successfuly.", FullName));

                            //assign new token to global class
                            MFundiRuntime.LogonToken = result.token;

                            // save logontoken
                            localSettings.Values["LogonToken"] = result.token;

                            //save Location                        
                            localSettings.Values["Location"] = SelectedLocation;
                            localSettings.Values["FullName"] = FullName;
                            localSettings.Values["PhoneNumber"] = PhoneNumber;
                            localSettings.Values["Email"] = Email;


                            await Host.ToggleProgressBar(false);

                            Host.ShowView(typeof(IHomePageViewModel));

                        }
                        else
                        {
                            await Host.ToggleProgressBar(false);

                            errors.AddError(result.status);
                            await Host.ShowAlertAsync(errors);
                        }

                    }
                    else
                    {
                        errors.CopyFrom(result);
                        await Host.ToggleProgressBar(false);
                    }
                }

            }

            // errors?
            if (errors.HasErrors)
                await Host.ShowAlertAsync(errors);
        }

        //validate login
        private void Validate(ErrorBucket errors)
        {
            // do basic data presence validation...
            if (string.IsNullOrEmpty(PhoneNumber))
                errors.AddError("PhoneNumber is required.");

            if (string.IsNullOrEmpty(Password) || Password.Length < 6)
                errors.AddError("Password must be at least 6 characters.");
        }

        //validate register
        private void ValidateSignUp(ErrorBucket errors)
        {
            // do basic data presence validation...
            if (string.IsNullOrEmpty(FullName))
                errors.AddError("FullName is required.");
            if (string.IsNullOrEmpty(Password))
                errors.AddError("Password is required.");
            if (string.IsNullOrEmpty(Confirm))
                errors.AddError("Confirm password is required.");            
            // check the passwords...
            if (!(string.IsNullOrEmpty(Password)) && Password != Confirm)
                errors.AddError("The passwords do not match.");
            if ( Password.Length < 6)
                errors.AddError("The passwords must be at least 6 characters.");
            if (string.IsNullOrEmpty(Email))
                errors.AddError("Email is required.");
            if (string.IsNullOrEmpty(PhoneNumber))
                errors.AddError("PhoneNumber is required.");
            if ((string.IsNullOrEmpty(PhoneNumber)) && PhoneNumber.Length < 12)
                errors.AddError("Invalid phone number.");
            if (string.IsNullOrEmpty(Code))
                errors.AddError("Verification Code is required.");
        }

        public async override void Activated(object args)
        {
            base.Activated(args);

            using (EnterBusy())
            {
               await LoadLocations();                
            }
        }

        //get locations
        private async Task LoadLocations()
        {
            var proxy = TinyIoCContainer.Current.Resolve<IGetLocationsServiceProxy>();
            // call...
            using (EnterBusy())
            {
                var result = await proxy.GetLocationsAsync();
                if (!(result.HasErrors))
                {
                    foreach (var item in result.Locations)
                    {
                        Locations.Add(item.name);
                        locations.Add(item.name);
                    }
                        
                }
                else
                    errors.CopyFrom(result);
            }

            // errors?
            if (errors.HasErrors)
                await Host.ShowAlertAsync(errors);
           
        }

        //autosuggest filter
        private void Suggest(CommandExecutionContext context)
        {
            if (context == null)
                context = new CommandExecutionContext();
            if (!string.IsNullOrEmpty(SelectedLocation))
                Locations = locations.Where(x => x.ToLower().Contains(SelectedLocation.ToLower())).ToList();
        }

        //get pendingtickets
        private async Task GetCurrentTickets()
        {

            ErrorBucket errors = new ErrorBucket();

            var proxy = TinyIoCContainer.Current.Resolve<IGetPendingTicketsServiceProxy>();

            using (EnterBusy())
            {
                var result = await proxy.GetTicketAsync();

                if (!(result.HasErrors))
                {
                   
                    if (result.tickets.Count > 0)
                    {
                        TicketID = result.tickets.First().Id;
                        ApplicationData.Current.LocalSettings.Values["TicketID"] = TicketID; 
                        ApplicationData.Current.LocalSettings.Values["DailyTicketID"] = TicketID; 
                   }
                }
                else
                {
                    errors.CopyFrom(result);
                }

                if (errors.HasErrors)
                    await Host.ShowAlertAsync(errors.GetErrorsAsString());
            }
        }



    }
}
