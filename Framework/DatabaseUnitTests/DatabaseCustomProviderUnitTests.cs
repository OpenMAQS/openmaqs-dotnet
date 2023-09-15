//--------------------------------------------------
// <copyright file="DatabaseCustomProviderUnitTests.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Database base test unit tests</summary>
//--------------------------------------------------

using OpenMaqs.BaseDatabaseTest;
using OpenMaqs.Utilities.Helper;
using NUnit.Framework;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DatabaseUnitTests
{
    /// <summary>
    /// Test basic database base test functionality
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [NonParallelizable]
    public class DatabaseCustomProviderUnitTests : BaseDatabaseTest
    {
        /// <summary>
        /// Check that we get back the state table
        /// </summary>
        [Test]
        [Category(TestCategories.Database)]
        public void CustomIProviderTest()
        {
            var states = this.DatabaseDriver.Query("SELECT * FROM States").ToList();

            // Our database only has 49 states
            Assert.AreEqual(49, states.Count, "Expected 49 states.");
        }

        /// <summary>
        /// Get the database connection
        /// </summary>
        /// <returns>The database connection</returns>
        protected override IDbConnection GetDataBaseConnection()
        {
            return DatabaseConfig.GetOpenConnection("DatabaseUnitTests.TestProvider", DatabaseConfig.GetConnectionString());
        }
    }
}
