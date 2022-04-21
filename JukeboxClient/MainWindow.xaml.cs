﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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


        public MainWindow()
        {
            FixMenuAlignment();
            InitializeComponent();
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
                + "2. Click the next button to start the playlist queue\n"
                + "3. Click the play button to start the music\n"
                + "4. If you wish to pause the music, click the play button to toggle playing audio\n"
                + "5. If you wish to switch between songs, click the forward or backward buttons\n"
                + "6. If you wish to quit, either go to the file menu and click \"Exit\", or click the close window button.";
            MessageBox.Show(message, "Help", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {    
            music.streamSong(SongPlayer);
        }


        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            music.incrementSong();
            SongPlayer = music.loadSong(SongPlayer);
        }

        private void Reverse_Click(object sender, RoutedEventArgs e)
        {

            music.decrementSong();
            SongPlayer = music.loadSong(SongPlayer);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Network.GetPlaylist();
            Network.GetSongs();
        }

        private void PlaylistGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine(PlaylistGrid.SelectedIndex);
        }
    }
}