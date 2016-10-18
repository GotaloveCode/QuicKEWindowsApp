using System;
using System.Collections.Generic;
using System.Globalization;
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
        public ICommand SignInCommand { get; private set; }
        public ICommand VerifyCommand { get; private set; }
        public ICommand UpdateListCommand { get; private set; }
        private int TicketID { get { return GetValue<int>(); } set { SetValue(value); } }
        private List<string> locations { get { return GetValue<List<string>>(); } set { SetValue(value); } }
        public List<string> Locations { get { return GetValue<List<string>>(); } set { SetValue(value); } }
        public string SelectedLocation { get { return GetValue<string>(); } set { SetValue(value); } }
        public string SignInText { get { return GetValue<string>(); } set { SetValue(value); } }
        CultureInfo culture = CultureInfo.CurrentCulture;
        public bool IsSelected
        {
            get
            {

                return GetValue<bool>();
            }
            set
            {
                if (value == true)
                {
                    if (culture.Name == "fr")
                        SignInText = "CONNEXION";
                    else
                        SignInText = "SIGN IN";
                }
                else
                {
                    if (culture.Name == "fr")
                        SignInText = "CRÉER UN COMPTE";
                    else
                        SignInText = "SIGN UP";
                }

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
            {
                if (culture.Name == "fr")
                    await Host.ShowAlertAsync("Entrer un numéro de téléphone valide");
                else
                    await Host.ShowAlertAsync("Enter a valid phone number");
            }
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

            Validate(errors);

            // ok?
            if (!(errors.HasErrors))
            {
                // get a handler...
                var proxy = TinyIoCContainer.Current.Resolve<ISignInServiceProxy>();
                // call...
                using (EnterBusy())
                {
                    if (culture.Name == "fr")
                        await Host.ToggleProgressBar(true, "Connexion en cours ...");
                    else
                        await Host.ToggleProgressBar(true, "Signing In ...");

                    var result = await proxy.SignInAsync(PhoneNumber, Password);

                    if (!(result.HasErrors))
                    {

                        var values = ApplicationData.Current.LocalSettings.Values;

                        //assign new token to global class
                        MFundiRuntime.LogonToken = result.token;

                        // save the logontoken
                        localSettings.Values["LogonToken"] = result.token;

                        //fetch pendingtickets & navigate to home
                        await GetCurrentTickets();

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


                if (culture.Name == "fr")
                    await Host.ToggleProgressBar(true, "Enregistrement en cours ...");
                else
                    await Host.ToggleProgressBar(true, "Signing Up ...");

                var result = await proxy.RegisterAsync(FullName, PhoneNumber, Password, SelectedLocation, Code, Email);

                // ok?
                if (!(result.HasErrors))
                {
                    if (result.status == "success")
                    {
                        if (culture.Name == "fr")
                            await Host.ToggleProgressBar(true, string.Format("{0} votre compte a été enregistré avec succès.", FullName));
                        else
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

            // errors?
            if (errors.HasErrors)
                await Host.ShowAlertAsync(errors);
        }

        //validate login
        private void Validate(ErrorBucket errors)
        {
            // do basic data presence validation...
            if (string.IsNullOrEmpty(PhoneNumber))
            {
                if (culture.Name == "fr")
                    errors.AddError("Numéro de téléphone est exigé.");
                else
                    errors.AddError("PhoneNumber is required.");
            }

            if (string.IsNullOrEmpty(Password) || Password.Length < 6)
            {
                if (culture.Name == "fr")
                    errors.AddError("Le mot de passe doit être au moins de 6 caractères");
                else
                    errors.AddError("The Password must be at least 6 characters.");
            }

        }

        //validate register
        private void ValidateSignUp(ErrorBucket errors)
        {
            // do basic data presence validation...
            if (string.IsNullOrEmpty(FullName))
            {
                if (culture.Name == "fr")
                    errors.AddError("Nom complet est exigé.");
                else
                    errors.AddError("FullName is required.");
            }
                
            if (string.IsNullOrEmpty(Password))
            {
                if (culture.Name == "fr")
                    errors.AddError("Mot de passe requis.");
                else
                    errors.AddError("Password is required.");
            }
            
            if (string.IsNullOrEmpty(Confirm))
            {
                if (culture.Name == "fr")
                    errors.AddError("Confirmer le mot de passe.");
                else
                    errors.AddError("Confirm password.");
            }
            
            // check the passwords...
            if (!(string.IsNullOrEmpty(Password)) && Password != Confirm)
            {
                if (culture.Name == "fr")
                    errors.AddError("Le mot de passe ne correspond pas.");
                else
                    errors.AddError("The passwords do not match.");
            }
            
            if (Password.Length < 6)
            {
                if (culture.Name == "fr")
                    errors.AddError("Le mot de passe doit être au moins de 6 caractères.");
                else
                    errors.AddError("The password must be at least 6 characters.");
            }
            
            if (string.IsNullOrEmpty(Email))
            {
                if (culture.Name == "fr")
                    errors.AddError("Adresse e-mail est exigé.");
                else
                    errors.AddError("Email is required.");
            }
            
            if (string.IsNullOrEmpty(PhoneNumber))
            {
                if (culture.Name == "fr")
                    errors.AddError("Numéro de téléphone est exigé.");
                else
                    errors.AddError("PhoneNumber is required.");
            }
            
            if ((string.IsNullOrEmpty(PhoneNumber)) && PhoneNumber.Length < 12)
            {
                if (culture.Name == "fr")
                    errors.AddError("Numéro de téléphone invalide.");
                else
                    errors.AddError("Invalid phone number.");
            }
            
            if (string.IsNullOrEmpty(Code))
            {
                if (culture.Name == "fr")
                    errors.AddError("Code de vérification est exigé.");
                else
                    errors.AddError("Verification Code is required.");
            }
            
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
                //to avoid no pending tickets error don't check for errors
            }
        }



    }
}
