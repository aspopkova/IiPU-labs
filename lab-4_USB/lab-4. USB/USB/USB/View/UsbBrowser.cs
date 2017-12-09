using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using USB.Api;
using USB.Model;

namespace USB.View
{
    public partial class UsbBrowser : Form
    {
        private const int UpdateFrequencyMillis = 3000;

        private readonly UsbManager _usbManager;

        private List<UsbDevice> _usbDevices;


        public UsbBrowser()
        {
            InitializeComponent();

            _usbManager = new UsbManager();
            RefreshStatus();
        }


        private async void RefreshStatus()
        {
            // The UI Thread TaskScheduler instance
            var taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            while (true)
            {
                await Task.Run(() => _usbManager.GetUserUsbDevices()).ContinueWith(t => RefreshItems(t.Result), taskScheduler);

                await Task.Delay(UpdateFrequencyMillis);
            }
        }

        private void RefreshItems(List<UsbDevice> usbDeviceList)
        {
            _usbDevices = usbDeviceList;
            usbDevicesListView.Items.Clear();

            foreach (var usbDevice in usbDeviceList)
            {
                var item = new ListViewItem(usbDevice.DeviceName);
                item.SubItems.Add(usbDevice.DeviceTotalMemory);
                item.SubItems.Add(usbDevice.DeviceFreeMemory);
                item.SubItems.Add(usbDevice.DeviceUsedMemory);

                usbDevicesListView.Items.Add(item);
            }
        }

        private UsbDevice GetSelectedUsbDevice(string deviceName)
        {
            UsbDevice usbDevice = null;
            foreach (var device in _usbDevices)
            {
                if (device.DeviceName.Equals(deviceName))
                {
                    usbDevice = device;
                }
            }

            return usbDevice;
        }

        private void usbDevicesListView_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            var deviceName = ((ListView)sender).FocusedItem.Text;
            var device = GetSelectedUsbDevice(deviceName);
            if (device != null)
            {
                if (!device.EjectDevice())
                {
                    MessageBox.Show(@"Device is used! Please, wait ...");
                }
            }
        }
    }
}