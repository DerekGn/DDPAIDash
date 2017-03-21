using System.IO;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace DDPAIDash.ViewModels
{
    public abstract class DeviceContent : IDeviceContent
    {
        public abstract ImageSource Image { get; }
        public abstract string Name { get; }
        public abstract string SourceName { get; }
        public virtual bool Saving { get; set; }

        protected BitmapImage CreateBitmapFromStream(Stream stream)
        {
            var result = new BitmapImage();
            result.SetSource(stream.AsRandomAccessStream());

            return result;
        }
    }
}
