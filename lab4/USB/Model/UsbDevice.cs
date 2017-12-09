using System;
using System.Linq;
using UsbEject;

namespace USB.Model
{
    public class UsbDevice      // $"{}" - Интерполяция
    {
        private const long BytesPerGigabyte = 1024 * 1024 * 1024;
        private const int DefaultDeviceNameSize = 5;
        private const int NumberOfDecimals = 2;
        private const string GigabytePostfix = "GB";


        public string DeviceName { get; }

        public string DeviceFreeMemory { get; }

        public string DeviceUsedMemory { get; }

        public string DeviceTotalMemory { get; }


        public UsbDevice(string name)
        {
            DeviceName = name;
        }

        public UsbDevice(string name, long freeMemory, long usedMemory, long totalMemory)
        {
            DeviceName = name;
            DeviceFreeMemory = $"{GetGigabytes(freeMemory)} {GigabytePostfix}";
            DeviceUsedMemory = $"{GetGigabytes(usedMemory)} {GigabytePostfix}";
            DeviceTotalMemory = $"{GetGigabytes(totalMemory)} {GigabytePostfix}";
        }


        //Method for ejecting device with utility RemoveDrive
        public bool EjectDevice()
        {
            if (DeviceName.Length > DefaultDeviceNameSize)
            {
                return false;
            }

            var tempName = DeviceName.Remove(2);
            var ejectedDevice = new VolumeDeviceClass().SingleOrDefault(v => v.LogicalDrive == DeviceName.Remove(2));
            ejectedDevice?.Eject(false);
            ejectedDevice = new VolumeDeviceClass().SingleOrDefault(v => v.LogicalDrive == tempName);

            return ejectedDevice == null;
        }


        private static double GetGigabytes(long bytesCount)
        {
            var gigabytes = Math.Round((double) bytesCount / BytesPerGigabyte, NumberOfDecimals);

            return gigabytes;
        }
    }
}