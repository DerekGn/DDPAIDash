using DDPAIDash.Core.Types;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DDPAIDash.Controls
{
    public sealed partial class DeviceFileViewer : UserControl
    {
        ImageSource _source;
        string _text;

        public DeviceFileViewer()
        {
            this.InitializeComponent();
        }

        public void ShowPlaceholder(ImageSource source, string text)
        {
            _source = source;
            _text = text;

            nameTextBlock.Opacity = 0;
            image.Opacity = 0;
        }
        public void ShowName()
        {
            nameTextBlock.Text = _text;
            nameTextBlock.Opacity = 1;
        }
        public void ShowImage()
        {
            image.Source = _source;
            image.Opacity = 1;
        }
        public void ClearData()
        {
            _source = null;
            _text = string.Empty;

            nameTextBlock.ClearValue(TextBlock.TextProperty);
            image.ClearValue(Image.SourceProperty);
        }
    }
}
