using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.IO;
using NetStat.Models;

namespace NetStat
{
    public partial class frmMain : Form
    {
        Timer timer;
        ConfigModel _cfgModel;
        volatile bool lastStatus = false;
        public frmMain()
        {
            InitializeComponent();
            using (var stream = File.OpenRead("offic.ico"))
            {
                this.notifyIcon1.Icon = new Icon(stream);
            }
            _cfgModel = new ConfigModel();
            readConfigFile();
            this.Visible = false;

        }

        void startPing()
        {
            if (timer != null)
            {
                timer.Enabled = false;
                timer.Stop();
                timer = null;
            }

            timer = new Timer();
            timer.Interval = _cfgModel.pingDelay.Value * 1000;
            timer.Tick += Timer_Tick;
            timer.Enabled = true;
            timer.Start();
        }

        void readConfigFile()
        {
            try
            {
                using (var reader = new StreamReader("config.txt"))
                {
                    var txt = reader.ReadToEnd();
                    _cfgModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigModel>(txt);
                }
            }
            catch
            {

            }
            finally
            {

                if (_cfgModel == null)
                    _cfgModel = new ConfigModel();

                if (string.IsNullOrWhiteSpace(_cfgModel.IPAddress))
                    _cfgModel.IPAddress = "4.2.2.4";

                if (_cfgModel.pingDelay == null || _cfgModel.pingDelay <= 0)
                    _cfgModel.pingDelay = 5;

                startPing();
            }
        }
        void writeConfigFile()
        {
            try
            {
                using (var reader = new StreamWriter("config.txt"))
                {
                    reader.Write(Newtonsoft.Json.JsonConvert.SerializeObject(_cfgModel));
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error on write config file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Ping();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void setAddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 250,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Please enter an address to ping",
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = "Address to ping" };
            TextBox textBox = new TextBox() { Left = 50, Top = 40, Width = 400, Text = _cfgModel.IPAddress };

            Label textLabel2 = new Label() { Left = 50, Top = 100, Text = "Ping Delay (Second)" };
            TextBox txtPingDelay = new TextBox() { Left = 50, Top = 120, Width = 100, Text = _cfgModel.pingDelay?.ToString() };

            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 170, DialogResult = DialogResult.OK };
            confirmation.Click += (sender2, e2) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(txtPingDelay);
            prompt.Controls.Add(textLabel2);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            prompt.MaximizeBox = false;
            prompt.FormBorderStyle = FormBorderStyle.FixedSingle;
            prompt.ControlBox = false;

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                _cfgModel.IPAddress = textBox.Text;
                _cfgModel.pingDelay = short.Parse(txtPingDelay.Text);
                writeConfigFile();
            }

        }

        bool Ping()
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(_cfgModel.IPAddress);
                pingable = reply.Status == IPStatus.Success;

                if (pingable)
                {
                    using (var stream = File.OpenRead("onic.ico"))
                    {
                        this.notifyIcon1.Icon = new Icon(stream);
                    }

                    //if (!lastStatus)
                    //{
                    //    lastStatus = true;
                    //    notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                    //    notifyIcon1.BalloonTipText = "Internet is connected.";
                    //    notifyIcon1.BalloonTipTitle = "Status";
                    //    notifyIcon1.ShowBalloonTip(3000);
                    //}
                }
                else
                {
                    //if (lastStatus)
                    //{
                    //    lastStatus = false;
                    //    notifyIcon1.BalloonTipIcon = ToolTipIcon.Error;
                    //    notifyIcon1.BalloonTipText = "Internet is disconnected.";
                    //    notifyIcon1.BalloonTipTitle = "Status";
                    //    notifyIcon1.ShowBalloonTip(3000);
                    //}
                }

            }
            catch (PingException)
            {
                using (var stream = File.OpenRead("offic.ico"))
                {
                    this.notifyIcon1.Icon = new Icon(stream);
                }

                //if (lastStatus)
                //{
                //    lastStatus = false;
                //    notifyIcon1.BalloonTipIcon = ToolTipIcon.Error;
                //    notifyIcon1.BalloonTipText = "Internet is disconnected.";
                //    notifyIcon1.BalloonTipTitle = "Status";
                //    notifyIcon1.ShowBalloonTip(3000);
                //}
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return pingable;
        }

        protected override void OnLoad(EventArgs e)
        {
            PlaceLowerRight();
            base.OnLoad(e);
        }

        private void PlaceLowerRight()
        {
            //Determine "rightmost" screen
            Screen rightmost = Screen.AllScreens[0];
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.Right > rightmost.WorkingArea.Right)
                    rightmost = screen;
            }

            this.Left = rightmost.WorkingArea.Right - this.Width;
            this.Top = rightmost.WorkingArea.Bottom - this.Height;
        }




        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void frmMain_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }


    }
}
