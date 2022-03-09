using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

public class roomPanel : Panel
{

    Label songLabel;
    Button leaveRoomButton;
    Button playButton;
    EventHandler leaveRoomEvent;
    EventHandler playEvent;

    public roomPanel(EventHandler leaveRoomFunction, EventHandler playFunction)
    {
        leaveRoomEvent = leaveRoomFunction;
        playEvent = playFunction;
        initializeComponents();
    }

    private void initializeComponents()
    {
        buildLeaveRoomButton();
        buildPlayButton();
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

    private void buildLeaveRoomButton()
    {
        leaveRoomButton = new Button();
        leaveRoomButton.Size = new Size(150, 30);
        leaveRoomButton.Text = "Exit Room";
        leaveRoomButton.Click += new EventHandler(leaveRoomEvent);
        leaveRoomButton.Location = new System.Drawing.Point(75, 175);
        Controls.Add(leaveRoomButton);
    }

    private void buildPlayButton()
    {
        playButton = new Button();
        playButton.Size = new Size(150, 30);
        playButton.Text = "Play";
        playButton.Click += new EventHandler(playEvent);
        playButton.Location = new System.Drawing.Point(75, 125);
        Controls.Add(playButton);
    }
}
