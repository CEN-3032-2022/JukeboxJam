using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Jukebox_Jam_Starting_GUI
{
    public class initialForm : Form
    {

        private Panel welcomePanel;
        private Panel roomPanel;

        public initialForm()
        {
            initializeComponents();
        }


        private void initializeComponents()
        {
            Text = "Jukebox Jam";
            ClientSize = new Size(300, 300);
            CenterToScreen();

            welcomePanel = new welcomePanel(new EventHandler(enterRoom));
            welcomePanel.Size = Size;
            roomPanel = new roomPanel(new EventHandler(leaveRoom));
            roomPanel.Size = Size;
            Controls.Add(welcomePanel);
        }

        private void enterRoom(object sender, EventArgs e)
        {
            Controls.Remove(welcomePanel);
            Controls.Add(roomPanel);
        }

        private void leaveRoom(object sender, EventArgs e)
        {
            Controls.Remove(roomPanel);
            Controls.Add(welcomePanel);
        }

        [STAThread]
        static void Main()
        { 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new initialForm());
        }
    }
}
