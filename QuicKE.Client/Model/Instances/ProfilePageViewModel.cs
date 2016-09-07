using System;
using System.IO;
using System.Threading.Tasks;
using TinyIoC;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace QuicKE.Client
{
    public class ProfilePageViewModel: ViewModel,IProfilePageViewModel
    {
        public string Location { get { return GetValue<string>(); } set { SetValue(value); } }
        public string FullName { get { return GetValue<string>(); } set { SetValue(value); } }
        public string PhoneNumber { get { return GetValue<string>(); } set { SetValue(value); } }
        public BitmapImage bitmap { get { return GetValue<BitmapImage>(); } set { SetValue(value); } }

        ErrorBucket errors = new ErrorBucket();

        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;


        public ProfilePageViewModel()
        {
        }

        public async override void Activated(object args)
        {
            base.Activated(args);

            if (localSettings.Values.ContainsKey("FullName"))
            {
                Location = localSettings.Values["Location"].ToString();
                FullName = localSettings.Values["FullName"].ToString();
                PhoneNumber = localSettings.Values["PhoneNumber"].ToString();
                string Photo = localSettings.Values["Photo"].ToString();
                bitmap = new BitmapImage();
                if (!string.IsNullOrEmpty(Photo))
                {
                    //Bitmap images use a URI or a stream as their source, so let's convert our base64 image string to a stream
                    using (var stream = new MemoryStream(Convert.FromBase64String(Photo)))
                    {
                        //Bitmaps in WinRT use an IRandomAccessStream as their source
                        await bitmap.SetSourceAsync(stream.AsRandomAccessStream());
                    }
                    //Bitmap is ready for binding to source
                }else
                {
                    Photo = "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";
                    using (var stream = new MemoryStream(Convert.FromBase64String(Photo)))
                    {
                        //Bitmaps in WinRT use an IRandomAccessStream as their source
                        await bitmap.SetSourceAsync(stream.AsRandomAccessStream());
                    }
                }
            }
            else
            {
                using (EnterBusy())
                {
                    await GetProfile();
                }
            }
            
            
        }

        private async Task GetProfile()
        {
            var proxy = TinyIoCContainer.Current.Resolve<IGetMyProfileServiceProxy>();
            // call...
            using (EnterBusy())
            {
                var result = await proxy.GetProfileAsync();
                if (!(result.HasErrors))
                {
                    FullName = result.Profile.name;
                    Location = result.Profile.location;
                    PhoneNumber = result.Profile.phone;
                    bitmap = new BitmapImage();
                    if (!string.IsNullOrEmpty(result.Profile.photo))
                    {
                        //Bitmap images use a URI or a stream as their source, so let's convert our base64 image string to a stream
                        using (var stream = new MemoryStream(Convert.FromBase64String(result.Profile.photo)))
                        {
                            //Bitmaps in WinRT use an IRandomAccessStream as their source
                            await bitmap.SetSourceAsync(stream.AsRandomAccessStream());
                        }
                        //Bitmap is ready for binding to source
                    }
                }
                else
                    errors.CopyFrom(result);
            }

            // errors?
            if (errors.HasErrors)
                await Host.ShowAlertAsync(errors);

        }


    }
}
