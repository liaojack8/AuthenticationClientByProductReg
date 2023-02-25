namespace AuthenticationClientByAvoidKeypro
{
    using KeyproToAvoid;
    using Microsoft.Win32;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Resources;
    using System.Security.Cryptography;
    using System.Text;
    using System.Windows.Forms;

    public class Form1 : Form
    {
        private int iUseSoftware = 1;
        private string sUseSoftware = Software.MagicShow.ToString();
        private ResourceManager Rm = new ResourceManager("AuthenticationClientByAvoidKeypro.LanguagePack", Assembly.GetExecutingAssembly());
        private IContainer components;
        private GroupBox gb_DeviceInformation;
        private TextBox txt_Key;
        private Label lbl_Key;
        private GroupBox gb_Authentication;
        private Button btn_Authentication;
        private TextBox txt_Authentication;
        private Label lbl_Authentication;
        private Button btn_Export;
        private Button btn_Import;
        private TextBox txt_UUID;
        private Label lbl_UUID;
        private TextBox txt_BIOS;
        private Label lbl_BIOS;
        private TextBox txt_MAC;
        private Label lbl_MAC;
        private TextBox txt_CPU;
        private Label lbl_CPU;

        public Form1()
        {
            this.InitializeComponent();
            this.Text = this.sUseSoftware + "-" + this.Text;
        }

        private void btn_Authentication_Click(object sender, EventArgs e)
        {
            string str = this.txt_Authentication.Text.Replace("-", "").Trim();
            byte[] cipherText = Authentication.HexToBytes(str);
            if (cipherText == null)
            {
                MessageBox.Show(this.Rm.GetString("AuthenticationFails") + this.Rm.GetString("SNIncorrectly"));
            }
            else
            {
                string registry = AuroraRegistry.GetRegistry(this.sUseSoftware + "Licence");
                if (registry != string.Empty)
                {
                    if (registry == str)
                    {
                        MessageBox.Show(this.Rm.GetString("SNSame"));
                        return;
                    }
                    string sN = this.GetSN(Authentication.AuthenticationByDevice(Authentication.HexToBytes(registry)), false);
                    string str4 = this.GetSN(Authentication.AuthenticationByDevice(Authentication.HexToBytes(str)), false);
                    if (MessageBox.Show(this.Rm.GetString("SNExist") + "\r\n\r\n" + this.Rm.GetString("SNOld") + "\r\n" + sN + "\r\n\r\n" + this.Rm.GetString("SNNew") + "\r\n" + str4, this.Rm.GetString("SNCover"), MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                byte[] pBLOCK = Authentication.AuthenticationByDevice(cipherText);
                if (pBLOCK != null)
                {
                    if (Convert.ToInt32(pBLOCK[pBLOCK.Length - 1]) != this.iUseSoftware)
                    {
                        MessageBox.Show(this.Rm.GetString("NotApplicable") + this.sUseSoftware);
                    }
                    else
                    {
                        string sDateTime = null;
                        if (!Authentication.CheckReleaseDate(pBLOCK, ref sDateTime))
                        {
                            MessageBox.Show(this.Rm.GetString("AuthenticationFails") + this.Rm.GetString("ReleaseTime") + sDateTime);
                        }
                        else if (!Authentication.CheckTryDate(pBLOCK, ref sDateTime))
                        {
                            MessageBox.Show(this.Rm.GetString("SNExpired") + this.Rm.GetString("TrialExpirationTime") + sDateTime);
                        }
                        else
                        {
                            AuroraRegistry.SetRegistry(this.sUseSoftware + "Licence", str);
                            string sN = this.GetSN(pBLOCK, true);
                            if (sDateTime != null)
                            {
                                string s = DateTime.Now.ToString("yyyyMMdd");
                                DateTime time = DateTime.ParseExact(s, "yyyyMMdd", null);
                                TimeSpan span = DateTime.Parse(sDateTime).Subtract(time);
                                AuroraRegistry.SetRegistry(this.sUseSoftware + "RegDate", AuroraRegistry.RC2Encrypt(s));
                                AuroraRegistry.SetRegistry(this.sUseSoftware + "CheckDate", AuroraRegistry.RC2Encrypt(s));
                                AuroraRegistry.SetRegistry(this.sUseSoftware + "ValidDate", AuroraRegistry.RC2Encrypt(span.Days.ToString()));
                                MessageBox.Show(this.Rm.GetString("AuthenticationSuccessful") + sN + this.Rm.GetString("TrialExpirationTime") + sDateTime);
                            }
                            else
                            {
                                MessageBox.Show(this.Rm.GetString("AuthenticationSuccessful") + sN);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(this.Rm.GetString("AuthenticationFails") + this.Rm.GetString("SNNotMatch"));
                }
            }
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog {
                FileName = "DeviceInformation.txt",
                Filter = "Text File | *.txt"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(dialog.OpenFile());
                writer.WriteLine(this.txt_Key.Text);
                writer.Dispose();
                writer.Close();
            }
            dialog.Dispose();
        }

        private void btn_Import_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog {
                Filter = "Text File | *.txt"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = new StreamReader(dialog.OpenFile());
                this.txt_Authentication.Text = reader.ReadToEnd().Trim();
                reader.Dispose();
                reader.Close();
            }
            dialog.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                DeviceInformation information = new DeviceInformation();
                string str = information.cpuId();
                string str2 = information.UUID().Replace("-", "").Trim();
                this.txt_UUID.Text = str2;
                this.txt_CPU.Text = str;
                this.txt_BIOS.Text = information.biosId();
                this.txt_MAC.Text = information.macId();
                if (((str == null) || (str2 == null)) || ((str == string.Empty) || (str2 == string.Empty)))
                {
                    MessageBox.Show(this.Rm.GetString("GetDeviceInformationError") + this.Rm.GetString("GetDeviceInformationEmpty"));
                }
                else
                {
                    byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(str + str2));
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        byte num2 = buffer[i];
                        builder.AppendFormat("{0:X2}", num2);
                        if (((i + 1) != buffer.Length) && (((i + 1) % 2) == 0))
                        {
                            builder.Append("-");
                        }
                    }
                    this.txt_Key.Text = builder.ToString();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(this.Rm.GetString("GetDeviceInformationError") + exception.Message);
            }
        }

        private string GetSN(byte[] PBLOCK, bool bSetRegistryRunningVersion)
        {
            string str = string.Empty;
            switch (Convert.ToInt32(PBLOCK[PBLOCK.Length - 1]))
            {
                case 1:
                    if (Convert.ToInt32(PBLOCK[5]) != 1)
                    {
                        if (Convert.ToInt32(PBLOCK[5]) == 2)
                        {
                            str = str + this.Rm.GetString("Enterprise");
                        }
                        else
                        {
                            str = str + this.Rm.GetString("Professional");
                        }
                        break;
                    }
                    str = str + this.Rm.GetString("Standard");
                    break;

                case 2:
                    if (Convert.ToInt32(PBLOCK[1]) != 2)
                    {
                        return (str + this.Rm.GetString("Standard"));
                    }
                    return (str + this.Rm.GetString("Professional"));

                case 3:
                    return str;

                case 4:
                {
                    object obj3 = str;
                    object obj4 = string.Concat(new object[] { obj3, this.Rm.GetString("MFP"), this.Rm.GetString("AuthorizeNumber"), Convert.ToInt32(PBLOCK[1]), "。" });
                    return string.Concat(new object[] { obj4, this.Rm.GetString("OCR"), this.Rm.GetString("AuthorizeNumber"), Convert.ToInt32(PBLOCK[2]), "。" });
                }
                default:
                    return str;
            }
            if (Convert.ToInt32(PBLOCK[4]) == 1)
            {
                str = str + this.Rm.GetString("WithSmartMonitor");
            }
            object obj2 = str;
            str = string.Concat(new object[] { obj2, this.Rm.GetString("AuthorizeNumber"), Convert.ToInt32(PBLOCK[0]), "。" });
            int nRunningVersion = (Convert.ToInt32(PBLOCK[4]) * 0x100) + Convert.ToInt32(PBLOCK[5]);
            if (bSetRegistryRunningVersion)
            {
                this.SetRegistryRunningVersion(nRunningVersion);
            }
            return str;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(Form1));
            this.gb_DeviceInformation = new GroupBox();
            this.btn_Export = new Button();
            this.txt_Key = new TextBox();
            this.lbl_Key = new Label();
            this.gb_Authentication = new GroupBox();
            this.btn_Import = new Button();
            this.btn_Authentication = new Button();
            this.txt_Authentication = new TextBox();
            this.lbl_Authentication = new Label();
            this.txt_UUID = new TextBox();
            this.lbl_UUID = new Label();
            this.txt_BIOS = new TextBox();
            this.lbl_BIOS = new Label();
            this.txt_MAC = new TextBox();
            this.lbl_MAC = new Label();
            this.txt_CPU = new TextBox();
            this.lbl_CPU = new Label();
            this.gb_DeviceInformation.SuspendLayout();
            this.gb_Authentication.SuspendLayout();
            base.SuspendLayout();
            this.gb_DeviceInformation.Controls.Add(this.txt_UUID);
            this.gb_DeviceInformation.Controls.Add(this.lbl_UUID);
            this.gb_DeviceInformation.Controls.Add(this.txt_BIOS);
            this.gb_DeviceInformation.Controls.Add(this.lbl_BIOS);
            this.gb_DeviceInformation.Controls.Add(this.txt_MAC);
            this.gb_DeviceInformation.Controls.Add(this.lbl_MAC);
            this.gb_DeviceInformation.Controls.Add(this.txt_CPU);
            this.gb_DeviceInformation.Controls.Add(this.lbl_CPU);
            this.gb_DeviceInformation.Controls.Add(this.btn_Export);
            this.gb_DeviceInformation.Controls.Add(this.txt_Key);
            this.gb_DeviceInformation.Controls.Add(this.lbl_Key);
            manager.ApplyResources(this.gb_DeviceInformation, "gb_DeviceInformation");
            this.gb_DeviceInformation.Name = "gb_DeviceInformation";
            this.gb_DeviceInformation.TabStop = false;
            manager.ApplyResources(this.btn_Export, "btn_Export");
            this.btn_Export.Name = "btn_Export";
            this.btn_Export.UseVisualStyleBackColor = true;
            this.btn_Export.Click += new EventHandler(this.btn_Export_Click);
            manager.ApplyResources(this.txt_Key, "txt_Key");
            this.txt_Key.Name = "txt_Key";
            this.txt_Key.ReadOnly = true;
            manager.ApplyResources(this.lbl_Key, "lbl_Key");
            this.lbl_Key.Name = "lbl_Key";
            this.gb_Authentication.Controls.Add(this.btn_Import);
            this.gb_Authentication.Controls.Add(this.btn_Authentication);
            this.gb_Authentication.Controls.Add(this.txt_Authentication);
            this.gb_Authentication.Controls.Add(this.lbl_Authentication);
            manager.ApplyResources(this.gb_Authentication, "gb_Authentication");
            this.gb_Authentication.Name = "gb_Authentication";
            this.gb_Authentication.TabStop = false;
            manager.ApplyResources(this.btn_Import, "btn_Import");
            this.btn_Import.Name = "btn_Import";
            this.btn_Import.UseVisualStyleBackColor = true;
            this.btn_Import.Click += new EventHandler(this.btn_Import_Click);
            manager.ApplyResources(this.btn_Authentication, "btn_Authentication");
            this.btn_Authentication.Name = "btn_Authentication";
            this.btn_Authentication.UseVisualStyleBackColor = true;
            this.btn_Authentication.Click += new EventHandler(this.btn_Authentication_Click);
            manager.ApplyResources(this.txt_Authentication, "txt_Authentication");
            this.txt_Authentication.Name = "txt_Authentication";
            manager.ApplyResources(this.lbl_Authentication, "lbl_Authentication");
            this.lbl_Authentication.Name = "lbl_Authentication";
            manager.ApplyResources(this.txt_UUID, "txt_UUID");
            this.txt_UUID.Name = "txt_UUID";
            this.txt_UUID.ReadOnly = true;
            manager.ApplyResources(this.lbl_UUID, "lbl_UUID");
            this.lbl_UUID.Name = "lbl_UUID";
            manager.ApplyResources(this.txt_BIOS, "txt_BIOS");
            this.txt_BIOS.Name = "txt_BIOS";
            this.txt_BIOS.ReadOnly = true;
            manager.ApplyResources(this.lbl_BIOS, "lbl_BIOS");
            this.lbl_BIOS.Name = "lbl_BIOS";
            manager.ApplyResources(this.txt_MAC, "txt_MAC");
            this.txt_MAC.Name = "txt_MAC";
            this.txt_MAC.ReadOnly = true;
            manager.ApplyResources(this.lbl_MAC, "lbl_MAC");
            this.lbl_MAC.Name = "lbl_MAC";
            manager.ApplyResources(this.txt_CPU, "txt_CPU");
            this.txt_CPU.Name = "txt_CPU";
            this.txt_CPU.ReadOnly = true;
            manager.ApplyResources(this.lbl_CPU, "lbl_CPU");
            this.lbl_CPU.Name = "lbl_CPU";
            manager.ApplyResources(this, "$this");
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.gb_Authentication);
            base.Controls.Add(this.gb_DeviceInformation);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.MaximizeBox = false;
            base.Name = "Form1";
            base.TopMost = true;
            base.Load += new EventHandler(this.Form1_Load);
            this.gb_DeviceInformation.ResumeLayout(false);
            this.gb_DeviceInformation.PerformLayout();
            this.gb_Authentication.ResumeLayout(false);
            this.gb_Authentication.PerformLayout();
            base.ResumeLayout(false);
        }

        private void SetRegistryRunningVersion(int nRunningVersion)
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
                key.CreateSubKey("AURORA");
                key = key.OpenSubKey("AURORA", true);
                key.CreateSubKey("MagicShow");
                key.OpenSubKey("MagicShow", true).SetValue("RunningVersion", nRunningVersion, RegistryValueKind.DWord);
            }
            catch
            {
            }
        }
    }
}

