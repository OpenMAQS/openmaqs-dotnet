//--------------------------------------------------
// <copyright file="SeleniumTestObject.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Holds Selenium context data</summary>
//--------------------------------------------------
using OpenMaqs.BaseTest;
using OpenMaqs.Utilities.Logging;
using OpenQA.Selenium;
using System;

namespace OpenMaqs.BaseSeleniumTest
{
    /// <summary>
    /// Selenium test context data
    /// </summary>
    public class SeleniumTestObject : BaseTestObject, ISeleniumTestObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeleniumTestObject" /> class
        /// </summary>
        /// <param name="webDriver">The test's Selenium web driver</param>
        /// <param name="logger">The test's logger</param>
        /// <param name="fullyQualifiedTestName">The test's fully qualified test name</param>
        public SeleniumTestObject(IWebDriver webDriver, ILogger logger, string fullyQualifiedTestName) : base(logger, fullyQualifiedTestName)
        {
            this.ManagerStore.Add(typeof(SeleniumDriverManager).FullName, new SeleniumDriverManager(() => webDriver, this));
            this.SoftAssert = new SeleniumSoftAssert(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeleniumTestObject" /> class
        /// </summary>
        /// <param name="getDriver">Function for getting a Selenium web driver</param>
        /// <param name="logger">The test's logger</param>
        /// <param name="fullyQualifiedTestName">The test's fully qualified test name</param>
        public SeleniumTestObject(Func<IWebDriver> getDriver, ILogger logger, string fullyQualifiedTestName) : base(logger, fullyQualifiedTestName)
        {
            this.ManagerStore.Add(typeof(SeleniumDriverManager).FullName, new SeleniumDriverManager(getDriver, this));
            this.SoftAssert = new SeleniumSoftAssert(this);
        }

        /// <inheritdoc /> 
        public SeleniumDriverManager WebManager
        {
            get
            {
                return this.ManagerStore.GetManager<SeleniumDriverManager>(typeof(SeleniumDriverManager).FullName);
            }
        }

        /// <inheritdoc /> 
        public IWebDriver WebDriver
        {
            get
            {
                return this.WebManager.GetWebDriver();
            }
        }

        /// <inheritdoc /> 
        public void OverrideWebDriver(IWebDriver webDriver)
        {
            this.WebManager.OverrideDriver(webDriver);
        }

        /// <inheritdoc /> 
        public void OverrideWebDriver(Func<IWebDriver> getDriver)
        {
            this.WebManager.OverrideDriver(getDriver);
        }
    }
}