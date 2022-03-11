using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class JukeboxClient : Form
{

    private Panel welcomePanel;
    private Panel roomPanel;
    private StreamAudio jc;
    private TcpClient client;
    private NetworkStream networkStream;
    private MemoryStream responseStream;
    private System.Timers.Timer timer;

    public JukeboxClient()
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
        roomPanel = new roomPanel(new EventHandler(leaveRoom), new EventHandler(play));
        roomPanel.Size = Size;
        Controls.Add(welcomePanel);
    }

    private void initializeConnection()
    {
        try
        {
            client = new TcpClient("127.0.0.1", 1302);
            networkStream = client.GetStream();

            byte[] temp = new byte[1960595];
            networkStream.Read(temp, 0, temp.Length);
            responseStream = new MemoryStream(temp);
            jc = new StreamAudio(responseStream);

            // timer for updating audio status
            timer = new System.Timers.Timer(25);
            timer.Elapsed += timer_Tick;
            timer.Enabled = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void enterRoom(object sender, EventArgs e)
    {
        initializeConnection();
        Controls.Remove(welcomePanel);
        Controls.Add(roomPanel);
    }

    private void leaveRoom(object sender, EventArgs e)
    {
        Controls.Remove(roomPanel);
        Controls.Add(welcomePanel);

        jc.Stop();

        // stream cleanup
        networkStream.Close();
        responseStream.Close();
        client.Close();
    }

    private void play(object sender, EventArgs e)
    {
        jc.StartBuffering(); 
    }

    public void timer_Tick(object sender, System.Timers.ElapsedEventArgs e)
    {
        jc.UpdateAudioState();
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new JukeboxClient());
    }

}

