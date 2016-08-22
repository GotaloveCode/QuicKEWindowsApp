using System;
using System.Windows.Input;
using TinyIoC;
using Windows.Storage;

namespace QuicKE.Client
{
    // concrete implementation of the RegisterPage's view-model...
    public class RegisterPageViewModel : ViewModel, IRegisterPageViewModel
    {

        public ICommand SignUpCommand { get { return this.GetValue<ICommand>(); } private set { this.SetValue(value); } }
        public ICommand SignInCommand { get { return this.GetValue<ICommand>(); } private set { this.SetValue(value); } }

        internal const string LastPhonenumberKey = "LastPhoneNumber";
        internal const string LastEmailKey = "LastEmail";
        internal const string LogonTokenKey = "LogonToken";
        internal const string LastUserPhoneNumberKey = "LastUserPhoneNumber";

        public bool IsSelected { get { return GetValue<bool>(); } set { SetValue(value); } }
        public string FullName { get { return GetValue<string>(); } set { SetValue(value); } }
        public string Email { get { return GetValue<string>(); } set { SetValue(value); } }
        public string Password { get { return GetValue<string>(); } set { SetValue(value); } }
        public string Confirm { get { return GetValue<string>(); } set { SetValue(value); } }
        public string PhoneNumber { get { return GetValue<string>(); } set { SetValue(value); } }

