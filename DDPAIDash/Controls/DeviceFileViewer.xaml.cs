using DDPAIDash.Core.Types;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DDPAIDash.Controls
{
    public sealed partial class DeviceFileViewer : UserControl
    {
        private DeviceFile _deviceFile;

        public DeviceFileViewer()
        {
            this.InitializeComponent();
        }

        public void ShowPlaceholder(DeviceFile deviceFile)
        {
            _deviceFile = deviceFile;
            nameTextBlock.Opacity = 0;
            image.Opacity = 0;
        }
        public void ShowName()
        {
            nameTextBlock.Text = _deviceFile.Name;
            nameTextBlock.Opacity = 1;
        }
        public void ShowImage()
        {
            image.Source = _deviceFile.Image;
            image.Opacity = 1;
        }
        public void ClearData()
        {
            _deviceFile = null;
            nameTextBlock.ClearValue(TextBlock.TextProperty);
            image.ClearValue(Image.SourceProperty);
        }
    }
}
