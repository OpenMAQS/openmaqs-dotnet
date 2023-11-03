﻿//--------------------------------------------------
// <copyright file="WebServiceNUnit.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Web service get unit tests</summary>
//--------------------------------------------------
using OpenMAQS.Maqs.BaseWebServiceTest;
using OpenMAQS.Maqs.Utilities.Helper;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using WebServiceTesterUnitTesting.Model;

namespace WebServiceTesterUnitTesting
{
    /// <summary>
    /// Test web service gets
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class WebServiceNUnit : BaseWebServiceTest
    {
        /// <summary>
        /// Test XML get
        /// </summary>
        [Test]
        [Category(TestCategories.NUnit)]
        public void GetXmlDeserialized()
        {
            WebServiceDriver client = new WebServiceDriver(new Uri(WebServiceConfig.GetWebServiceUri()));
            client.Get<ArrayOfProduct>("/api/XML_JSON/GetAllProducts", "application/xml", false);
        }

        /// <summary>
        /// Check that tests can run as repeat
        /// </summary>
        [Test]
        [Category(TestCategories.Framework)]
        [Category(TestCategories.NUnit)]
        [Repeat(2)]
        public void RepeatWorks()
        {
            // Make sure the driver was set
            Assert.IsNotNull(this.WebServiceDriver.HttpClient.BaseAddress);
        }
    }
}