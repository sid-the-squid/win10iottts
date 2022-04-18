using System;
using System.Threading.Tasks;
using Microsoft.Maker.Media.UniversalMediaEngine;

namespace IoT.Audio
{
    internal class PlayService
    {

        private MediaEngine mediaEngine;


        public PlayService()
        {
            mediaEngine = new MediaEngine();
        }

        public async Task Init()
        {
            var result = await this.mediaEngine.InitializeAsync();
            if (result == MediaEngineInitializationResult.Fail)
            {
                throw new Exception("failed to init player");
            }
        }
    
        public async Task PlayMedia(string path)
        {
            if (IsQuietHoursTime()) return;
            mediaEngine.Play(path);
        }

        public async Task Stop()
        {
            mediaEngine.Pause();
        }
       

        public bool IsQuietHoursTime()
        {
            var now = DateTime.Now;
            //TODO: quiet hours set dynamically
            return now.Hour < 9 || now.Hour > 21;

        }

        
    }
}