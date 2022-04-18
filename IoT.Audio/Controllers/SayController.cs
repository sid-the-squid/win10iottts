using System.Threading.Tasks;

namespace IoT.Audio.Controllers
{
    internal class SayController : Controller
    {
        private readonly SpeechService _speechService;
        public SayController(SpeechService speechService)
        {
            _speechService = speechService;
        }

        public async Task<WebServerResponse> Get(string text)
        {

            await _speechService.SayAsync(text);
            return WebServerResponse.CreateOk("OK");
        }

    }
}
