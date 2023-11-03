﻿//--------------------------------------------------
// <copyright file="PlaywrightTestObject.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Holds Playwright context data</summary>
//--------------------------------------------------
using OpenMAQS.Maqs.BaseTest;
using OpenMAQS.Maqs.Utilities.Logging;
using Microsoft.Playwright;
using System;

namespace OpenMAQS.Maqs.BasePlaywrightTest
{
    /// <summary>
    /// Playwright test context data
    /// </summary>
    public class PlaywrightTestObject : BaseTestObject, IPlaywrightTestObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaywrightTestObject" /> class
        /// </summary>
        /// <param name="PageDriver">The test's Playwright page</param>
        /// <param name="logger">The test's logger</param>
        /// <param name="fullyQualifiedTestName">The test's fully qualified test name</param>
        public PlaywrightTestObject(PageDriver PageDriver, ILogger logger, string fullyQualifiedTestName) : base(logger, fullyQualifiedTestName)
        {
            this.ManagerStore.Add(typeof(PlaywrightDriverManager).FullName, new PlaywrightDriverManager(() => PageDriver, this));
            this.SoftAssert = new SoftAssert(this.Log);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaywrightTestObject" /> class
        /// </summary>
        /// <param name="getDriver">Function for getting a Playwright page</param>
        /// <param name="logger">The test's logger</param>
        /// <param name="fullyQualifiedTestName">The test's fully qualified test name</param>
        public PlaywrightTestObject(Func<PageDriver> getDriver, ILogger logger, string fullyQualifiedTestName) : base(logger, fullyQualifiedTestName)
        {
            this.ManagerStore.Add(typeof(PlaywrightDriverManager).FullName, new PlaywrightDriverManager(getDriver, this));
            this.SoftAssert = new SoftAssert(this.Log);
        }

        /// <inheritdoc /> 
        public PlaywrightDriverManager PageManager
        {
            get
            {
                return this.ManagerStore.GetManager<PlaywrightDriverManager>(typeof(PlaywrightDriverManager).FullName);
            }
        }

        /// <inheritdoc /> 
        public PageDriver PageDriver
        {
            get
            {
                return this.PageManager.GetPageDriver();
            }
        }

        /// <inheritdoc /> 
        public void OverridePageDriver(Func<IPage> getPage)
        {
            this.PageManager.OverrideDriver(getPage);
        }

        /// <inheritdoc /> 
        public void OverridePageDriver(IPage page)
        {
            this.PageManager.OverrideDriver(() => page);
        }

        /// <inheritdoc /> 
        public void OverridePageDriver(PageDriver pageDriver)
        {
            this.PageManager.OverrideDriver(pageDriver);
        }
    }
}