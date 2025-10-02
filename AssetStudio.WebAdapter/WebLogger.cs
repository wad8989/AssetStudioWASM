using System;
using AssetStudio;

namespace AssetStudio_WebAdaptor
{
    class WebLogger : ILogger
    {
        public void Log(LoggerEvent loggerEvent, string message, bool ignoreLevel = false)
        {
            #if !DEBUG
            if (loggerEvent == LoggerEvent.Debug) return;
            #endif
            Console.WriteLine($"<{loggerEvent.ToString()}>\n{message}");
        }
    }
}