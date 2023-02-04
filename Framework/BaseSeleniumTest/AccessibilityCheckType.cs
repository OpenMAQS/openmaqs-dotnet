//--------------------------------------------------
// <copyright file="AccessibilityCheckType.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Accessibility check types</summary>
//--------------------------------------------------
namespace OpenMaqs.BaseSeleniumTest
{
    /// <summary>
    /// Known browser types
    /// </summary>
    public enum AccessibilityCheckType
    {
        /// <summary>
        /// Check for violations
        /// </summary>
        Violations,

        /// <summary>
        /// Check for passing
        /// </summary>
        Passes,

        /// <summary>
        /// Check for inapplicable
        /// </summary>
        Inapplicable,

        /// <summary>
        /// Check for incomplete
        /// </summary>
        Incomplete
    }
}
