//--------------------------------------------------
// <copyright file="BasePlaywrightTestSteps.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Base teststeps code for tests using Playwright</summary>
//--------------------------------------------------
using OpenMaqs.BasePlaywrightTest;
using TechTalk.SpecFlow;
using MaqsPlaywright = OpenMaqs.BasePlaywrightTest.BasePlaywrightTest;

namespace OpenMaqs.SpecFlow.TestSteps
{
    /// <summary>
    /// Base for Playwright TestSteps classes
    /// </summary>
    [Binding, Scope(Tag = "MAQS_Playwright")]
    public class BasePlaywrightTestSteps : ExtendableTestSteps<IPlaywrightTestObject, MaqsPlaywright>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasePlaywrightTestSteps" /> class
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public BasePlaywrightTestSteps(ScenarioContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets the page driver from the test object
        /// </summary>
        protected PageDriver PageDriver
        {
            get { return this.TestObject.PageDriver; }
        }
    }
}
