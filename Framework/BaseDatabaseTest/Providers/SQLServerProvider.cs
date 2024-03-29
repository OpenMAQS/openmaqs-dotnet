﻿//--------------------------------------------------
// <copyright file="SQLServerProvider.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>SQLServerProvider class</summary>
//--------------------------------------------------

using System.Data.SqlClient;

namespace OpenMAQS.Maqs.BaseDatabaseTest.Providers
{
    /// <summary>
    /// The SQL server provider.
    /// </summary>
    public class SqlServerProvider : IProvider<SqlConnection>
    {
        /// <summary>
        /// Method used to create a new connection for SQL server databases
        /// </summary>
        /// <param name="connectionString"> The connection string. </param>
        /// <returns> The <see cref="SqlConnection"/> connection client. </returns>
        public SqlConnection SetupDataBaseConnection(string connectionString)
        {
            SqlConnection connection = new SqlConnection
            {
                ConnectionString = connectionString
            };

            return connection;
        }
    }
}
