

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TinyIoC;

namespace QuicKE.Client
{
    public class LocationPageViewModel : ViewModel, ILocationPageViewModel
    {
        // defines the username settings key...
        
        public ICommand SubmitCommand { get; private set; }
        public ObservableCollection<string> Locations { get; set; }
        public string SelectedLocation { get; set; }
        ErrorBucket errors = new ErrorBucket();

        public LocationPageViewModel()
        {
        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);

            Locations = new ObservableCollection<string>();
            SubmitCommand = new DelegateCommand((args) => Submit(args as CommandExecutionContext));

        }

 
        
        

        private async void Submit(CommandExecutionContext context)
        {
            await SettingItem.SetValueAsync("Location", SelectedLocation);
            // validate...
            

            // get a handler...
            var proxy = TinyIoCContainer.Current.Resolve<IGetLocationsServiceProxy>();
            // call...
            using (EnterBusy())
            {
                var result = await proxy.GetLocationsAsync();
                if (!(result.HasErrors))
                {
                    
                    // show the reports page...
                    if (!errors.HasErrors)
                        Host.ShowView(typeof(IServicesPageViewModel));
                }
                else
                    errors.CopyFrom(result);
            }


            // errors?
            if (errors.HasErrors)
                await Host.ShowAlertAsync(errors);
        }

       

        public override void Activated(object args)
        {
            base.Activated(args);

            LoadLocations();

            //// get a handler...
            //var proxy = TinyIoCContainer.Current.Resolve<IGetLocationsServiceProxy>();
            //// call...
            //using (EnterBusy())
            //{
            //    var result = await proxy.GetLocationsAsync();
            //    if (!(result.HasErrors))
            //    {
            //        foreach (var item in result.Locations)
            //            Locations.Add(item);                    
            //    }
            //    else
            //    {
            //        errors.CopyFrom(result);
            //        await Host.ShowAlertAsync(errors);
            //    }

            //}


        }

          void LoadLocations()
        {
            List<LocationItem> locs = new List<LocationItem>()
            {
                new LocationItem {id = 1,name = "Kilimani" },
                new LocationItem {id = 2,name = "Milimani" },
                new LocationItem {id = 3,name = "Yaya" },
                new LocationItem {id = 4,name = "Riverside" },
                new LocationItem {id = 5,name = "Westlands" },
                new LocationItem {id = 6,name = "Community" },
            };

            foreach (var item in locs)
                Locations.Add(item.name);
        }

    }
}
