using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuicKE.Client
{
    public class ProfilePageViewModel : ViewModel, IProfilePageViewModel
    {
        public ICommand ViewTicketCommand { get; private set; }

        public string Name { get { return GetValue<string>(); } set { SetValue(value); } }
        public string Phone  { get { return GetValue<string>(); } set { SetValue(value); } }
        public string Email { get { return GetValue<string>(); } set { SetValue(value); } }
        public ProfilePageViewModel()
        {

        }
        public override void Initialize(IViewModelHost host)
        {
            base.Initialize(host);

            this.ViewTicketCommand = new NavigateCommand<ITicketPageViewModel>(host);
        }
        public async override void Activated(object args)
        {
            base.Activated(args);
            using (this.EnterBusy())
            {
                var hasprofile = await ProfileItem.IsCacheEmpty();
                if (hasprofile)
                    await ProfileItem.UpdateCacheFromServerAsync();
                IEnumerable<ProfileItem> profiles = new List<ProfileItem>();
                profiles = await ProfileItem.GetAllFromCacheAsync();
                ProfileItem Profile = profiles.FirstOrDefault();
                this.Name = Profile.name;
                this.Phone = Profile.phone;
                this.Email = Profile.email;
            }
        }
    }
}
