using System.Globalization;
using System.Windows.Input;
using TinyIoC;
using Windows.Storage;

namespace QuicKE.Client
{
    public class EvaluationPageViewModel : ViewModel, IEvaluationPageViewModel
    {
        public double Rating { get { return GetValue<double>(); } set { SetValue(value); } }
        public string Review { get { return GetValue<string>(); } set { SetValue(value); } }
        public string TicketID { get { return GetValue<string>(); } set { SetValue(value); } }
        ErrorBucket errors = new ErrorBucket();
        CultureInfo culture = CultureInfo.CurrentCulture;

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
            // if we don't have a context, create one...
            if (context == null)
                context = new CommandExecutionContext();
            if(Rating < 0.5 || Review == null)
            {
                if (culture.Name == "fr")
                    await Host.ShowAlertAsync("S'il vous plaît fournir une note et un commentaire");
                else
                    await Host.ShowAlertAsync("Please provide a rating and comment");
            }
            else
            {
                //servicecall 
                var proxy = TinyIoCContainer.Current.Resolve<IEvaluateServiceProxy>();
                // call...
                using (EnterBusy())
                {
                    if (culture.Name == "fr")
                        await Host.ShowAlertAsync("Envoi de vos commentaires ...");
                    else
                        await Host.ToggleProgressBar(true, "Sending your comments...");

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
