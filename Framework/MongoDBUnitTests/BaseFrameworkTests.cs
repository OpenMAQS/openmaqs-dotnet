﻿//--------------------------------------------------
// <copyright file="BaseFrameworkTests.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Low level framework tests</summary>
//--------------------------------------------------
using OpenMaqs.BaseMongoTest;
using OpenMaqs.BaseTest;
using OpenMaqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace MongoDBUnitTests
{
    /// <summary>
    /// Framework unit test class
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    [DoNotParallelize]
    public class BaseFrameworkTests : BaseTestUnitTests.BaseFrameworkTests
    {
        /// <summary>
        ///  Base test does soft assert check
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        public new void SoftAssertWithNoFailure()
        {
            base.SoftAssertWithNoFailure();
        }

        /// <summary>
        ///  Base test does soft assert check
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        [ExpectedException(typeof(AssertFailedException))]
        public new void SoftAssertWithFailure()
        {
            base.SoftAssertWithFailure();
        }

        /// <summary>
        ///  Base test does soft assert check
        /// </summary>
        [Test]
        [Category(TestCategories.Framework)]
        [Category(TestCategories.NUnit)]
        public new void SoftAssertNUnitWithNoFailure()
        {
            base.SoftAssertWithNoFailure();
        }

        /// <summary>
        ///  Base test does soft assert check
        /// </summary>
        [Test]
        [Category(TestCategories.Framework)]
        [Category(TestCategories.NUnit)]
        public new void SoftAssertNUnitWithFailure()
        {
            base.SoftAssertNUnitWithFailure();
        }

        /// <summary>
        /// Override the base test object
        /// </summary>
        /// <returns>The base test as base Mongo</returns>
        protected override BaseTest GetBaseTest()
        {
            return new BaseMongoTest<BsonDocument>();
        }
    }
}
