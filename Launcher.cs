using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTAWLauncher
{
    public partial class Launcher : Form
    {
        private bool processPriority = true;
        public static bool IsAdministrator => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        public Launcher()
        {
            InitializeComponent();
            // it should not be possible to run this without admin but just incase
            if (!IsAdministrator)
            {
                Application.Exit();
            }
        }
        private void btnLaunch_Click(object sender, EventArgs e)
        {
            //string ip = "play.gta.world";
            //string port = "22005";
            //RegistryKey ragemp = Registry.CurrentUser.CreateSubKey("Software\\RAGE-MP");
            //ragemp.SetValue("launch2.ip", (object)ip);
            //ragemp.SetValue("launch2.port", (object)port);
            //// launch game
            //Process.Start("rage://v/connect?ip=play.gta.world:22005", "");
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\ragemp_v.exe", true);
            if (processPriority)
            {
                key = key.CreateSubKey("PerfOptions");
                key.SetValue("CpuPriorityClass", 3, RegistryValueKind.DWord);
            }
            else
            {
                key.DeleteSubKey("PerfOptions");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cbPriority_CheckedChanged(object sender, EventArgs e)
        {
            // priority checkbox changed
            if (cbPriority.Checked)
            {
                processPriority = true;
            }
            else
            {
                processPriority = false;
            }
        }


        // allow custom form to be dragged
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private void lblTitle_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void lblTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void lblTitle_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

    }
}
