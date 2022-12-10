//--------------------------------------------------
// <copyright file="IFileLogger.cs" company="MAQS">
//  Copyright 2022 MAQS, All rights Reserved
// </copyright>
// <summary>File logger interface</summary>
//--------------------------------------------------
namespace Maqs.Utilities.Logging
{
    /// <summary>
    /// Interface for file logger
    /// </summary>
    public interface IFileLogger : ILogger
    {
        /// <summary>
        /// Gets or sets path to the log file
        /// </summary>
        string FilePath { get; set; }
    }
}