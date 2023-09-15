//--------------------------------------------------
// <copyright file="SoftAssert.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>This is the SoftAssert class</summary>
//--------------------------------------------------
using System;

namespace OpenMaqs.Utilities.Helper
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