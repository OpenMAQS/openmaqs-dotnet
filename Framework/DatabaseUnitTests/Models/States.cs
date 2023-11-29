﻿//--------------------------------------------------
// <copyright file="States.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Model representing States table</summary>
//--------------------------------------------------

namespace DatabaseUnitTests.Models
{
    /// <summary>
    /// Class representing the states table.
    /// </summary>
    public class States
    {
        /// <summary>
        /// Gets or sets the state id.
        /// </summary>
        public int StateID { get; set; }

        /// <summary>
        /// Gets or sets the state name.
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// Gets or sets the state abbreviation.
        /// </summary>
        public string StateAbbreviation { get; set; }
    }
}
