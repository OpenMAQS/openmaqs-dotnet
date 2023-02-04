﻿//-----------------------------------------------------
// <copyright file="CustomXmlMediaTypeFormatterTests.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Test the CustomXMLMediaTypeFormatter class</summary>
//-----------------------------------------------------
using OpenMaqs.BaseWebServiceTest;
using OpenMaqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace WebServiceTesterUnitTesting
{
    /// <summary>
    /// Test the CustomXMLMediaTypeFormatter class
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CustomXmlMediaTypeFormatterTests : CustomXmlMediaTypeFormatter
    {
        /// <summary>
        /// Default xmlMediaType for default constructor
        /// </summary>
        private static readonly string xmlMediaType = "text/plain";

        /// <summary>
        /// Default typesArray for default constructor
        /// </summary>
        private static readonly Type[] types = new Type[] { typeof(string) };

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomXmlMediaTypeFormatterTests"/> class
        /// </summary>
        public CustomXmlMediaTypeFormatterTests() : base(xmlMediaType, types)
        {
        }

        /// <summary>
        /// Verify that GetSerializer returns an XMLSerializer
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void GetSerializerTest()
        {
            var retVal = GetSerializer(typeof(string), string.Empty, new StringContent(string.Empty));
            Assert.AreEqual("System.Xml.Serialization.XmlSerializer", retVal.ToString());
        }

        /// <summary>
        /// Verify that GetSerializer returns an XMLDeserializer
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void GetDeserializerTest()
        {
            var retVal = GetDeserializer(typeof(string), new StringContent(string.Empty));
            Assert.AreEqual("System.Xml.Serialization.XmlSerializer", retVal.ToString());
        }
    }
}
