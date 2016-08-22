using System;
using System.Windows.Input;
using TinyIoC;

namespace QuicKE.Client
{
    public class SignInPageViewModel : ViewModel, ISignInPageViewModel
    {
        // defines the username settings key...
        internal const string LastUsernameKey = "LastUsername";
        public ICommand SignInCommand { get; private set; }

        public SignInPageViewModel()
        {
        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);


            this.SignInCommand = new DelegateCommand((args) => DoLogin(args as CommandExecutionContext));

        }

        public string Username
        {
            get
            {
                return this.GetValue<string>();
            }
            set
            {
                this.SetValue(value);
            }
        }

        public string Password
        {
            get
            {
                return this.GetValue<string>();
            }
            set
            {
                this.SetValue(value);
            }

        }


        private async void DoLogin(CommandExecutionContext context)
        {
            // validate...
            ErrorBucket errors = new ErrorBucket();
            Validate(errors);

            // get a handler...
            var proxy = TinyIoCContainer.Current.Resolve<ISignInServiceProxy>();
            // call...
            using (this.EnterBusy())
            {
                var result = await proxy.SignInAsync(this.Username, this.Password);
                if (!(result.HasErrors))
                {
                    // check if user has database by checking if username matches LastUsername or LastEmail key in setting db
                    string LastUsername = await SettingItem.GetValueAsync("LastUsername");
                    string LastEmail = await SettingItem.GetValueAsync("LastEmail");
                    MFundiRuntime.LogonToken = result.token;
                    if (Username.Equals(LastUsername, StringComparison.OrdinalIgnoreCase) && Username.Equals(LastEmail, StringComparison.OrdinalIgnoreCase))
                    {
                        // while we're here - store a setting containing the logon name of the user...
                        await SettingItem.SetValueAsync(LastUsernameKey, this.Username);
                    }
                    else
                    {
                        //call IGetMyProfileServiceProxy get details  
                        var proxy2 = TinyIoCContainer.Current.Resolve<IGetMyProfileServiceProxy>();
                        var user = await proxy2.GetProfileAsync();
                        if (!(user.HasErrors))
                        {
                            MFundiRuntime.UserDatabaseConnectionString = string.Format("MFundi-user-{0}.db", user.Profile.phone);
                            var conn = MFundiRuntime.GetUserDatabase();
                            await conn.CreateTableAsync<UserItem>();
                            await UserItem.SetValueAsync("token", result.token);
                            await UserItem.SetValueAsync("phonenumber", user.Profile.phone);
                            await UserItem.SetValueAsync("email", user.Profile.email);
                            await UserItem.SetValueAsync("fullname", user.Profile.name);
                            await UserItem.SetValueAsync("password", Password);
                            var conn1 = MFundiRuntime.GetSystemDatabase();
                            await SettingItem.SetValueAsync("LastUsername", user.Profile.phone);
                            await SettingItem.SetValueAsync("LastEmail", user.Profile.email);
                        }
                        else
                            errors.CopyFrom(user);
                    }
                    // show the reports page...
                    if (!errors.HasErrors)
                        this.Host.ShowView(typeof(IServicesPageViewModel));
                }
                else
                    errors.CopyFrom(result);
            }


            // errors?
            if (errors.HasErrors)
                await this.Host.ShowAlertAsync(errors);
        }

        private void Validate(ErrorBucket errors)
        {
            // do basic data presence validation...
            if (string.IsNullOrEmpty(Username))
                errors.AddError("Username or Email is required.");
            if (string.IsNullOrEmpty(Password))
                errors.AddError("Password is required.");
        }

        //public override async void Activated()
        //{
        //    base.Activated();

        //    // restore the setting...
        //    this.Username = await SettingItem.GetValueAsync(LastUsernameKey);
        //}

    }
}
