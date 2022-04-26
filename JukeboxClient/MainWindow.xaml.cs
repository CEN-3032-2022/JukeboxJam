using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JukeboxClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MusicPlayer music = new MusicPlayer();
        private int songIndex = 0;
        System.IO.DirectoryInfo di = null;
        bool songsWereChanged = true;

        public MainWindow()
        {
            FixMenuAlignment();
            InitializeComponent();
            string mydir = Directory.GetCurrentDirectory();
            mydir = mydir.Replace(@"bin\Debug\net6.0-windows", "music");

            if (!Directory.Exists(mydir))
            {
                Directory.CreateDirectory(mydir);
            }

            di = new DirectoryInfo(mydir);
            
        }

        /**
         * For some very odd reason, WPF menus are
         * aligned awkardly to the left of the
         * main window. This makes them behave
         * normally.
         */
        private void FixMenuAlignment()
        {
            var ifLeft = SystemParameters.MenuDropAlignment;
            if (ifLeft)
            {
                // change to false
                var t = typeof(SystemParameters);
                var field = t.GetField("_menuDropAlignment", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                field.SetValue(null, false);
                ifLeft = SystemParameters.MenuDropAlignment;
            }
        }

        private void JamSessionMenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            JamSessionWindow jamWindow = new JamSessionWindow();
            jamWindow.Show();
        }

        private void JamSessionMenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void HelpClick(object sender, RoutedEventArgs e)
        {
            string message = "1. Click the \"load songs\" button\n"
                + "2. Click the play button to start the music\n"
                + "3. If you wish to pause the music, click the play button to toggle playing audio\n"
                + "4. If you wish to switch between songs, click the forward or backward buttons\n"
                + "5. If you wish to quit, either go to the file menu and click \"Exit\", or click the close window button.";
            MessageBox.Show(message, "Help", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (music.isLoaded(SongPlayer) && songsWereChanged == false)
            {
                music.streamSong(SongPlayer);
            }
            else if (0 == di.GetFiles().Length)
            {
                AppData.playlist.Clear();
                MessageBox.Show("No files in music folder!");
            }
            else
            {
                music.incrementSong();
                SongPlayer = music.loadSong(SongPlayer);
                music.decrementSong();
                SongPlayer = music.loadSong(SongPlayer);
                music.streamSong(SongPlayer);
                songsWereChanged = false;
                UpdatePlaylistSelection();
            }

            SwapPlayPause();
        }


        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            if (0 == di.GetFiles().Length)
            {
                AppData.playlist.Clear();
                MessageBox.Show("no files in music");
            }
            else
            {
                music.incrementSong();
                SongPlayer = music.loadSong(SongPlayer);
                UpdatePlaylistSelection();
            }
        }

        private void Reverse_Click(object sender, RoutedEventArgs e)
        {

            if (0 == di.GetFiles().Length)
            {
                AppData.playlist.Clear();
                MessageBox.Show("no files in music");
            }
            else
            {
                music.decrementSong();
                SongPlayer = music.loadSong(SongPlayer);
                UpdatePlaylistSelection();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            Network.GetPlaylist();
            Network.GetSongs();

            songsWereChanged = true;
        }

        private void PlaylistGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine(PlaylistGrid.SelectedIndex);
            music.setSongIndex(PlaylistGrid.SelectedIndex);
            SongPlayer = music.loadSong(SongPlayer);
        }

        private void UpdatePlaylistSelection()
        {
            PlaylistGrid.SelectedIndex = music.getSongIndex();
        }

        private void Send_Button(object sender, RoutedEventArgs e)
        {
            AppData.roomState.Position = SongPlayer.Position;
            Network.PostRoomState();
        }

        private void Recieve_Button(object sender, RoutedEventArgs e)
        {
            Network.GetRoomState();
            MessageBox.Show(AppData.roomState.Position.ToString());
        }

        private void SwapPlayPause()
        {
            string mydir = Directory.GetCurrentDirectory();
            mydir = mydir.Replace(@"bin\Debug\net6.0-windows", "");

            var brush = new ImageBrush();
            if (music.getIsPlaying())
            {
                brush.ImageSource = new BitmapImage(new Uri($"{mydir}\\pause-button.png", UriKind.Absolute));
            } 
            else
            {
                brush.ImageSource = new BitmapImage(new Uri($"{mydir}\\play-button.png", UriKind.Absolute));
            }

            Play.Background = brush;
        }
    }
}