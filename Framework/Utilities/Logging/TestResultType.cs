//--------------------------------------------------
// <copyright file="TestResultType.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Test result type enumeration</summary>
//--------------------------------------------------

namespace OpenMAQS.Maqs.Utilities.Logging
{
    /// <summary>
    /// The type of result
    /// </summary>
    public enum TestResultType
    {
        /// <summary>
        /// The test passed
        /// </summary>
        PASS = 0,

        /// <summary>
        /// The test failed
        /// </summary>
        FAIL = 1,

        /// <summary>
        /// The test was inconclusive
        /// </summary>
        INCONCLUSIVE = 2,

        /// <summary>
        /// The test was skipped
        /// </summary>
        SKIP = 3,

        /// <summary>
        /// The test had an unexpected result
        /// </summary>
        OTHER = 4,
    }
}
