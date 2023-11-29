//--------------------------------------------------
// <copyright file="BaseSeleniumTestSteps.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Base teststeps code for tests using selenium</summary>
//--------------------------------------------------
using OpenMAQS.Maqs.BaseSeleniumTest;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using MaqsSelenium = OpenMAQS.Maqs.BaseSeleniumTest.BaseSeleniumTest;

namespace OpenMAQS.Maqs.SpecFlow.TestSteps
{
    /// <summary>
    /// Base for selenium TestSteps classes
    /// </summary>
    [Binding, Scope(Tag = "MAQS_Selenium")]
    public class BaseSeleniumTestSteps : ExtendableTestSteps<ISeleniumTestObject, MaqsSelenium>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSeleniumTestSteps" /> class
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public BaseSeleniumTestSteps(ScenarioContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets the webdriver from the test object
        /// </summary>
        protected IWebDriver WebDriver
        {
            get { return this.TestObject.WebDriver; }
        }
    }
}
