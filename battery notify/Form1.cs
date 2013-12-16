using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;

namespace battery_notify
{
 
    public partial class Form1 : Form
    {

        PowerStatus power = SystemInformation.PowerStatus;



        public Form1()
        {

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            notifyIcon1.ShowBalloonTip(2000, "Battery Full", "Please Unplug the Charger", ToolTipIcon.Warning);
            RefreshStatus();
            timer1.Enabled = true;

            float secondsRemaining = power.BatteryLifePercent;
            if (secondsRemaining >= 0)
            {
                ChargeRemaining.Text = (secondsRemaining * 100).ToString() + "% available.";
            }
            else
            {
                ChargeRemaining.Text = string.Empty;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            RefreshStatus();
        }

        private void RefreshStatus()
        {
            int powerPercent = (int)(power.BatteryLifePercent * 100);

            switch (power.PowerLineStatus)
            {

                case PowerLineStatus.Online:
                    if (powerPercent >= 99)
                    {
                        this.TopMost = true;
                        this.FormBorderStyle = FormBorderStyle.None;
                        this.WindowState = FormWindowState.Maximized;
                        batteryFull.Text = "Battery FULL!!";
                    }
                    else if (powerPercent == 100 && power.BatteryChargeStatus.ToString() == "NoSystemBattery")
                    {
                        this.TopMost = false;
                        this.FormBorderStyle = FormBorderStyle.Sizable;
                        this.WindowState = FormWindowState.Normal;
                        batteryFull.Text = "No Battery!!";
                    }
                    else 
                    {
                        this.FormBorderStyle = FormBorderStyle.Sizable;
                        this.TopMost = false;
                        batteryFull.Text = "Charging!!";
                    }
                    groupBox1.Text = "Charger Connected";
                    float secondsRemaining = power.BatteryLifePercent;
                    if (secondsRemaining >= 0)
                    {
                        ChargeRemaining.Text = (secondsRemaining * 100).ToString() + "% available.";
                    }
                    else
                    {
                        ChargeRemaining.Text = string.Empty;
                    }
                    BatteryStatus.Text = power.BatteryChargeStatus.ToString();
                    break;

                case PowerLineStatus.Offline:

                    if (this.TopMost == true && this.FormBorderStyle == FormBorderStyle.None) {
                        this.TopMost = false;
                        this.FormBorderStyle = FormBorderStyle.Sizable;
                        this.WindowState = FormWindowState.Normal;
                    }

                        secondsRemaining = power.BatteryLifePercent;
                        ChargeRemaining.Text = (secondsRemaining * 100).ToString() + "% available.";
                        batteryFull.Text = "Connect Charger!!";
                        groupBox1.Text = "Running On Battery";
                        BatteryStatus.Text = power.BatteryChargeStatus.ToString();

                    break;

                case PowerLineStatus.Unknown:                    
                        batteryFull.Text = "No Battery available";
                        groupBox1.Text = "Running On Charger";
                    break;
            }

        }


        private void timer1_Tick_1(object sender, EventArgs e)
        {
            RefreshStatus();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\run", true);

            if (checkBox1.Checked == true)
            {
                rk.SetValue("Battery Notifier", Application.ExecutablePath.ToString());
            }
            else {
                rk.DeleteValue("Battery Notifier", false);                
            }
        }

    }
}
