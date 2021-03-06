﻿using System.Management;


namespace Laba_5
{
    class Device
    {
        public string Guid { get; set; }
        public string HardwareId { get; set; }
        public string Manufacturer { get; set; }
        public string Provider { get; set; }
        public string DriverDescription { get; set; }
        public string SysPath { get; set; }
        public string DevicePath { get; set; }
        public string Status { get; set; }
        public bool CanDisable { get; set; }

        public Device(
            string guid,
            string hard,
            string manufacturer,
            string provider,
            string desc,
            string sys,
            string path,
            string status)
        {
            Guid = guid;
            HardwareId = hard;
            Manufacturer = manufacturer;
            Provider = provider;
            DriverDescription = desc;
            SysPath = sys;
            DevicePath = path;
            Status = status;
            CanDisable = true;
        }
        public void DisEnaDevice(string method)
        {
            var tempDevId = this.DevicePath.Remove(this.DevicePath.Length - 2);
            ManagementObject tempCurrentElement = null;
            var deviceList = new ManagementObjectSearcher("SELECT * FROM Win32_PNPEntity");
            foreach (ManagementObject item in deviceList.Get())
            {
                if (tempDevId == item["DeviceID"].ToString())
                {
                    tempCurrentElement = item;
                    break;
                }
            }
            if (tempCurrentElement != null)
            {
                tempCurrentElement.InvokeMethod(method, new object[] { false });
            }
        }
    }
}
