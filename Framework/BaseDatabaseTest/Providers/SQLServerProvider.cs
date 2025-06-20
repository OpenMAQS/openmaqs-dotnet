//--------------------------------------------------
// <copyright file="SQLServerProvider.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>SQLServerProvider class</summary>
//--------------------------------------------------

using Microsoft.Data.Sqlite;
//using System.Data.SqlClient;

namespace OpenMAQS.Maqs.BaseDatabaseTest.Providers
{
    /// <summary>
    /// The SQL server provider.
    /// </summary>
    public class SqlServerProvider : IProvider<SqliteConnection>
    {
        /// <summary>
        /// Method used to create a new connection for SQL server databases
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
