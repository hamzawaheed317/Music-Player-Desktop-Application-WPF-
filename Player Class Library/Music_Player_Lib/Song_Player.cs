using System;
using System.Media;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NetCoreAudio;
namespace Music_Player_Lib
{
    public class Song_Player
    {
        Player _player;
        private string file;
        public Song_Player(string file)
        {
            this.file = file;
            _player = new Player();        
        }
        public void Play()

        {
            _player.Play(this.file);
        }
       public void Pause()
        {
            _player.Pause();
        }
    }
}
