using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace QuicKE.Client
{
    public class MaidsPageViewModel : ViewModel, IMaidsPageViewModel
    {
        public ICommand ViewMaidCommand { get; private set; }

        public ObservableCollection<MaidItem> Maids { get; set; }
        

        public MaidsPageViewModel()
        {
            
            Maids = new ObservableCollection<MaidItem>();
           

            //    maids.Add(new MaidItem() { Id = 1, Name = "Jeniffer Jones", Age = 23, IDNo="23233245", Pic = "Assets/img_profile.png", Phone = "254710020500" });
            //    maids.Add(new MaidItem() { Id = 2, Name = "Martha Jones", Age = 24, IDNo = "23597889", Pic = "Assets/img_profile_selected.png", Phone = "254710020500" });
            //    maids.Add(new MaidItem() { Id = 3, Name = "Linda Kamau", Age = 25, IDNo="9985606", Pic = "Assets/img_user_profile.png", Phone = "254733320500" });
            //    maids.Add(new MaidItem() { Id = 1, Name = "Suzy Jones", Age = 23, IDNo = "23233245", Pic = "Assets/img_profile.png", Phone = "254710020500" });
            //    maids.Add(new MaidItem() { Id = 1, Name = "Steff Jones", Age = 24, IDNo = "23597889", Pic = "Assets/img_profile.png", Phone = "254710020500" });
            //    maids.Add(new MaidItem() { Id = 3, Name = "Lizzy Kamau", Age = 25, IDNo = "9985606", Pic = "Assets/img_user_profile.png", Phone = "254733320500" });
            //    maids.Add(new MaidItem() { Id = 1, Name = "Tabby Jones", Age = 23, IDNo = "23233245", Pic = "Assets/img_profile.png", Phone = "254710020500" });
            //    maids.Add(new MaidItem() { Id = 1, Name = "Becky Jones", Age = 24, IDNo = "23597889", Pic = "Assets/img_ticket.png", Phone = "254710020500" });
            //    maids.Add(new MaidItem() { Id = 3, Name = "Cathy Kamau", Age = 25, IDNo = "9985606", Pic = "Assets/Microsoft.png", Phone = "254733320500" });
            //    maids.Add(new MaidItem() { Id = 1, Name = "Drew Jones", Age = 23, IDNo = "23233245", Pic = "Assets/img_profile.png", Phone = "254710020500" });
            //    maids.Add(new MaidItem() { Id = 1, Name = "Terry Jones", Age = 24, IDNo = "23597889", Pic = "Assets/monthly service.png", Phone = "254710020500" });
            //    maids.Add(new MaidItem() { Id = 3, Name = "Kelly Kamau", Age = 25, IDNo = "9985606", Pic = "Assets/img_user_profile.png", Phone = "254733320500" });
            //    maids.Add(new MaidItem() { Id = 1, Name = "Gwen Jones", Age = 23, IDNo = "23233245", Pic = "Assets/settings.png", Phone = "254710020500" });
            //    maids.Add(new MaidItem() { Id = 1, Name = "Belinda Jones", Age = 24, IDNo = "23597889", Pic = "Assets/img_profile.png", Phone = "254710020500" });
            //    maids.Add(new MaidItem() { Id = 3, Name = "Sally Kamau", Age = 25, IDNo = "9985606", Pic = "Assets/img_location_pin.png", Phone = "254733320500" });
        }

        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);

            //List<MaidItem> Maids = new List<MaidItem>()
            //{
            //    new MaidItem() { Id = 1, Name="Jeniffer Jones", Age = 23, Pic ="Assets/img_profile.png", Phone = "254710020500"},
            //    new MaidItem() { Id = 1, Name="Jeniffer Jones", Age = 23, Pic ="Assets/img_profile.png", Phone = "254710020500"},,
            //    new MaidItem() { Id = 3, Name="Jeniffer Kamau", Age = 25, Pic ="Assets/img_user_profile.png", Phone = "254733320500"},
            //};
            //MaidItem Maid = new MaidItem()
            //{

            //    IDNo = "28194785",
            //    Name = "Jessie Pinkton",
            //    Age = 24,
            //    Pic = "Assets/img_profile.png",
            //    Phone = "254710020500"
            //};
            ViewMaidCommand = new DelegateCommand((args) => DoLogin(args as CommandExecutionContext));

            //ViewMaidCommand = new NavigateCommand<ITicketPageViewModel>(host);
        }
        public override void Activated(object args)
        {
            base.Activated(args);
            using (EnterBusy())
            {
                List<MaidItem> maids = new List<MaidItem>()
            {
                new MaidItem() { Id = 1, Name="Jeniffer Jones", Age = 23, Pic ="Assets/img_profile.png", Phone = "254710020500"},
                new MaidItem() { Id = 2, Name="Mary Mwende", Age = 26, Pic ="Assets/img_profile_selected.png", Phone = "254711078500"},
                new MaidItem() { Id = 3, Name="Jeniffer Kamau", Age = 25, Pic ="Assets/img_user_profile.png", Phone = "254733320500"},
            };

                //    ObservableCollection<MaidItem> Maids = new ObservableCollection<MaidItem>();
                foreach (var item in maids)
                {
                               

                    Maids.Add(item);
                }
                   
                //var hasprofile = await ProfileItem.IsCacheEmpty();
                //if (hasprofile)
                //    await ProfileItem.UpdateCacheFromServerAsync();
                //IEnumerable<ProfileItem> profiles = new List<ProfileItem>();
                //profiles = await ProfileItem.GetAllFromCacheAsync();
                //ProfileItem Profile = profiles.FirstOrDefault();
                //Name = Profile.name;
                //Phone = Profile.phone;

            }

        }

        private async void DoLogin(CommandExecutionContext context)
        {
            // if we don't have a context, create one...
            if (context == null)
                context = new CommandExecutionContext();
            await Host.ShowAlertAsync("nyangau");
        }

     }
}

