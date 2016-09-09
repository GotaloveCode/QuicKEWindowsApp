using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyIoC;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace QuicKE.Client
{
     public class HistoryPageViewModel : ViewModel, IHistoryPageViewModel
    {
        public ICommand HireCommand { get; private set; }
        public ICommand CallCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand RequestOtherCommand { get; private set; }

        public int ticketID { get { return GetValue<int>(); } set { SetValue(value); } }
        public int id { get { return GetValue<int>(); } set { SetValue(value); } }
        public string name { get { return GetValue<string>(); } set { SetValue(value); } }
        public string age { get { return GetValue<string>(); } set { SetValue(value); } }
        public string id_number { get { return GetValue<string>(); } set { SetValue(value); } }
        public string badge { get { return GetValue<string>(); } set { SetValue(value); } }
        public string phone { get { return GetValue<string>(); } set { SetValue(value); } }
        public BitmapImage bitmap { get { return GetValue<BitmapImage>(); } set { SetValue(value); } }
        public string Code { get { return GetValue<string>(); } set { SetValue(value); } }
        public int Count { get { return GetValue<int>(); } set { SetValue(value); } }
        public bool IsMonthly { get { return GetValue<bool>(); } set { SetValue(value); } }

        //xaml images cannot use a base64 string as their source. We'll need to create a bitmap image instead. 


        public HistoryPageViewModel()
        {
        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);

            HireCommand = new DelegateCommand((args) => Hire(args as CommandExecutionContext));
            CancelCommand = new NavigateCommand<IHomePageViewModel>(host);
            CallCommand = new DelegateCommand((args) => Call(args as CommandExecutionContext));
            RequestOtherCommand = new DelegateCommand((args) => Other(args as CommandExecutionContext));
           
        }

        public async override void Activated(object args)
        {
            base.Activated(args);
            var values = ApplicationData.Current.LocalSettings.Values;
           

            if (values.ContainsKey("TicketID"))
            {
                
                //ticketID = 74;
                //MFundiRuntime.TicketID = "74";
                //values["TicketID"] = "74";
                ticketID = int.Parse(values["TicketID"].ToString());                
                await LoadTicket();
            }
            else
            {
                await Host.ShowAlertAsync("No pending tickets");                
            }

        }

        private async Task GetMaid()
        {
            ErrorBucket errors = new ErrorBucket();

            var proxy = TinyIoCContainer.Current.Resolve<IGetMaidsServiceProxy>();
            using (EnterBusy())
            {
                await Host.ToggleProgressBar(true, "Fetching expert profile ...");

                var result = await proxy.GetMaidAsync();

                if (!(result.HasErrors))
                {
                    name = result.Maid.name;
                    id_number = result.Maid.id_number;
                    phone = result.Maid.phone;
                    id = result.Maid.id;

                    bitmap = new BitmapImage();
                    if (!string.IsNullOrEmpty(result.Maid.photo))
                    {
                        //Bitmap images use a URI or a stream as their source, so let's convert our base64 image string to a stream
                        using (var stream = new MemoryStream(Convert.FromBase64String(result.Maid.photo)))
                        {
                            //Bitmaps in WinRT use an IRandomAccessStream as their source
                            await bitmap.SetSourceAsync(stream.AsRandomAccessStream());
                        }
                        //Bitmap is ready for binding to source
                    }
                    age = result.Maid.age.ToString();
                    badge = result.Maid.badge;
                    ticketID = result.Maid.ticketID;

                    MFundiRuntime.TicketID = result.Maid.ticketID.ToString();
                    //save TicketID to use to mark task as done
                    ApplicationData.Current.LocalSettings.Values["TicketID"] = result.Maid.ticketID.ToString();

                    System.Diagnostics.Debug.WriteLine("TicketID: " + ticketID.ToString());                    

                    if (result.Maid.expert_type == "Full Time")
                    {
                        Count = result.Maid.remaining;
                        System.Diagnostics.Debug.WriteLine(Count);
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

        private async Task LoadTicket()
        {
            ErrorBucket errors = new ErrorBucket();

            var proxy = TinyIoCContainer.Current.Resolve<ILoadHistoryServiceProxy>();
            using (EnterBusy())
            {
                await Host.ToggleProgressBar(true, "Fetching expert profile ...");

                var result = await proxy.GetMaidAsync();

                if (!(result.HasErrors))
                {
                    name = result.Maid.name;
                    id_number = result.Maid.id_number;
                    phone = result.Maid.phone;
                    id = result.Maid.id;

                    bitmap = new BitmapImage();
                    if (!string.IsNullOrEmpty(result.Maid.photo))
                    {
                        //Bitmap images use a URI or a stream as their source, so let's convert our base64 image string to a stream
                        using (var stream = new MemoryStream(Convert.FromBase64String(result.Maid.photo)))
                        {
                            //Bitmaps in WinRT use an IRandomAccessStream as their source
                            await bitmap.SetSourceAsync(stream.AsRandomAccessStream());
                        }                        
                    }
                    age = result.Maid.age.ToString();
                    badge = result.Maid.badge;
                    ticketID = result.Maid.ticketID;
                    
                    MFundiRuntime.TicketID = result.Maid.ticketID.ToString();
                    //save TicketID to use to mark task as done
                    ApplicationData.Current.LocalSettings.Values["TicketID"] = result.Maid.ticketID.ToString();
                    System.Diagnostics.Debug.WriteLine("TicketID: " + ticketID.ToString());                    

                    if (result.Maid.expert_type == "Full Time")
                    {
                        IsMonthly = true;
                        Count = result.Maid.remaining;
                    }else
                    {
                        IsMonthly = false;
                    }
                        
                   
                    System.Diagnostics.Debug.WriteLine(Count);
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


        public void MakePhoneCall(string PhoneNumber, string DisplayName)
        {
            Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(PhoneNumber, DisplayName);
        }

        private void Call(CommandExecutionContext context)
        {
            if (context == null)
                context = new CommandExecutionContext();
            //append + to number so it appears as +254
            MakePhoneCall("+" + phone, name);

        }

        private async void Other(CommandExecutionContext context)
        {
            // if we don't have a context, create one...
            if (context == null)
                context = new CommandExecutionContext();

            if (Count > 0)
                await GetMaid();
            else
                await Host.ShowAlertAsync("Please pay to view other experts");
        }


        private async void Hire(CommandExecutionContext context)
        {
            // if we don't have a context, create one...
            if (context == null)
                context = new CommandExecutionContext();

            ErrorBucket errors = new ErrorBucket();

            //servicecall 
            var proxy = TinyIoCContainer.Current.Resolve<IHireServiceProxy>();
            // call...
            using (EnterBusy())
            {
                await Host.ToggleProgressBar(true, "Confirming expert as hired ...");

                var result = await proxy.HireAsync(ticketID);

                if (!(result.HasErrors))
                {
                    var toast = new ToastNotificationBuilder(new string[] { result.Message });                    
                    toast.Update();
                    //hired maid so clear ticketid and code
                    ApplicationData.Current.LocalSettings.Values.Remove("Code");
                    ApplicationData.Current.LocalSettings.Values.Remove("TicketID");
                    Host.ShowView(typeof(IHomePageViewModel));
                }
                else
                    errors.CopyFrom(result);

                await Host.ToggleProgressBar(false);
            }
            if (errors.HasErrors)
                await Host.ShowAlertAsync(errors);
        }



    }
}

