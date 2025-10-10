//--------------------------------------------------
// <copyright file="EmailDriverFailureTests.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Email driver failure tests</summary>
//--------------------------------------------------
using OpenMAQS.Maqs.BaseEmailTest;
using OpenMAQS.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeKit;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FrameworkUnitTests
{
    /// <summary>
    /// Test the email driver store
    /// </summary>
    [TestClass]
    [TestCategory(TestCategories.Email)]
    [ExcludeFromCodeCoverage]
    public class EmailDriverFailureTests : BaseEmailTest
    {
        /// <summary>
        /// Setup a fake email client
        /// </summary>
        [TestInitialize]
        public void SetupMoqDriver()
        {
            this.TestObject.OverrideEmailClient(() => EmailDriverMocks.GetMoq().Object);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        //[ExpectedException(typeof(TimeoutException))]
        public void MailBoxNamesError()
        {
            Assert.Throws<TimeoutException>(() => EmailDriver.GetMailBoxNames());
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        //[ExpectedException(typeof(TimeoutException))]
        public void MailoxError()
        {
            Assert.Throws<TimeoutException>(() => EmailDriver.GetMailbox(string.Empty));
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        //[ExpectedException(typeof(TimeoutException))]
        public void SelectBoxError()
        {
            Assert.Throws<TimeoutException>(() => EmailDriver.SelectMailbox(string.Empty));
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        //[ExpectedException(typeof(NullReferenceException))]
        public void CreateMailboxError()
        {
            Assert.Throws<NullReferenceException>(() => EmailDriver.CreateMailbox(string.Empty));
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        //[ExpectedException(typeof(FormatException))]
        public void GetMessageError()
        {
            Assert.Throws<FormatException>(() => EmailDriver.GetMessage(string.Empty));
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        //[ExpectedException(typeof(TimeoutException))]
        public void GetAllMessageHeadersError()
        {
            Assert.Throws<TimeoutException>(() => EmailDriver.GetAllMessageHeaders(string.Empty));
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        //[ExpectedException(typeof(NotImplementedException))]
        public void DeleteMessageError()
        {
            Assert.Throws<NotImplementedException>(() => EmailDriver.DeleteMessage(string.Empty));
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        //[ExpectedException(typeof(NotImplementedException))]
        public void DeleteMimeMessageError()
        {
            Assert.Throws<NotImplementedException>(() => EmailDriver.DeleteMessage(new MimeMessage()));
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        //[ExpectedException(typeof(NotImplementedException))]
        public void MoveMimeMessageError()
        {
            Assert.Throws<NotImplementedException>(() => EmailDriver.MoveMailMessage(new MimeMessage(), string.Empty));
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        //[ExpectedException(typeof(NotImplementedException))]
        public void MoveMessageError()
        {
            Assert.Throws<NotImplementedException>(() => EmailDriver.MoveMailMessage(string.Empty, string.Empty));
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        //[ExpectedException(typeof(NotImplementedException))]
        public void GetAttachmentsError()
        {
            Assert.Throws<NotImplementedException>(() => EmailDriver.GetAttachments(string.Empty));
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        //[ExpectedException(typeof(NotSupportedException))]
        public void GetMimeAttachmentsError()
        {
            Assert.Throws<NotSupportedException>(() => EmailDriver.GetAttachments(EmailDriverMocks.GetMocMime()));
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        //[ExpectedException(typeof(NotSupportedException))]
        public void DownloadAttachmentsToError()
        {
            Assert.Throws<NotSupportedException>(() => EmailDriver.DownloadAttachments(EmailDriverMocks.GetMocMime(), string.Empty));
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        //[ExpectedException(typeof(NotSupportedException))]
        public void DownloadAttachmentsError()
        {
            Assert.Throws<NotSupportedException>(() => EmailDriver.DownloadAttachments(EmailDriverMocks.GetMocMime()));
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        //[ExpectedException(typeof(TimeoutException))]
        public void SearchMessagesError()
        {
            Assert.Throws<TimeoutException>(() => EmailDriver.SearchMessages(null));
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        //[ExpectedException(typeof(NullReferenceException))]
        public void GetContentTypesError()
        {
            Assert.Throws<NullReferenceException>(() => EmailDriver.GetContentTypes(null));
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        //[ExpectedException(typeof(NullReferenceException))]
        public void GetBodyByContentTypesError()
        {
            Assert.Throws<NullReferenceException>(() => EmailDriver.GetBodyByContentTypes(null, string.Empty));
        }
    }
}
