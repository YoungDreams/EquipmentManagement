using System;

namespace Foundation.Core
{
    public static class Logger
    {
        public static NLog.Logger Log(this object obj)
        {
            return NLog.LogManager.GetCurrentClassLogger(obj.GetType());
        }

        public static NLog.Logger Log()
        {
            return NLog.LogManager.GetCurrentClassLogger();
        }

        public static void Error(string message, Exception ex)
        {
            NLog.LogManager.GetCurrentClassLogger().Error(ex, message);
        }
    }
}
