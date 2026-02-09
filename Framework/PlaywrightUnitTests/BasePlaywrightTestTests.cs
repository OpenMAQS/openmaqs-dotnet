//-----------------------------------------------------
// <copyright file="BasePlaywrightTestTests.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Test different base test combinations</summary>
//-----------------------------------------------------
using OpenMAQS.Maqs.BasePlaywrightTest;
using OpenMAQS.Maqs.Utilities.Helper;
using OpenMAQS.Maqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System;

namespace PlaywrightTests
{
    /// <summary>
    /// Test base Playwrite test
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    [DoNotParallelize]
    [TestCategory(TestCategories.Playwright)]
    public class BasePlaywrightTestTests : BasePlaywrightTest
    {
        /// <summary>
        /// Make sure we have are capturing trace and video
        /// </summary>
        /// <param name="_"></param>
        [ClassInitialize]
        public static void ClassInit(TestContext _)
        {
            Config.AddTestSettingValue("CaptureVideo", "YES", ConfigSection.PlaywrightMaqs);
            Config.AddTestSettingValue("CaptureScreenshots", "YES", ConfigSection.PlaywrightMaqs);
            Config.AddTestSettingValue("CaptureSnapshots", "YES", ConfigSection.PlaywrightMaqs);
        }

        /// <summary>
        /// Cleanup after this test run
        /// </summary>
        [ClassCleanup]
        public static void ClassCleanup()
        {
            Config.ClearOverrides();
        }

        /// <summary>
        /// Make sure test passes if driver is never touched
        /// </summary>
        [TestMethod]
        public void TestWorksIfWeNeverStartDriver()
        {
            Assert.IsNotNull(this.Log);
        }

        /// <summary>
        /// Make sure test passes if driver is used
        /// </summary>
        [TestMethod]
        public void TestWorksIfLoggingIsOn()
        {
            Assert.IsNotNull(this.PageDriver);
        }

        /// <summary>
        /// Make sure test passes with correct expected exception
        /// Expected excepions at the test method level no longer exist.
        /// </summary>
        //[TestMethod]
        ////[MyExpectedException(typeof(AggregateException))]
        //public void TestFailsAsExpected()
        //{
        //    Assert.Throws<AggregateException>(() => this.PageDriver.Goto(PlaywrightConfig.WebBase()));
        //    //Assert.Throws<AssertFailedException>(() => throw new AssertFailedException());
        //}

        /// <summary>
        ///  Make sure test passes if there is something wrong with the logger
        /// </summary>
        [TestMethod]
        public void TestWorksIfLogIsInvalid()
        {
            (this.Log as IFileLogger).FilePath = null;
            Assert.IsNotNull(this.PageDriver);
        }
    }
}
