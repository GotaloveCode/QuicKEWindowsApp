using System.Windows.Input;
using TinyIoC;
using Windows.ApplicationModel.Resources;
using Windows.Storage;

namespace QuicKE.Client
{
    public class EvaluationPageViewModel : ViewModel, IEvaluationPageViewModel
    {
        public double Rating { get { return GetValue<double>(); } set { SetValue(value); } }
        public string Review { get { return GetValue<string>(); } set { SetValue(value); } }
        public string TicketID { get { return GetValue<string>(); } set { SetValue(value); } }
        ResourceLoader res = ResourceLoader.GetForCurrentView();
        

        public ICommand SubmitCommand
        {
            get; private set;
        }

        public EvaluationPageViewModel()
        {
        }
       

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);
            SubmitCommand = new DelegateCommand((args) => Evaluate(args as CommandExecutionContext));
        }

        public override void Activated(object args)
        {
            base.Activated(args);

        }


        private async void Evaluate(CommandExecutionContext context)
        {
            ErrorBucket errors = new ErrorBucket();
            // if we don't have a context, create one...
            if (context == null)
                context = new CommandExecutionContext();
            if(Rating < 0.5 || Review == null)
            {
                await Host.ShowAlertAsync(res.GetString("Rating"));                
            }
            else
            {
                //servicecall 
                var proxy = TinyIoCContainer.Current.Resolve<IEvaluateServiceProxy>();
                // call...
                using (EnterBusy())
                {
                    await Host.ToggleProgressBar(true, res.GetString("SendingComments"));

                    var result = await proxy.EvaluateAsync(Rating, Review, MFundiRuntime.TicketID);

                    if (!(result.HasErrors))
                    {
                        //remove dailyticketid to remove mark as done icon
                        ApplicationData.Current.LocalSettings.Values.Remove("DailyTicketID");
                        await Host.ShowAlertAsync(result.Message);
                        Host.ShowView(typeof(IHomePageViewModel));
                    }
                    else
                    {
                        errors.CopyFrom(result);
                    }

                    await Host.ToggleProgressBar(false);
                }

                if (errors.HasErrors)
                    await Host.ShowAlertAsync(errors);
            }
        }



    }
}
