using System;
using System.Threading;
using Windows.ApplicationModel.Background;
using Microsoft.Extensions.DependencyInjection;

namespace IoT.Audio
{
    public sealed class StartupTask : IBackgroundTask
    {
        private BackgroundTaskDeferral deferral;
        private Timer clockTimer;
        private SpeechService speechService;
        private Webserver webServer;
        private PlayService playService;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            deferral = taskInstance.GetDeferral();

            var servicesBuilder = new ServiceCollection()
                .AddSingleton<SpeechService>()
                .AddSingleton<PlayService>()
                ;
            Services = servicesBuilder.BuildServiceProvider();

            speechService = Services.GetService<SpeechService>();

            playService = Services.GetService<PlayService>();
            await playService.Init();

            var timeToFullHour = GetTimeSpanToNextFullHour();
            clockTimer = new Timer(OnClock, null, timeToFullHour, TimeSpan.FromHours(1));

            webServer = new Webserver();

            await webServer.StartAsync();
            await speechService.SayTime24h();
        }

        private static TimeSpan GetTimeSpanToNextFullHour()
        {
            var now = DateTime.Now;
            var nextHour = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0).AddHours(1);
            return nextHour - now;
        }

        private async void OnClock(object state)
        {
            await speechService.SayTime24h();
        }

        internal static IServiceProvider Services { get; private set; }
    }
}
