using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 한영
{
    public partial class Form1 : Form
    {
        [DllImport("imm32.dll")]
        private static extern IntPtr ImmGetDefaultIMEWnd(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr IParam);
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        private const int WM_IME_CONTROL = 643;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e) { 
            timer1.Stop(); 
            base.OnClosed(e); 
        }


        private async void FadeOut(Form o, int interval = 80)
        {
            
            while (o.Opacity > 0.0)
            {
                await Task.Delay(interval);
                o.Opacity -= 0.05;
            }
            o.Opacity = 0;  
        }

        private void UPDATE_LABEL(string a)
        {
            label1.Text = a;

            label1.Location = new Point((this.Width - label1.Width) / 2,
                          (this.Height - label1.Height) / 2);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            this.Opacity = 0;

            this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                          (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 10 * 9);

            label1.Location = new Point((this.Width - label1.Width) / 2,
                          (this.Height - label1.Width) / 2);
            timer1.Start();
        }
        bool last = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                IntPtr hwnd = GetForegroundWindow();
                IntPtr hime = ImmGetDefaultIMEWnd(hwnd);
                IntPtr status = SendMessage(hime, WM_IME_CONTROL, new IntPtr(0x5), new IntPtr(0));

                if (status.ToInt32() != 0)
                {
                    if (last == true)
                    {
                        UPDATE_LABEL("한");
                        this.Opacity = 1;
                        FadeOut(this, 10);
                        last = false;
                    }
                }
                else {
                    if (last == false)
                    {
                        UPDATE_LABEL("영");
                        this.Opacity = 1;
                        FadeOut(this, 10);
                        last = true;
                    }
                } 
            } 
            catch 
            {} 
        }
    }
}
