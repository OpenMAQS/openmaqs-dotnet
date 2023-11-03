﻿//--------------------------------------------------
// <copyright file="LoggerFactory.cs" company="OpenMAQS">
//  Copyright 2022 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Logger factory</summary>
//--------------------------------------------------
using OpenMAQS.Maqs.Utilities.Helper;

namespace OpenMAQS.Maqs.Utilities.Logging
{
    /// <summary>
    /// Logger factory
    /// </summary>
    public static class LoggerFactory
    {
        /// <summary>
        /// Get a new console logger which respects current logging level
        /// </summary>
        /// <returns>A console logger</returns>
        public static ILogger GetConsoleLogger()
        {
            return new ConsoleLogger(LoggingConfig.GetLoggingLevelSetting());
        }

        /// <summary>
        /// Get a logger
        /// </summary>
        /// <param name="logName">Log name- gets added as console message or file name</param>
        /// <returns>A logger</returns>
        public static ILogger GetLogger(string logName)
        {
            if (LoggingConfig.GetLoggingEnabledSetting() == LoggingEnabled.NO)
            {
                return GetLogger(logName, "CONSOLE", MessageType.SUSPENDED);
            }

            return GetLogger(logName, Config.GetGeneralValue("LogType", "CONSOLE"), LoggingConfig.GetLoggingLevelSetting());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logName">Log name- gets added as console message or file name</param>
        /// <param name="logType">Type of log - Console, Text or HTML</param>
        /// <param name="loggingLevel">Logging level</param>
        /// <returns>A logger</returns>
        public static ILogger GetLogger(string logName, string logType, MessageType loggingLevel)
        {
            string logDirectory = LoggingConfig.GetLogDirectory();

            switch (logType.ToUpper())
            {
                case "CONSOLE":
                    return new ConsoleLogger(loggingLevel);
                case "TXT":
                case "TEXT":
                    return new FileLogger(logDirectory, logName, loggingLevel);
                case "HTML":
                case "HTM":
                    return new HtmlFileLogger(logDirectory, logName, loggingLevel);
                default:
                    throw new MaqsLoggingConfigException($"Log type '{logType}' is not a valid option");
            }
        }
    }
}
