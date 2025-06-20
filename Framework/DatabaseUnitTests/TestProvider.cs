// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestProvider.cs" company="OpenMAQS">
//   Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>
//   The test provider classed used to test custom providers.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using OpenMAQS.Maqs.BaseDatabaseTest.Providers;
using Microsoft.Data.Sqlite;

namespace DatabaseUnitTests
{
    /// <summary>
    /// The test provider class for testing
    /// </summary>
    public class TestProvider : IProvider<SqliteConnection>
    {
        /// <summary>
        /// Method used to setup a SQL connection client
        /// </summary>
        /// <param name="connectionString"> The connection string. </param>
        /// <returns> The <see cref="SqliteConnection"/> connection client. </returns>
        public SqliteConnection SetupDataBaseConnection(string connectionString)
        {
            SqliteConnection connection = new SqliteConnection
            {
                ConnectionString = connectionString
            };

            return connection;
        }
    }
}
