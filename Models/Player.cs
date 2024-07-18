using ProjectA.Models;
using System;
using System.Collections.Generic;
using System.Security.RightsManagement;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using TagLib.Ape;

namespace ProjectA.Models
{
    internal class Player 
    {
        private List<string> Song_fileslist { get; set; }
        public MediaPlayer player { get; set; }
        private List<string> unshuffled_list { get; set; }
        private string current_file { get; set; }

     
       

        public Player()
        {
            Song_fileslist = new List<string>();
            player = new MediaPlayer();
           
        }
        
       
        public async Task Play_Playlist(List<string> songsfiles)
        {
            
            this.Song_fileslist.Clear();
            this.Song_fileslist = songsfiles;
            foreach (var item in Song_fileslist)
            {
                await Play_Song(item);
            }
        }
     
        private async Task Play_Song(string song_file)
        {
           
            try
            {
                player.Open(new Uri(song_file, UriKind.RelativeOrAbsolute));
                Metadata.ExtractMetadata(song_file);
               
                this.current_file = song_file;
                player.Play();

                var songCompletionTask = new TaskCompletionSource<object>();
                player.MediaEnded += (sender, e) => songCompletionTask.SetResult(null);

             
                await songCompletionTask.Task;

                player.Stop();
                this.current_file = null;
            
                MessageBox.Show("Song has completed." +$"{this.current_file}");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

       
       
        public void Play()
        {
            player.Play();
           
        }

        public void Pause()
        {
            player.Pause();
            
        }

        public void Stop()
        {
            player.Stop();
        }
        public async void Loop()
        {
            if (this.current_file != null)
            {
                mediaEnded = false;
                while (!mediaEnded)
                {
                    await Play_Song(this.current_file.ToString());
                }
            }
        }

        public void UnLoop()
        {
            mediaEnded = true;
        }

        private bool mediaEnded = false;
        public void shuffle()
        {
            this.Song_fileslist = this.unshuffled_list;
            Random random=new  Random();
            if (this.unshuffled_list!=null)
            {
                for (int i=this.Song_fileslist.Count-1; i>0 ;i--)
                {
                    
                    int random_index = random.Next(i + 1);
                    string temp = this.Song_fileslist[i];
                    this.Song_fileslist[i] = Song_fileslist[random_index];
                    Song_fileslist[random_index]=temp;
                   
                }

            }
            

        }
        public void unshuffle()
        {
            if (this.unshuffled_list!=null)
            {
                this.Song_fileslist = this.unshuffled_list;
            }

        }

    }
}
