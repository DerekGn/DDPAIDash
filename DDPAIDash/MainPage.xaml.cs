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

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using DDPAIDash.Core.Types;
using DDPAIDash.Model;
using DDPAIDash.Controls;
using Windows.Foundation;
using DDPAIDash.Core;
using System;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace DDPAIDash
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _delegate;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.

            DeviceFileGridView.ItemsSource = DeviceModel.Instance.DeviceFiles;

            GSensorImageGridView.ItemsSource = DeviceModel.Instance.DeviceEvents;

            GSensorVideosGridView.ItemsSource = DeviceModel.Instance.DeviceEvents;
        }

        private void btnCamera_Click(object sender, RoutedEventArgs e)
        {
            DeviceModel.Instance.DeviceInstance.Connect(new UserInfo("012345678912345", "admin", "admin", 0));

            DeviceModel.Instance.LoadFilesAndEvents();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings));
        }

        private void DeviceFileGridView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            Handle_DeviceFileGridView_ContainerContentChanging(args, (deviceFileViewer) => 
            {
                var deviceFile = args.Item as DeviceFile;

                deviceFileViewer.ShowPlaceholder(deviceFile.Image, deviceFile.Name);
            });
        }

        private void GSensorVideosGridView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            Handle_DeviceFileGridView_ContainerContentChanging(args, (deviceFileViewer) =>
            {
                var deviceEvent = args.Item as DeviceEvent;

                deviceFileViewer.ShowPlaceholder(deviceEvent.VideoThumbnail, deviceEvent.BVideoName);
            });
        }

        private void GSensorImageGridView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            Handle_DeviceFileGridView_ContainerContentChanging(args, (deviceFileViewer) =>
            {
                var deviceEvent = args.Item as DeviceEvent;

                deviceFileViewer.ShowPlaceholder(deviceEvent.ImageThumbnail, deviceEvent.ImageName);
            });
        }

        private void Handle_DeviceFileGridView_ContainerContentChanging(ContainerContentChangingEventArgs args, Action<DeviceFileViewer> mapFile)
        {
            DeviceFileViewer deviceFileViewer = args.ItemContainer.ContentTemplateRoot as DeviceFileViewer;

            if (args.InRecycleQueue == true)
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

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> ContainerContentChangingDelegate
        {
            get
            {
                if (_delegate == null)
                {
                    _delegate = new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(DeviceFileGridView_ContainerContentChanging);
                }
                return _delegate;
            }
        }

        private void DeviceFileItem_ItemClickHandler(object sender, ItemClickEventArgs e)
        {

        }

        private void GSensorVideosItem_ItemClickHandler(object sender, ItemClickEventArgs e)
        {

        }

        private void GSensorImageGridView_ItemClickHandler(object sender, ItemClickEventArgs e)
        {

        }
    }
}
