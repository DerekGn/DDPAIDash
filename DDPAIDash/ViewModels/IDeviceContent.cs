using Windows.UI.Xaml.Media;

namespace DDPAIDash.ViewModels
{
    public interface IDeviceContent
    {
        string Name { get; }

        ImageSource Image { get; }
    }
}