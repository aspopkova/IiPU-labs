using System.Collections.Generic;
using System.Linq;
using USB.Model;
using System.IO;
using MediaDevices;

namespace USB.Api
{
    public class UsbManager
    { 
        //Method for getting results in list of devices
        public List<UsbDevice> GetUserUsbDevices()
        {
            var usbDevices = new List<UsbDevice>();

            //Getting USB and MTP drives
            var diskDrives = DriveInfo.GetDrives().Where(d => d.IsReady && d.DriveType == DriveType.Removable).ToList();
            var mtpDrives = MediaDevice.GetDevices().ToList();

            //We can't eject MTP and we don't have size of it, so we should work with it differently
            foreach (var device in mtpDrives)
            {
                //Connect to our MTP device
                device.Connect();
                //If our device isn't usual USB
                if (device.DeviceType != DeviceType.Generic)
                {
                    //We add this device to list as MTP
                    usbDevices.Add(new UsbDevice(device.FriendlyName));
                }
            }
            //After MTP we count and perform other USB devices
            foreach (var drive in diskDrives)
            {
                usbDevices.Add(new UsbDevice(drive.Name, drive.TotalFreeSpace, drive.TotalSize - drive.TotalFreeSpace, drive.TotalSize));
            }

            return usbDevices;
        }
    }
}