        public RegisterPageViewModel()
        {

        }
        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);
            this.SignUpCommand = new DelegateCommand((args) => DoRegister(args as CommandExecutionContext));
            this.SignInCommand = new DelegateCommand((args) => DoLogin(args as CommandExecutionContext));
            this.IsSelected = true;
           

        }



        private async void DoLogin(CommandExecutionContext context)
        {
            // if we don't have a context, create one...
            if (context == null)
                context = new CommandExecutionContext();
            // validate...
            ErrorBucket errors = new ErrorBucket();
            Validate(errors);

            // ok?
            if (!(errors.HasErrors))
            {
                // get a handler...
                var proxy = TinyIoCContainer.Current.Resolve<ISignInServiceProxy>();
                // call...
                using (this.EnterBusy())
                {
                    var result = await proxy.SignInAsync(this.Email, this.Password);
                    if (!(result.HasErrors))
                    {
                        ApplicationData.Current.LocalSettings.Values["LoggedIn"] = "True";
                        // check if user has database by checking if username matches LastUserPhoneNumber or LastEmail key in setting db
                        string LastUserPhoneNumber = await SettingItem.GetValueAsync("LastUserPhoneNumber");
                        string LastEmail = await SettingItem.GetValueAsync("LastEmail");
                        MFundiRuntime.LogonToken = result.token;
                        // save the logontoken
                        await SettingItem.SetValueAsync("LogonToken", result.token);
                        //not last logged user
                        if (!Email.Equals(LastUserPhoneNumber, StringComparison.OrdinalIgnoreCase) || !Email.Equals(LastEmail, StringComparison.OrdinalIgnoreCase))
                        {
                            //call IGetMyProfileServiceProxy get userdetails  
                            var proxy2 = TinyIoCContainer.Current.Resolve<IGetMyProfileServiceProxy>();
                            var user = await proxy2.GetProfileAsync();
                            if (!(user.HasErrors))
                            {
                                MFundiRuntime.UserDatabaseConnectionString = string.Format("MFundi-user-{0}.db", user.Profile.phone);
                                var conn = MFundiRuntime.GetUserDatabase();
                                await conn.CreateTableAsync<UserItem>();
                                await conn.CreateTableAsync<ProfileItem>();
                                await conn.CreateTableAsync<TicketItem>();
                                await UserItem.SetValueAsync("token", result.token);
                                await UserItem.SetValueAsync("phonenumber", user.Profile.phone);
                                await UserItem.SetValueAsync("email", user.Profile.email);
                                await UserItem.SetValueAsync("fullname", user.Profile.name);
                                await UserItem.SetValueAsync("password", Password);
                                var conn1 = MFundiRuntime.GetSystemDatabase();
                                await SettingItem.SetValueAsync("LastUserPhoneNumber", user.Profile.phone);
                                await SettingItem.SetValueAsync("LastEmail", user.Profile.email);
                                

                            }
                        }

                        // show the home page...
                        this.Host.ShowView(typeof(IHomePageViewModel));
                    }
                    else
                        errors.CopyFrom(result);
                }
            }
            // errors?
            if (errors.HasErrors)
                await this.Host.ShowAlertAsync(errors);
        }

        private async void DoRegister(CommandExecutionContext context)
        {
            // if we don't have a context, create one...
            if (context == null)
                context = new CommandExecutionContext();

            // validate...
            ErrorBucket errors = new ErrorBucket();
            ValidateSignUp(errors);

            // ok?
            if (!(errors.HasErrors))
            {
                // get a handler...
                var proxy = TinyIoCContainer.Current.Resolve<IRegisterServiceProxy>();

                using (this.EnterBusy())
                {
                    var result = await proxy.RegisterAsync(this.FullName, this.PhoneNumber, this.Email, this.Password);

                    // ok?
                    if (!(result.HasErrors))
                    {
                        // show a message to say that a user has been created... (this isn't a helpful message, 

                        await this.Host.ShowAlertAsync(string.Format("User {0} has been registered successfuly.", this.FullName));
                        if (result.status == "success")
                        {
                            // save the username as the last used...
                            await SettingItem.SetValueAsync("LogonToken", result.token);
                            // logon... pass through the username as each user gets their own database...
                            await MFundiRuntime.SignUpAsync(FullName, PhoneNumber, Email, Password, result.token);
                            this.Host.ShowView(typeof(IHomePageViewModel));
                        }
                        else
                        {
                            errors.AddError(result.status);
                            await this.Host.ShowAlertAsync(errors);
                        }

                    }
                    else
                        errors.CopyFrom(result);
                }

            }

            // errors?
            if (errors.HasErrors)
                await this.Host.ShowAlertAsync(errors);
        }

        private void Validate(ErrorBucket errors)
        {
            // do basic data presence validation...
            if (string.IsNullOrEmpty(Email))
                errors.AddError("Email is required.");
            if (string.IsNullOrEmpty(Password) || Password.Length < 6)
                errors.AddError("Password must be at least 6 characters.");
        }

        private void ValidateSignUp(ErrorBucket errors)
        {
            // do basic data presence validation...
            if (string.IsNullOrEmpty(FullName))
                errors.AddError("FullName is required.");
            if (string.IsNullOrEmpty(Email))
                errors.AddError("Email required.");
            if (string.IsNullOrEmpty(Password))
                errors.AddError("Password is required.");
            if (string.IsNullOrEmpty(Confirm))
                errors.AddError("Confirm password is required.");
            if (string.IsNullOrEmpty(PhoneNumber))
                errors.AddError("PhoneNumber is required.");
            // check the passwords...
            if (!(string.IsNullOrEmpty(Password)) && this.Password != this.Confirm)
                errors.AddError("The passwords do not match.");
            if (Password.Length < 6)
                errors.AddError("The passwords must be at least 6 characters.");
            if ((string.IsNullOrEmpty(PhoneNumber)) && PhoneNumber.Length < 12)
                errors.AddError("Invalid phone number.");
        }

        public async override void Activated(object args)
        {
            base.Activated(args);
            string LogonToken = null;
            using (this.EnterBusy())
            {
                LogonToken = await SettingItem.GetValueAsync("LogonToken");
            }
            if (!(string.IsNullOrEmpty(LogonToken)))
            {
                // show the home page as already logged in...
                this.Host.ShowView(typeof(IHomePageViewModel));
            }
            

        }
    }
}
