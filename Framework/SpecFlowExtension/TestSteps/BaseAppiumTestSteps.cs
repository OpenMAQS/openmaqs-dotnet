//--------------------------------------------------
// <copyright file="BaseAppiumTestSteps.cs" company="MAQS">
//  Copyright 2022 MAQS, All rights Reserved
// </copyright>
// <summary>Base teststeps code for tests using appium</summary>
//--------------------------------------------------
using Maqs.BaseAppiumTest;
using OpenQA.Selenium.Appium;
using TechTalk.SpecFlow;
using MaqsAppium = Maqs.BaseAppiumTest.BaseAppiumTest;

namespace Maqs.SpecFlow.TestSteps
{
    /// <summary>
    /// Base for appium TestSteps classes
    /// </summary>
    [Binding, Scope(Tag = "MAQS_Appium")]
    public class BaseAppiumTestSteps : ExtendableTestSteps<IAppiumTestObject, MaqsAppium>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAppiumTestSteps" /> class
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public BaseAppiumTestSteps(ScenarioContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets the Appium driver from the test object
        /// </summary>
        protected AppiumDriver AppiumDriver
        {
            get { return this.TestObject.AppiumDriver; }
        }
    }
}
