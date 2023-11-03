//--------------------------------------------------
// <copyright file="BaseEmailTestSteps.cs" company="OpenMAQS">
//  Copyright 2022 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Base teststeps code for tests using email</summary>
//--------------------------------------------------
using OpenMAQS.Maqs.BaseEmailTest;
using TechTalk.SpecFlow;
using MaqsEmail = OpenMAQS.Maqs.BaseEmailTest.BaseEmailTest;

namespace OpenMAQS.Maqs.SpecFlow.TestSteps
{
    /// <summary>
    /// Base for email TestSteps classes
    /// </summary>
    [Binding, Scope(Tag = "MAQS_Email")]
    public class BaseEmailTestSteps : ExtendableTestSteps<IEmailTestObject, MaqsEmail>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEmailTestSteps" /> class
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public BaseEmailTestSteps(ScenarioContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets the email driver from the test object
        /// </summary>
        protected EmailDriver EmailDriver
        {
            get { return this.TestObject.EmailDriver; }
        }
    }
}
