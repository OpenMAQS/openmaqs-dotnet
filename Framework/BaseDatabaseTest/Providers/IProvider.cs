﻿//--------------------------------------------------
// <copyright file="IProvider.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>IProvider interface</summary>
//--------------------------------------------------

namespace OpenMAQS.Maqs.BaseDatabaseTest.Providers
{
    /// <summary>
    /// The Provider interface.
    /// </summary>
    /// <typeparam name="T"> Type of the connection client </typeparam>
    public interface IProvider<out T> where T : class
    {
        /// <summary> 
        /// Default database connection setup - Override this function to create your own connection
        /// </summary>
        /// <param name="connectionString">The database connection string</param>
        /// <returns>The http client</returns>
        T SetupDataBaseConnection(string connectionString);
    }
}
