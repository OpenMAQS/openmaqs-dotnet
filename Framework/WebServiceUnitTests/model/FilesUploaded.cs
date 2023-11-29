//--------------------------------------------------
// <copyright file="FilesUploaded.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>FilesUploaded model</summary>
//--------------------------------------------------
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace WebServiceTesterUnitTesting.Model
{
    /// <summary>
    /// Employee Model
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FilesUploaded
    {
        /// <summary>
        /// Gets or sets the list of files uploaded
        /// </summary>
        public virtual List<Files> Files { get; set; }
    }
}