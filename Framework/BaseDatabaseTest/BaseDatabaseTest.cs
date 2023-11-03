//--------------------------------------------------
// <copyright file="BaseDatabaseTest.cs" company="OpenMAQS">
//  Copyright 2022 OpenMAQS, All rights Reserved
// </copyright>
// <summary>This is the base database test class</summary>
//--------------------------------------------------

using OpenMAQS.Maqs.BaseTest;
using OpenMAQS.Maqs.Utilities.Logging;
using System.Data;

namespace OpenMAQS.Maqs.BaseDatabaseTest
{
    /// <summary>
    /// Generic base database test class
    /// </summary>
    public class BaseDatabaseTest : BaseExtendableTest<DatabaseTestObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDatabaseTest"/> class.
        /// Setup the database client for each test class
        /// </summary>
        public BaseDatabaseTest() : base()
        {
        }

        /// <summary>
        /// Gets or sets the web service driver
        /// </summary>
        public DatabaseDriver DatabaseDriver
        {
            get
            {
                return this.TestObject.DatabaseDriver;
            }

            set
            {
                this.TestObject.OverrideDatabaseDriver(value);
            }
        }

        /// <summary>
        /// Get the database connection
        /// </summary>
        /// <returns>The database connection</returns>
        protected virtual IDbConnection GetDataBaseConnection()
        {
            return DatabaseConfig.GetOpenConnection();
        }

        /// <inheritdoc /> 
        protected override DatabaseTestObject CreateSpecificTestObject(ILogger log)
        {
            return new DatabaseTestObject(() => this.GetDataBaseConnection(), log, this.GetFullyQualifiedTestClassName());
        }
    }
}