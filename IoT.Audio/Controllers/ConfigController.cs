using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT.Audio.Controllers
{
    internal class ConfigController : Controller
    {

        private readonly SpeechService _speechService;
        public ConfigController(SpeechService speechService)
        {
            _speechService = speechService;
        }
        public async Task<WebServerResponse> Get(string action, string value)
        {
            string text = "action not found";
            if (action == "clock")
            {
                text = clock(value);
            }
            else if (action =="voices")
            {
                text = voices(value);
            }
            else if (action =="voice")
            {
                text = await SetVoice(value);
            }
            return WebServerResponse.CreateOk("OK,"+text);
        }

        private string clock(string action)
        {
            if (action == "off")
            {
                _speechService.HourlyAnnounce = false;
                return "clock is now off";
            }
            else
            {
                _speechService.HourlyAnnounce=true;
            }
            return "clock is now on";
        }

        private async Task<string> SetVoice(string value)
        {
            await _speechService.SetVoice(value);
            return "Done";
        }
        private string voices (string action)
        {
            return _speechService.ListVoices();
        }

    }
}
