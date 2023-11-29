﻿//--------------------------------------------------
// <copyright file="IFileLogger.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>File logger interface</summary>
//--------------------------------------------------
namespace OpenMAQS.Maqs.Utilities.Logging
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