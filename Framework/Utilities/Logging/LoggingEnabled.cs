//--------------------------------------------------
// <copyright file="LoggingEnabled.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>When to enable logging enumeration</summary>
//--------------------------------------------------

namespace OpenMAQS.Maqs.Utilities.Logging
{
    /// <summary>
    /// The type of message
    /// </summary>
    public enum LoggingEnabled
    {
        /// <summary>
        /// Yes log
        /// </summary>
        YES = 0,

        /// <summary>
        /// Only save a log when there is a failure
        /// </summary>
        ONFAIL = 1,

        /// <summary>
        /// No, don't log
        /// </summary>
        NO = 2,
    }
}
