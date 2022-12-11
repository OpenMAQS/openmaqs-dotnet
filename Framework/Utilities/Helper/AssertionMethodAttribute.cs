//--------------------------------------------------
// <copyright file="SoftAssert.cs" company="MAQS">
//  Copyright 2022 MAQS, All rights Reserved
// </copyright>
// <summary>This is the SoftAssert class</summary>
//--------------------------------------------------
using System;

namespace Maqs.Utilities.Helper
{
    /// <summary>
    /// SonarLink 2699 Tests should include assertions
    /// Used for SoftAsserts
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class AssertionMethodAttribute : Attribute
    {
    }
}