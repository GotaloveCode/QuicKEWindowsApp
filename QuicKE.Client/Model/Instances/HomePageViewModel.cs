using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyIoC;
using Windows.Storage;
using Windows.UI.Popups;

namespace QuicKE.Client
{
    public class HomePageViewModel : ViewModel, IHomePageViewModel
    {
        //public ICommand ProfileCommand { get; private set; }
        public ICommand ViewMonthlyServiceCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }
        public ICommand ViewDayServiceCommand { get; private set; }
        public ICommand ChangeLocationCommand { get; private set; }
        public ICommand TaskDoneCommand { get; private set; }
        public int TicketId { get { return GetValue<int>(); } set { SetValue(value); } }
        public bool HasPendingTask { get { return GetValue<bool>(); } set { SetValue(value); } }
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public HomePageViewModel()
        {
        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);

            ViewMonthlyServiceCommand = new DelegateCommand((e) =>
            {
                //set servicetypeid
                MFundiRuntime.ServiceTypeID = 2;

                Host.ShowView(typeof(IChargePageViewModel));
            });
            ViewDayServiceCommand = new DelegateCommand((e) =>
            {
                MFundiRuntime.ServiceTypeID = 1;
                Host.ShowView(typeof(IChargePageViewModel));
            });
            LogoutCommand = new DelegateCommand(async (e) =>
            { // sign out
                await LogOut();
            });

            TaskDoneCommand = new DelegateCommand(async (e) =>
            { // mark task done dialog
                await TaskDone();
            });


        }

        //mark task done dialog
        private async Task TaskDone()
        {
            MessageDialog dialog = new MessageDialog("Please Mark Task as done", "Task Complete");

            dialog.Commands.Add(new UICommand("Cancel"));

            dialog.Commands.Add(new UICommand("Confirm", async delegate (IUICommand command)
            {
                await SetDone();

            }));

            await dialog.ShowAsync();
        }

        //server call toset task done #TODO
        private async Task SetDone()
        {
            ErrorBucket errors = new ErrorBucket();

            var proxy = TinyIoCContainer.Current.Resolve<ITaskCompleteServiceProxy>();

            using (EnterBusy())
            {
                var result = await proxy.TaskCompleteAsync();

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
                        if (ApplicationData.Current.LocalSettings.Values.ContainsKey("TicketID"))
                        {
                            if (string.IsNullOrEmpty(MFundiRuntime.TicketID))
                                MFundiRuntime.TicketID = ApplicationData.Current.LocalSettings.Values["TicketID"].ToString();
                            ApplicationData.Current.LocalSettings.Values.Remove("TicketID");
                        }
                        Host.ShowView(typeof(IEvaluationPageViewModel));
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



        //logout
        private async Task LogOut()
        {
            ErrorBucket errors = new ErrorBucket();
            var proxy = TinyIoCContainer.Current.Resolve<ILogOutServiceProxy>();
            using (EnterBusy())
            {
                await Host.ToggleProgressBar(true, "Signing Out ...");

                var result = await proxy.LogOutAsync();
                if (!(result.HasErrors))
                {

                    localSettings.Values.Remove("LoggedIn");
                    localSettings.Values.Remove("LogonToken");

                    MFundiRuntime.LogonToken = null;

                    await Host.ToggleProgressBar(true, "Signed Out Successfully");

                    Host.ShowView(typeof(IRegisterPageViewModel));
                }
                else
                    errors.CopyFrom(result);

                await Host.ToggleProgressBar(false);
            }
            // errors?
            if (errors.HasErrors)
                await Host.ShowAlertAsync(errors);
        }


        public override async void Activated(object args)
        {
            base.Activated(args);

            var values = ApplicationData.Current.LocalSettings.Values;

            if (values.ContainsKey("TicketID"))
            {
                TicketId = int.Parse(localSettings.Values["TicketID"].ToString());
                HasPendingTask = true;
            }
            else
                HasPendingTask = false;

            //If doesn't have location data grab and save profile data
            if (!values.ContainsKey("Location"))
            {
                await GetMyProfile();
            }

        }

            //get profile data
        public async Task GetMyProfile()
        {
            var proxy = TinyIoCContainer.Current.Resolve<IGetMyProfileServiceProxy>();
            using (EnterBusy())
            {
                await Host.ToggleProgressBar(true, "Fetching your Location data ...");

                var result = await proxy.GetProfileAsync();

                if (!result.HasErrors)
                {
                    //save location and profile info
                    localSettings.Values["Location"] = result.Profile.location;
                    localSettings.Values["Name"] = result.Profile.name;
                    localSettings.Values["Phone"] = result.Profile.phone;
                    localSettings.Values["Id"] = result.Profile.id;

                    MFundiRuntime.Location = result.Profile.location;
                }
                else
                {
                    await Host.ShowAlertAsync(result.GetErrorsAsString());
                }

                await Host.ToggleProgressBar(false);

            }
        }

    }
}
