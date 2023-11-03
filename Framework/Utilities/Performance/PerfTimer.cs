﻿//--------------------------------------------------
// <copyright file="PerfTimer.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Performance Timer Class</summary>
//--------------------------------------------------
using System;
using System.Xml.Serialization;

namespace OpenMAQS.Maqs.Utilities.Performance
{
    /// <summary>
    /// Response timer class - holds a single response timer
    /// </summary>
    public class PerfTimer
    {
        /// <summary>
        /// Gets or sets the name of the Page associated with the Timer
        /// </summary>
        public string TimerContext { get; set; }

        /// <summary>
        /// Gets or sets the Timer Name
        /// </summary>
        public string TimerName { get; set; }

        /// <summary>
        /// Gets or sets the start time
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the duration
        /// </summary>
        [XmlIgnore]
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the Serializable duration
        /// </summary>
        public long DurationTicks
        {
            get { return this.Duration.Ticks; }
            set { this.Duration = new TimeSpan(value); }
        }
    }
}
