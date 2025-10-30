//--------------------------------------------------
// <copyright file="SeleniumSoftAssert.cs" company="OpenMAQS">
//  Copyright 2023 OpenMAQS, All rights Reserved
// </copyright>
// <summary>Selenium override for the soft asserts</summary>
//--------------------------------------------------
using OpenMAQS.Maqs.BaseTest;
using OpenMAQS.Maqs.BasePlaywrightTest;
using System;

namespace OpenMAQS.Maqs.BasePlaywrightTest
{
    /// <summary>
    /// Soft Assert override for playwright tests
    /// </summary>
    public class PlaywrightSoftAssert : SoftAssert
    {
        /// <summary>
        /// WebDriver to be used
        /// </summary>
        private readonly IPlaywrightTestObject testObject;

        /// <summary>
        /// Initializes a new instance of the SeleniumSoftAssert class
        /// </summary>
        /// <param name="playwrightTestObject">The related Selenium test object</param>
        public PlaywrightSoftAssert(IPlaywrightTestObject playwrightTestObject)
            : base(playwrightTestObject.Log)
        {
            this.testObject = playwrightTestObject;
        }

        /// <inheritdoc /> 
        public override bool Assert(Action assertFunction, string assertName, string failureMessage = "")
        {
            bool didPass = base.Assert(assertFunction, assertName, failureMessage);
            if (!didPass && this.testObject.GetDriverManager<PlaywrightDriverManager>().IsDriverIntialized())
            {
                if (PlaywrightConfig.CaptureScreenshots())
                {
                    PlaywrightUtilities.CaptureScreenshot(this.testObject.PageDriver, this.testObject, this.TextToAppend(assertName));
                }

                 if (PlaywrightConfig.CapturePagesourceOnFail())
                {
                    PlaywrightUtilities.SavePageSource(this.testObject.PageDriver, this.testObject, $" ({this.NumberOfAsserts})");
                }

                return false;
            }
            return didPass;
        }

        /// <summary>
        /// Method to determine the text to be appended to the screenshot file names
        /// </summary>
        /// <param name="softAssertName">Soft assert name</param>
        /// <returns>String to be appended</returns>
        private string TextToAppend(string softAssertName)
        {

            // If softAssertName name is not provided only append the AssertNumber
            if (string.IsNullOrEmpty(softAssertName))
            {
                return $" ({this.NumberOfAsserts})";
            }
            else
            {
                // Make sure that softAssertName has valid file name characters only
                foreach (char invalidChar in System.IO.Path.GetInvalidFileNameChars())
                {
                    softAssertName = softAssertName.Replace(invalidChar, '~');
                }

                // If softAssertName is provided, use combination of softAssertName and AssertNumber
                return $" {softAssertName} ({this.NumberOfAsserts})";
            }
        }
    }
}
