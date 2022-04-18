using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Media.SpeechSynthesis;
using System.Collections.Generic;
using System.Text;
using Windows.Globalization;
using Windows.Media.SpeechRecognition;

namespace IoT.Audio
{
    internal class SpeechService
    {
        private SpeechSynthesizer speechSynthesizer;
        private readonly MediaPlayer speechPlayer;


        public SpeechService()
        {
            ChosenVoice = SpeechSynthesizer.AllVoices.FirstOrDefault(i => i.Gender == VoiceGender.Female) ?? SpeechSynthesizer.DefaultVoice;
            speechSynthesizer = CreateSpeechSynthesizer();
            speechPlayer = new MediaPlayer();
            HourlyAnnounce = true;
        }

        private static SpeechSynthesizer CreateSpeechSynthesizer()
        {
            var synthesizer = new SpeechSynthesizer();
            synthesizer.Voice = ChosenVoice;
            return synthesizer;
        }

        public async Task SetVoice(string voice)
        {
            if (string.IsNullOrEmpty(voice))
            {
                await SayAsync("Invalid voice chosen, no changes made");
                return;
            }
            try
            {
                var new_voice = SpeechSynthesizer.AllVoices.FirstOrDefault(x => x.DisplayName == voice);
                if (new_voice != null)
                {
                    ChosenVoice = new_voice;
                    speechSynthesizer = CreateSpeechSynthesizer();
                    await SayAsync("Hi I'm " + voice + " Nice to meet you, you have chosen my voice.");
                }
            }
            catch (Exception ex)
            {
                await SayAsync("Error setting voice to "+voice+", no changes made");
                return;
            }
        } 
    
        public async Task SayAsync(string text)
        {
            using (var stream = await speechSynthesizer.SynthesizeTextToStreamAsync(text))
            {
                speechPlayer.Source = MediaSource.CreateFromStream(stream, stream.ContentType);
            }
            speechPlayer.Play();
        }

        public async Task SayAsync(string text, double balance)
        {
            speechPlayer.AudioBalance = balance;
            using (var stream = await speechSynthesizer.SynthesizeTextToStreamAsync(text))
            {
                speechPlayer.Source = MediaSource.CreateFromStream(stream, stream.ContentType);
            }
            speechPlayer.Play();
        }

        public bool IsQuietHoursTime()
        {
            var now = DateTime.Now;
            //TODO: quiet hours set dynamically
            return now.Hour < 9 || now.Hour > 21;

        }

        public String ListVoices()
        {
            StringBuilder voices_string = new StringBuilder();
            foreach (var voice in SpeechSynthesizer.AllVoices)
            {
                voices_string.AppendLine($"{voice.DisplayName} ({voice.Language}), {voice.Gender}");
            }
            return voices_string.ToString();
        }


        public static VoiceInformation ChosenVoice { get; set; }

        public bool HourlyAnnounce { get; set; }

        public async Task SayTime()
        {
            if (IsQuietHoursTime()) return;
            if (! HourlyAnnounce) return;

            var now = DateTime.Now;
            var hour = now.Hour;

            string timeOfDay;
            if (hour <= 12)
            {
                timeOfDay = "morning";
            }
            else if (hour <= 17)
            {
                timeOfDay = "afternoon";
            }
            else
            {
                timeOfDay = "evening";
            }

            if (hour > 12)
            {
                hour -= 12;
            }
            if (now.Minute == 0)
            {
                await SayAsync($"Good {timeOfDay}, it's {hour} o'clock.");
            }
            else
            {
                await SayAsync($"Good {timeOfDay}, it's {hour} {now.Minute}.");
            }
        }


        public async Task SayTime24h()
        {
            if (IsQuietHoursTime()) return;
            if (!HourlyAnnounce) return;

            var now = DateTime.Now;
            var hour = now.Hour;

            string timeOfDay;
            if (hour <= 12)
            {
                timeOfDay = "morning";
            }
            else if (hour <= 17)
            {
                timeOfDay = "afternoon";
            }
            else
            {
                timeOfDay = "evening";
            }

            StringBuilder b = new StringBuilder();
            b.Append($"Good {timeOfDay}, ");
            if (hour < 10)
            {
                b.Append($"it's, zero {hour}");
            }
            else
            {
                b.Append($"it's, {hour}");
            }
            if (now.Minute <10)
            {
               
                b.Append($", zero {now.Minute}");
            }
            else
            {
                b.Append($", {now.Minute}");

            }

            await SayAsync(b.ToString());
        }
    }
}