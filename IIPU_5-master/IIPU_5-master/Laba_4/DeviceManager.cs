using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Management;

namespace Laba_5
{
    class DeviceManager
    {
        public List<Device> DeviseListCreate()
        {
            List<Device> devices = new List<Device>();
            var deviceList = new ManagementObjectSearcher("\\\\.\\root\\cimv2", "SELECT * FROM Win32_PnPEntity");
            foreach (ManagementObject item in deviceList.Get())
            {
                string tempGUID = "", tempHardware = "", tempManufacturer = "", tempProvider = "", tempDesc = "", tempSys = "", tempPath = "", tempStatus = "";
                if (item["ClassGuid"] != null)
                    tempGUID = item["ClassGuid"].ToString() + "\r\n";
                if (item["HardwareID"] != null)
                    tempHardware = String.Join("\r\n", (string[])item["HardwareID"]) + "\r\n";
                if (item["Manufacturer"] != null)
                    tempManufacturer = item["Manufacturer"].ToString() + "\r\n";
                if (item["Caption"] != null)
                    tempManufacturer = item["Caption"].ToString() + "\r\n";
                tempPath = item["DeviceID"].ToString() + "\r\n";
                var driver = item.GetRelated("Win32_SystemDriver");
                foreach (var driverProperty in driver)
                {
                    tempDesc += driverProperty["Description"].ToString() + "\r\n";
                    tempSys += driverProperty["PathName"].ToString() + "\r\n";
                }
                tempStatus = item["Status"].ToString();
                devices.Add(new Device(tempGUID, tempHardware, tempManufacturer, tempProvider, tempDesc, tempSys, tempPath, tempStatus));
            }
            return devices;
        }
    }
}
