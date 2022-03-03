using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Jukebox_Jam_Starting_GUI
{
    public class roomPanel : Panel
    {

        Label songLabel;
        Button leaveRoomButton;
        EventHandler leaveRoomEvent;

        public roomPanel(EventHandler leaveRoomFunction)
        {
            leaveRoomEvent = leaveRoomFunction;
            initializeComponents();
        }

        private void initializeComponents()
        { 
            buildButton();
            buildSongLabel();
        }

        private void buildSongLabel()
        {
            songLabel = new Label();
            songLabel.Text = "Song Name";
            songLabel.Size = new Size(150, 30);
            songLabel.Location = new System.Drawing.Point(115, 60);
            Controls.Add(songLabel);
        }

        private void buildButton()
        {
            leaveRoomButton = new Button();
            leaveRoomButton.Size = new Size(150, 30);
            leaveRoomButton.Text = "Exit Room";
            leaveRoomButton.Click += new EventHandler(leaveRoomEvent);
            leaveRoomButton.Location = new System.Drawing.Point(75, 175);
            Controls.Add(leaveRoomButton);
        }
    }
}
