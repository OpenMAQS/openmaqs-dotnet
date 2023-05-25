﻿//--------------------------------------------------
// <copyright file="WebServiceBaseTests.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Web service general unit tests</summary>
//--------------------------------------------------
using OpenMaqs.BaseWebServiceTest;
using OpenMaqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using WebServiceTesterUnitTesting.Model;

namespace WebServiceTesterUnitTesting
{
    /// <summary>
    /// Test web service base test
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WebServiceBaseTests : BaseWebServiceTest
    {
        /// <summary>
        /// Default ProductJson object for use in tests
        /// </summary>
        private static readonly ProductJson Product = new ProductJson
        {
            Category = "ff",
            Id = 4,
            Name = "ff",
            Price = 3.25f
        };

        /// <summary>
        /// Verify that the webService driver can be properly set
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void SetWebServiceDriver()
        {
            WebServiceDriver driver = new WebServiceDriver(this.GetBaseWebServiceUrl());
            this.WebServiceDriver = driver;
            Assert.AreEqual(this.TestObject.WebServiceDriver.ToString(), driver.ToString());
        }

        /// <summary>
        /// Verify that SetupNoneEventFiringTester sets the ObjectUnderTest as WebClientDriver without an event firing driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void WebServiceSetupNoneEventFiringTesterTest()
        {
            // Turn off logging
            Config.AddGeneralTestSettingValues(new Dictionary<string, string> { { "Log", "NO" } });

            WebServiceDriver driver = new WebServiceDriver(this.GetBaseWebServiceUrl());
            Assert.AreEqual(this.TestObject.WebServiceDriver.ToString(), driver.ToString());
        }

        /// <summary>
        /// Verify that if EnsureSuccessStatusCode gets a false value it throws an exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(HttpRequestException))]
        public void EnsureSuccessStatusCodeThrownException()
        {
            this.WebServiceDriver.Post("notaurl", "image/GIF", null);
        }

        /// <summary>
        /// Verify that WebServiceDriver Constructor overload properly creates driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void WebServiceDriverConstructorTest()
        {
            Uri testUri = new Uri(this.GetBaseWebServiceUrl());
            WebServiceDriver testDriver = new WebServiceDriver(testUri);
            testDriver.SetCustomMediaFormatters(new List<MediaTypeFormatter>());
            Assert.IsNotNull(testDriver);
            Assert.AreEqual(testUri, testDriver.HttpClient.BaseAddress);
        }

        /// <summary>
        /// Verify that WebServiceDriver Constructor overload properly creates driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void WebServiceDriverConstructorTest2()
        {
            WebServiceDriver testDriver = new WebServiceDriver(this.GetBaseWebServiceUrl());
            testDriver.SetCustomMediaFormatters(new CustomXmlMediaTypeFormatter("application/xml", typeof(string)));
            Assert.IsNotNull(testDriver);
            Assert.AreEqual(this.GetBaseWebServiceUrl(),testDriver.HttpClient.BaseAddress.ToString());
        }

        /// <summary>
        /// Verify that WebServiceDriver Constructor overload properly creates driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void WebServiceDriverConstructorTest3()
        {
            WebServiceDriver testDriver = new WebServiceDriver(this.GetBaseWebServiceUrl());
            testDriver.SetCustomMediaFormatters(new List<MediaTypeFormatter>());
            Assert.IsNotNull(testDriver);
            Assert.AreEqual(this.GetBaseWebServiceUrl(),testDriver.HttpClient.BaseAddress.ToString());
        }

        /// <summary>
        /// Verify that BaseHttpClient setter properly sets the client
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void BaseHttpClientSetterTest()
        {
            WebServiceDriver testDriver = new WebServiceDriver(this.GetBaseWebServiceUrl());
            testDriver.SetCustomMediaFormatters(new List<MediaTypeFormatter>());
            HttpClient client = new HttpClient();
            testDriver.HttpClient = client;
            Assert.AreEqual(testDriver.HttpClient.ToString(), client.ToString());
            Assert.AreEqual(testDriver.HttpClient.Timeout.ToString(), client.Timeout.ToString());
        }

        /// <summary>
        /// Verify that WebServiceUtils.DeserializeResponse throws expected exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WebServiceUtilsDeserializeResponseThrownException()
        {
            HttpResponseMessage response = new HttpResponseMessage
            {
                Content = new StringContent("BAD_FORMAT")
            };

            WebServiceUtils.DeserializeResponse<ProductJson>(response, new List<MediaTypeFormatter> { new CustomXmlMediaTypeFormatter("image/gif", typeof(ProductJson)) });
        }

        /// <summary>
        /// Verify that WebServiceUtils.MakeStreamContent throws expected exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(NotSupportedException))]
        public void MakeStreamContentThrowException()
        {
            WebServiceUtils.MakeStreamContent<ProductJson>(Product, Encoding.UTF8, "notsupported");
        }

        /// <summary>
        /// Verify that WebServiceUtils.MakeStringContent throws expected exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(NotSupportedException))]
        public void MakeStringContentThrowException()
        {
            WebServiceUtils.MakeStringContent<ProductJson>(Product, Encoding.UTF8, "notsupported");
        }
    }
}
