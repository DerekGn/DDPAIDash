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

        protected BitmapImage CreateBitmapFromStream(Stream stream)
        {
            var result = new BitmapImage();
            result.SetSource(stream.AsRandomAccessStream());

            return result;
        }
    }
}
