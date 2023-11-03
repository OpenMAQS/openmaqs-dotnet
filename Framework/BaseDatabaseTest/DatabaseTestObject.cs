﻿//--------------------------------------------------
// <copyright file="DatabaseTestObject.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Holds database context data</summary>
//--------------------------------------------------
using OpenMAQS.Maqs.BaseTest;
using OpenMAQS.Maqs.Utilities.Logging;
using System;
using System.Data;

namespace OpenMAQS.Maqs.BaseDatabaseTest
{
    /// <summary>
    /// Database test context data
    /// </summary>
    public class DatabaseTestObject : BaseTestObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseTestObject" /> class
        /// </summary>
        /// <param name="databaseConnection">The test's database connection</param>
        /// <param name="logger">The test's logger</param>
        /// <param name="fullyQualifiedTestName">The test's fully qualified test name</param>
        public DatabaseTestObject(Func<IDbConnection> databaseConnection, ILogger logger, string fullyQualifiedTestName) : base(logger, fullyQualifiedTestName)
        {
            this.ManagerStore.Add(typeof(DatabaseDriverManager).FullName, new DatabaseDriverManager(databaseConnection, this));
        }

        /// <summary>
        /// Gets the database driver manager
        /// </summary>
        public DatabaseDriverManager DatabaseManager
        {
            get
            {
                return this.ManagerStore.GetManager<DatabaseDriverManager>();
            }
        }

        /// <summary>
        /// Gets the database driver
        /// </summary>
        public DatabaseDriver DatabaseDriver
        {
            get
            {
                return this.DatabaseManager.GetDatabaseDriver();
            }
        }

        /// <summary>
        /// Override the function for getting a database connection
        /// </summary>
        /// <param name="databaseConnection">Function for creating a database connection</param>
        public void OverrideDatabaseConnection(Func<IDbConnection> databaseConnection)
        {
            this.DatabaseManager.OverrideDriver(databaseConnection);
        }

        /// <summary>
        /// Override the database connection
        /// </summary>
        /// <param name="databaseConnection">New database connection</param>
        public void OverrideDatabaseDriver(IDbConnection databaseConnection)
        {
            this.DatabaseManager.OverrideDriver(() => databaseConnection);
        }

        /// <summary>
        /// Override the database connection driver
        /// </summary>
        /// <param name="driver">New database connection driver</param>
        public void OverrideDatabaseDriver(DatabaseDriver driver)
        {
            this.DatabaseManager.OverrideDriver(driver);
        }
    }
}