/**
* MIT License
*
* Copyright (c) 2016 Derek Goslin < http://corememorydump.blogspot.ie/ >
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using DDPAIDash.Controls;
using DDPAIDash.Core.Types;
using DDPAIDash.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace DDPAIDash
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _delegate;

        public MainPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        ///     Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">
        ///     Event data that describes how this page was reached.
        ///     This parameter is typically used to configure the page.
        /// </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.

            VideoGridView.ItemsSource = DeviceModel.Instance.Videos;
            EventImageGridView.ItemsSource = DeviceModel.Instance.EventImages;
            EventVideosGridView.ItemsSource = DeviceModel.Instance.EventVideos;
        }

        private void btnCamera_Click(object sender, RoutedEventArgs e)
        {
#warning TODO load user info from settings
#warning TODO async
            DeviceModel.Instance.DeviceInstance.Connect(new UserInfo("012345678912345", "admin", "admin", 0));
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings));
        }

        private void VideoGridView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            HandleDeviceVideoGridViewContainerContentChanging(args, deviceFileViewer =>
            {
                var video = args.Item as Video;

                deviceFileViewer.ShowPlaceholder(video.DeviceVideo.Image, video.DisplayName);
            });
        }

        private void EventVideosGridView_ContainerContentChanging(ListViewBase sender,
            ContainerContentChangingEventArgs args)
        {
            HandleDeviceVideoGridViewContainerContentChanging(args, deviceFileViewer =>
            {
                var video = args.Item as EventVideo;

                deviceFileViewer.ShowPlaceholder(video.Event.VideoThumbnail, video.DisplayName);
            });
        }

        private void EventImageGridView_ContainerContentChanging(ListViewBase sender,
            ContainerContentChangingEventArgs args)
        {
            HandleDeviceVideoGridViewContainerContentChanging(args, deviceFileViewer =>
            {
                var image = args.Item as EventImage;

                deviceFileViewer.ShowPlaceholder(image.Event.ImageThumbnail, image.DisplayName);
            });
        }

        private void VideoGridView_ItemClickHandler(object sender, ItemClickEventArgs e)
        {
            VideoMediaElement.Source =
                new Uri(string.Format("{0}/{1}", DeviceModel.Instance.DeviceInstance.BaseAddress,
                    ((Video) e.ClickedItem).DeviceVideo.Name));
        }

        private void EventVideosGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            VideoMediaElement.Source =
                new Uri(string.Format("{0}/{1}", DeviceModel.Instance.DeviceInstance.BaseAddress,
                    ((EventVideo)e.ClickedItem).Event.BVideoName));
        }

        private void EventImageGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
        }

        private void VideoSaveButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void EventVideoSaveButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void EventImageSaveButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void MediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
        }

        private void HandleDeviceVideoGridViewContainerContentChanging(ContainerContentChangingEventArgs args,
            Action<DeviceVideoViewer> mapFile)
        {
            var deviceFileViewer = args.ItemContainer.ContentTemplateRoot as DeviceVideoViewer;

            if (args.InRecycleQueue)
            {
                deviceFileViewer.ClearData();
            }
            else if (args.Phase == 0)
            {
                mapFile(deviceFileViewer);

                args.RegisterUpdateCallback(ContainerContentChangingDelegate);
            }
            else if (args.Phase == 1)
            {
                deviceFileViewer.ShowName();
                args.RegisterUpdateCallback(ContainerContentChangingDelegate);
            }
            else if (args.Phase == 2)
            {
                deviceFileViewer.ShowImage();
            }

            args.Handled = true;
        }

        private void DisplayFlyoutOnHolding(object sender, HoldingRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement) sender);
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> ContainerContentChangingDelegate
        {
            get
            {
                if (_delegate == null)
                {
                    _delegate = VideoGridView_ContainerContentChanging;
                }
                return _delegate;
            }
        }
    }
}