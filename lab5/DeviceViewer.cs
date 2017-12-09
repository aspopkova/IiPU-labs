using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Laba_5
{
    //Class of form, where we have all important methods for GUI
    public partial class DeviceViewer : Form
    {
        private static readonly DeviceManager Manager = new DeviceManager();
        private List<Device> _deviceList;
        //Our selecting table
        private readonly DataTable _table = new DataTable();

        public DeviceViewer()
        {
            InitializeComponent();
        }

        //When we first load our form
        private void LoadForm(object sender, EventArgs e)
        {
            _deviceList = new List<Device>();
            _table.Columns.Add("Название", typeof(string));

            ReloadForm();
            usbList.DataSource = _table;
            disableButton.Enabled = false;
            enableButton.Enabled = false;
        }
        private void ReloadForm()
        {
            int currentPosition = 0;
            if (usbList.CurrentRow != null)
            {
                currentPosition = usbList.CurrentRow.Index;
            }
            _table.Clear();
            _deviceList = Manager.DeviseListCreate();
            foreach(Device device in _deviceList)
            {
                _table.Rows.Add(device.Guid);
            }
            if (usbList.RowCount - 1 > currentPosition)
            {
                usbList.Rows[currentPosition].Selected = true;
            }
            label1.Text = "";
        }

        //Event for selecting some row in list
        private void ChangeSelect(object sender, EventArgs e)
        {
            //If row is exists
            if (usbList.CurrentRow != null)
            {
                //If there are no index of bounds
                if (usbList.CurrentRow.Index >= 0 && usbList.CurrentRow.Index < _deviceList.Count)
                {
                    spaceTextBox.Text = _deviceList[usbList.CurrentRow.Index].Guid + _deviceList[usbList.CurrentRow.Index].HardwareId +
                        _deviceList[usbList.CurrentRow.Index].Manufacturer + _deviceList[usbList.CurrentRow.Index].Provider +
                        _deviceList[usbList.CurrentRow.Index].DriverDescription + _deviceList[usbList.CurrentRow.Index].SysPath +
                        _deviceList[usbList.CurrentRow.Index].Status;
                    if (_deviceList[usbList.CurrentRow.Index].Status == "OK" && _deviceList[usbList.CurrentRow.Index].CanDisable == true)
                    {
                        disableButton.Enabled = true;
                        enableButton.Enabled = false;
                    }
                    if (_deviceList[usbList.CurrentRow.Index].Status == "Error")
                    {
                        disableButton.Enabled = false;
                        enableButton.Enabled = true;
                    }
                }
                else
                {
                    //In other situations just block everything
                    disableButton.Enabled = false;
                    spaceTextBox.Text = "";
                }
            }
        }

        //Event for clicking deleting button
        private void DisableButton(object sender, EventArgs e)
        {
            //If we choose what device we want to eject
            if (usbList.CurrentRow != null)
            {
                //Then we should invoke a method Eject_Device()
                var device =_deviceList[usbList.CurrentRow.Index];
                device.DisEnaDevice("Disable");
                device.CanDisable = false;
                disableButton.Enabled = false;
                enableButton.Enabled = true;
            }
        }

        private void EnableButton(object sender, EventArgs e)
        {
            //If we choose what device we want to eject
            if (usbList.CurrentRow != null)
            {
                //Then we should invoke a method Eject_Device()
                var device = _deviceList[usbList.CurrentRow.Index];
                device.DisEnaDevice("Enable");
                device.CanDisable = true;
                enableButton.Enabled = false;
                disableButton.Enabled = true;
            }
        }
    }
}
