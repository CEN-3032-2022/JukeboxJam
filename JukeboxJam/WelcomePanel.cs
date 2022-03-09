using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

public class welcomePanel : Panel
{

    Button enterRoomButton;
    EventHandler enterRoomEvent;

    public welcomePanel(EventHandler enterRoomFunction)
    {
        enterRoomEvent = enterRoomFunction;
        initializeComponents();
    }

    private void initializeComponents()
    {
        buildButton();
        Controls.Add(enterRoomButton);
    }

    private void buildButton()
    {
        enterRoomButton = new Button();
        enterRoomButton.Text = "Enter Room";
        enterRoomButton.Size = new Size(150, 30);
        enterRoomButton.Click += enterRoomEvent;
        enterRoomButton.Location = new System.Drawing.Point(75, 120);
    }
}
