using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyIoC;
using Windows.UI.Xaml.Media.Imaging;

namespace QuicKE.Client
{
    public class ViewMaidPageViewModel : ViewModel, IViewMaidPageViewModel
    {
        public ICommand HireCommand { get; private set; }
        public ICommand CallCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand RequestOtherCommand { get; private set; }
        public string name { get { return GetValue<string>(); } set { SetValue(value); } }
        public int id { get { return GetValue<int>(); } set { SetValue(value); } }
        public string age { get { return GetValue<string>(); } set { SetValue(value); } }
        public string id_number { get { return GetValue<string>(); } set { SetValue(value); } }
        public string badge { get { return GetValue<string>(); } set { SetValue(value); } }
        public string phone { get { return GetValue<string>(); } set { SetValue(value); } }
       
        public bool IsMonthly { get; set; }
        public BitmapImage bitmap { get { return GetValue<BitmapImage>(); } set { SetValue(value); } }

        ErrorBucket errors = new ErrorBucket();
        //xaml images cannot use a base64 string as their source. We'll need to create a bitmap image instead. 
         

        public ViewMaidPageViewModel()
        {
        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);

            HireCommand = new NavigateCommand<IViewMaidPageViewModel>(host);
            CancelCommand = new NavigateCommand<IHomePageViewModel>(host);
            CallCommand = new DelegateCommand((args) => Call(args as CommandExecutionContext));
            RequestOtherCommand = new DelegateCommand((args) => Other(args as CommandExecutionContext));
            if (MFundiRuntime.ServiceTypeID == 2)
                IsMonthly = true;
            else
                IsMonthly = false;
        }

        public async override void Activated(object args)
        {
            base.Activated(args);
            await GetMaid();

        }

        private async Task GetMaid()
        {
            var proxy = TinyIoCContainer.Current.Resolve<IGetMaidsServiceProxy>();
            using (EnterBusy())
            {
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
                }
                else
                    errors.CopyFrom(result);

                if (errors.HasErrors)
                    await Host.ShowAlertAsync(errors);
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
            MakePhoneCall("+"+phone, name);

        }

        private async void Other(CommandExecutionContext context)
        {
            // if we don't have a context, create one...
            if (context == null)
                context = new CommandExecutionContext();

            await GetMaid();
        }


        private async void Hire(CommandExecutionContext context)
        {
            // if we don't have a context, create one...
            if (context == null)
                context = new CommandExecutionContext();

            //servicecall 
            var proxy = TinyIoCContainer.Current.Resolve<IHireServiceProxy>();
            // call...
            using (EnterBusy())
            {
                var result = await proxy.HireAsync(id);
                if (!(result.HasErrors))
                {

                    Host.ShowView(typeof(IHomePageViewModel));
                }
                else
                    errors.CopyFrom(result);
            }
            if (errors.HasErrors)
                await this.Host.ShowAlertAsync(errors);
        }



    }
}
