//--------------------------------------------------
// <copyright file="ConfigSection.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Configuration sections</summary>
//--------------------------------------------------

using System;

namespace OpenMaqs.Utilities.Helper
{
    /// <summary>
    /// The configuration sections
    /// </summary>
    public enum ConfigSection
    {
        /// <summary>
        /// General MAQS section
        /// </summary>
        [Obsolete("This section has been replaced with 'GlobalMaqs'.")]
        MagenicMaqs,

        /// <summary>
        /// General MAQS section
        /// </summary>
        GlobalMaqs,

        /// <summary>
        /// Appium section
        /// </summary>
        AppiumMaqs,

        /// <summary>
        /// Appium capabilities section
        /// </summary>
        AppiumCapsMaqs,

        /// <summary>
        /// Database section
        /// </summary>
        DatabaseMaqs,

        /// <summary>
        /// Email section
        /// </summary>
        EmailMaqs,

        /// <summary>
        /// Selenium section
        /// </summary>
        SeleniumMaqs,

        /// <summary>
        /// Selenium remote capabilities section
        /// </summary>
        RemoteSeleniumCapsMaqs,

        /// <summary>
        /// Playwright section
        /// </summary>
        PlaywrightMaqs,

        /// <summary>
        /// Web service section
        /// </summary>
        WebServiceMaqs,

        /// <summary>
        /// Mongo DB Section
        /// </summary>
        MongoMaqs
    }
}