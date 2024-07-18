using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using ProjectA.Models;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Threading;
using ProjectA.VIEW;
using ProjectA;
using System.Timers;

namespace ProjectA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        
        List<string> Songs = new List<string>();
        
        Player Player;

        addplaylist addplaylist;
        DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
       
            Player=new Player();

            addplaylist = new addplaylist();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;



        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Player.player.Source != null && Player.player.NaturalDuration.HasTimeSpan)
            {
                TimeSpan currentPosition = Player.player.Position;
                TimeSpan totalDuration = Player.player.NaturalDuration.TimeSpan;

                timespan.Content = String.Format("{0:mm\\:ss}", currentPosition);

                // Calculate remaining time
                TimeSpan remainingTime = totalDuration - currentPosition;
                lefttime.Content = String.Format("-{0:mm\\:ss}", remainingTime);
            }
            else
            {
                // Handle case when no file is selected or playing
                timespan.Content = "No file selected...";
                lefttime.Content = "";
            }
        }

        private void pause_click(object sender, RoutedEventArgs e)
        {

            playbtn.Visibility = Visibility.Visible;
            pausebtn.Visibility = Visibility.Hidden;
            Player.Pause();    
        }
        private void play_click(object sender, RoutedEventArgs e)
        {
            playbtn.Visibility = Visibility.Hidden;
            pausebtn.Visibility = Visibility.Visible;
            Player.Play();
            
        }
      
      
        private void addnewbtn_click(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            openFileDialog.Multiselect=true;
            if (openFileDialog.ShowDialog() == true)
            {

                
                string file_to_play = openFileDialog.FileName;
                this.Songs.Add(file_to_play);
                
                Metadata data = Metadata.ExtractMetadata(file_to_play);
                MessageBox.Show($"{file_to_play}");
                MessageBox.Show($"{data.Artist}");
                MessageBox.Show($"{data.Song_n}");
                MessageBox.Show($"{data.Duration}");



            }

        }
      

       
      
        private async void homebtn_click(object sender, RoutedEventArgs e)
        {
           
                
              await Player.Play_Playlist(this.Songs);
            
        }

        private void nextclick(object sender, RoutedEventArgs e)
        {
          
        }

        private void unloop_click(object sender, RoutedEventArgs e)
        {
            unloop.Visibility = Visibility.Hidden;
            loop.Visibility = Visibility.Visible;
            Player.UnLoop();
        }

        private void loop_click(object sender, RoutedEventArgs e)
        {
            loop.Visibility = Visibility.Hidden;
            unloop.Visibility = Visibility.Visible;
            Player.Loop();
        }

        private void shuffle_click(object sender, RoutedEventArgs e)
        {

            shuffle1.Visibility = Visibility.Hidden;
            unshuffle.Visibility = Visibility.Visible;
            Player.shuffle();
        }

      
        private void unshuffle_click(object sender, RoutedEventArgs e)
        {

            unshuffle.Visibility = Visibility.Hidden;
            shuffle1.Visibility = Visibility.Visible;
            Player.unshuffle();
        }

        private void addnewplaylist_click(object sender, RoutedEventArgs e)
        {
            addplaylist addplaylist = new addplaylist(); 
                gridbuttons.Children.Add(addplaylist);
              addplaylist.VerticalAlignment= VerticalAlignment.Top;
                addplaylist.HorizontalAlignment= HorizontalAlignment.Center;
            addplaylist.Height = 117;
            addplaylist.Width = 200;
            addplaylist.Margin = new Thickness(10, 163, 0, 0);
        }
    }
}
public class Metadata 
{
    public string Song_n { get; private set; } 

    public string Artist { get; private set; }
    public TimeSpan Duration { get; private set; }

    public Metadata()
    {
       

    }

    public static Metadata ExtractMetadata(string filePath)
    {
        try
        {
            var file = TagLib.File.Create(filePath);

            return new Metadata
            {
                Artist = file.Tag.FirstPerformer,
                Song_n = file.Tag.Title,
                Duration = file.Properties.Duration
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error extracting metadata: {ex.Message}");
            return null; // or throw an exception, log the error, etc.
        }
    }
   
}