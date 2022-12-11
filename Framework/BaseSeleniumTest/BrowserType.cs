//--------------------------------------------------
// <copyright file="BrowserType.cs" company="MAQS">
//  Copyright 2022 MAQS, All rights Reserved
// </copyright>
// <summary>Known browser types</summary>
//--------------------------------------------------
namespace Maqs.BaseSeleniumTest
{
    /// <summary>
    /// Known browser types
    /// </summary>
    public enum BrowserType
    {
        /// <summary>
        /// Chrome web browser
        /// </summary>
        Chrome,

        /// <summary>
        /// Edge web browser
        /// </summary>
        Edge,

        /// <summary>
        /// Firefox web browser
        /// </summary>
        Firefox,

        /// <summary>
        /// Chrome web browser - run headless
        /// </summary>
        HeadlessChrome,

        /// <summary>
        /// IE web browser
        /// </summary>
        IE,

        /// <summary>
        /// Remote web browser - Used when executing on Grid or cloud based provides like Sauce Labs
        /// </summary>
        Remote
    }
}
