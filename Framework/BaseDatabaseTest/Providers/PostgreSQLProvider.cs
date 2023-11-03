//--------------------------------------------------
// <copyright file="PostgreSqlProvider.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>PostgreSqlProvider class</summary>
//--------------------------------------------------

using Npgsql;

namespace OpenMAQS.Maqs.BaseDatabaseTest.Providers
{
    /// <summary>
    /// The POSTGRE SQL provider.
    /// </summary>
    public class PostgreSqlProvider : IProvider<NpgsqlConnection>
    {
        /// <summary>
        /// Method used to create a new NPGSQL connection for POSTGRE SQL databases
        /// </summary>
        /// <param name="connectionString"> The connection string. </param>
        /// <returns> The <see cref="NpgsqlConnection"/>. </returns>
        public NpgsqlConnection SetupDataBaseConnection(string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}
