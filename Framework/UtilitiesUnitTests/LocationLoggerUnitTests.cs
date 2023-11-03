//--------------------------------------------------
// <copyright file="LocationLoggerUnitTests.cs" company="OpenMAQS">
//  Copyright 2022 OpenMAQS, All rights Reserved
// </copyright>
// <summary>LocationLoggerUnitTests class</summary>
//--------------------------------------------------
using OpenMAQS.Maqs.BaseTest;
using OpenMAQS.Maqs.Utilities.Helper;
using OpenMAQS.Maqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace UtilitiesUnitTesting
{
    /// <summary>
    /// Custom logging location test class
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class LocationLoggerUnitTests : BaseTest
    {
        /// <summary>
        /// Custom log file path
        /// </summary>
        private static readonly string customPath = Config.GetGeneralValue("CustomLogPath");

        /// <summary>
        /// Cleanup after the custom log path
        /// </summary>
        [ClassCleanup]
        public static void DeleteArtificats()
        {
            if (customPath != string.Empty)
            {
                Directory.Delete(customPath, true);
            }
        }

        /// <summary>
        /// Setup test with properties
        /// </summary>
        [TestInitialize]
        public void LoggingSetup()
        {
            // Set property overrides
            this.TestContext.Properties.Add("FileLoggerPath", customPath);
            Config.UpdateWithVSTestContext(this.TestContext);

            // Create a new file
            this.TestObject.Log = LoggerFactory.GetLogger(this.GetFileNameWithoutExtension(), "text", MessageType.INFORMATION);
        }

        /// <summary>
        /// Test that the file does not exist if we didn't write to it
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void NoFile()
        {
            string path = ((IFileLogger)this.Log).FilePath;
            Assert.IsFalse(System.IO.File.Exists(path), path + " exists, but it should not");
        }

        /// <summary>
        /// Test that the file does  exist if we write to it
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void File()
        {
            string path = ((IFileLogger)this.Log).FilePath;
            this.Log.LogMessage(MessageType.ERROR, "Error message");
            Assert.IsTrue(System.IO.File.Exists(path), path + " does not exist, but it should");
            Assert.IsTrue(path.StartsWith(customPath), path + " should be under" + @"C:\FrameworksCustom\");
        }
    }
}
