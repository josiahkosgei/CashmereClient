
// Type: Cashmere.Library.Standard.Logging.CashmereWebLogger


using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cashmere.Library.Standard.Logging
{
    public class CashmereWebLogger : ICashmereWebLogger
    {
        private static ILogger _logger;

        private string DateTimeFormat { get; set; }

        public CashmereWebLogger(string version, string name, IConfiguration configuration)
        {
            _logger = LogManager.GetLogger(name);
            DateTimeFormat = configuration?["Logging.DateTimeFormat"] ?? "yyyy-MM-dd HH:mm:ss.fff";
            Info("CashmereWebLogger.Constructor", "Initialising", "Init", "Initialising logger for type [{0}] version {1}", name, new object[1]
            {
        version
            });
        }

        public void Trace(
          string user,
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects)
        {
            if (!_logger.IsTraceEnabled)
                return;
            _logger.Trace(string.Format("\x0002{0:5}|{1}|{2}|{3}|{4}|{5}|{6}\x0003", LogLevel.Trace, DateTime.Now.ToString(DateTimeFormat), Component, EventName, EventType, user, MessageFormatObjects == null || MessageFormatObjects.Count() <= 0 ? Message : (object)string.Format(Message, MessageFormatObjects)));
        }

        public void Debug(
          string user,
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects)
        {
            if (!_logger.IsDebugEnabled)
                return;
            _logger.Debug(string.Format("\x0002{0:5}|{1}|{2}|{3}|{4}|{5}|{6}\x0003", LogLevel.Debug, DateTime.Now.ToString(DateTimeFormat), Component, EventName, EventType, user, MessageFormatObjects == null || MessageFormatObjects.Count() <= 0 ? Message : (object)string.Format(Message, MessageFormatObjects)));
        }

        public void Info(
          string user,
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects)
        {
            if (!_logger.IsInfoEnabled)
                return;
            _logger.Info(string.Format("\x0002{0:5}|{1}|{2}|{3}|{4}|{5}|{6}\x0003", LogLevel.Info, DateTime.Now.ToString(DateTimeFormat), Component, EventName, EventType, user, MessageFormatObjects == null || MessageFormatObjects.Count() <= 0 ? Message : (object)string.Format(Message, MessageFormatObjects)));
        }

        public void Warning(
          string user,
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects)
        {
            if (!_logger.IsWarnEnabled)
                return;
            _logger.Warn(string.Format("\x0002{0:5}|{1}|{2}|{3}|{4}|{5}|{6}\x0003", LogLevel.Warn, DateTime.Now.ToString(DateTimeFormat), Component, EventName, EventType, user, MessageFormatObjects == null || MessageFormatObjects.Count() <= 0 ? Message : (object)string.Format(Message, MessageFormatObjects)));
        }

        public void Error(
          string user,
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects)
        {
            if (!_logger.IsErrorEnabled)
                return;
            _logger.Error(string.Format("\x0002{0:5}|{1}|{2}|{3}|{4}|{5}|{6}\x0003", LogLevel.Error, DateTime.Now.ToString(DateTimeFormat), Component, EventName, EventType, user, MessageFormatObjects == null || MessageFormatObjects.Count() <= 0 ? Message : (object)string.Format(Message, MessageFormatObjects)));
        }

        public void Fatal(
          string user,
          string Component,
          string EventName,
          string EventType,
          string Message,
          params object[] MessageFormatObjects)
        {
            if (!_logger.IsFatalEnabled)
                return;
            _logger.Fatal(string.Format("\x0002{0:5}|{1}|{2}|{3}|{4}|{5}|{6}\x0003", LogLevel.Fatal, DateTime.Now.ToString(DateTimeFormat), Component, EventName, EventType, user, MessageFormatObjects == null || MessageFormatObjects.Count() <= 0 ? Message : (object)string.Format(Message, MessageFormatObjects)));
        }
    }
}
