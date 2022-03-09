using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Timers;



public class JukeboxClient : Form
{

    private Panel welcomePanel;
    private Panel roomPanel;
    private StreamAudio jc;
    private System.Timers.Timer timer;

    public JukeboxClient()
    {
        initializeComponents();

        try
        {
            TcpClient client = new TcpClient("127.0.0.1", 1302);
            NetworkStream stream = client.GetStream();

            byte[] temp = new byte[1960595];
            stream.Read(temp, 0, temp.Length);
            Stream responseStream = new MemoryStream(temp);
            jc = new StreamAudio(responseStream);

            stream.Close();
            client.Close();
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


    private void initializeComponents()
    {
        Text = "Jukebox Jam";
        ClientSize = new Size(300, 300);
        CenterToScreen();

        // create a timer
        timer = new System.Timers.Timer(25);

        welcomePanel = new welcomePanel(new EventHandler(enterRoom));
        welcomePanel.Size = Size;
        roomPanel = new roomPanel(new EventHandler(leaveRoom), new EventHandler(play));
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

    private void play(object sender, EventArgs e)
    {
        jc.buttonPlay_Click();
        timer.Elapsed += jc.timer1_Tick;
        timer.Enabled = true;
    }

    private static void testAudio(Stream stream)
    {
        //Console.Write("Testing audio...");
        StreamAudio jc = new StreamAudio(stream);
        //jc.buttonPlay_Click();
        //jc.timer1_Tick();
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new JukeboxClient());
    }

}

