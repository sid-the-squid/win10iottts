using System;
using System.Threading.Tasks;

namespace IoT.Audio.Controllers
{
    internal class PlayController : Controller
    {
        private readonly PlayService _playService;

        public PlayController(PlayService playService)
        {
            _playService =  playService;
        }

        public async Task<WebServerResponse> Get(string action, string path)
        {
            string text = "";
            try
            {
                if (action == "play")
                {
                    await _playService.PlayMedia(path);
                    text = "playing track " + path;
                }
                else if (action == "stop")
                {
                    await _playService.Stop();
                    text = "stopping track";
                }


            }
            catch (Exception ex)
            {
                return WebServerResponse.CreateError<string>("Error: " + ex.ToString());
            }
            return WebServerResponse.CreateOk("OK: "+text);
        }

    }
}
