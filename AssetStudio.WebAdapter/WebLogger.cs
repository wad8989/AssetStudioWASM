using System;
using AssetStudio;

namespace AssetStudio_WebAdaptor
{
    class WebLogger : ILogger
    {
        public void Log(LoggerEvent loggerEvent, string message, bool ignoreLevel = false)
        {
            Console.WriteLine($"<{loggerEvent.ToString()}>\n{message}");
        }
    }
}