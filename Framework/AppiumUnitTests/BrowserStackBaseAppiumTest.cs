﻿using CognizantSoftvision.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CognizantSoftvision.Maqs.BaseAppiumTest;

namespace AppiumUnitTests
{
    [ExcludeFromCodeCoverage]
    public class BrowserStackBaseAppiumTest : BaseAppiumTest
    {
        private static readonly string BuildDate = DateTime.Now.ToString("MMddyyyy hhmmss");

        protected override AppiumDriver GetMobileDevice()
        {
            if (string.Equals(Config.GetValueForSection(ConfigSection.AppiumMaqs, "RunOnBrowserStack"), "YES", StringComparison.OrdinalIgnoreCase))
            {
                var name = this.TestContext.FullyQualifiedTestClassName + "." + this.TestContext.TestName;
                var options = AppiumConfig.GetCapabilitiesAsObjects();

                var browserStackOptions = options["bstack:options"] as Dictionary<string, object>;
                browserStackOptions.Add("buildName", string.IsNullOrEmpty(Environment.GetEnvironmentVariable("BROWSERSTACK_BUILD_NAME")) ? BuildDate : Environment.GetEnvironmentVariable("BROWSERSTACK_BUILD_NAME"));
                browserStackOptions.Add("projectName", string.IsNullOrEmpty(Environment.GetEnvironmentVariable("BROWSERSTACK_PROJECT_NAME")) ? BuildDate : Environment.GetEnvironmentVariable("BROWSERSTACK_PROJECT_NAME"));
                browserStackOptions.Add("sessionName", name);

                var browserOptions = new ChromeOptions();
               

                //browserOptions.SetDriverOptions(options);
                foreach (var key in options.Keys)
                {
                    browserOptions.AddAdditionalOption(key, options[key]);
                }



                return new AndroidDriver(new Uri(Config.GetValueForSection(ConfigSection.AppiumMaqs, "MobileHubUrl")), browserOptions);
            }

            return base.GetMobileDevice();
        }

        [TestCleanup]
        public void Cleanup()
        {
            var passed = this.GetResultType() == CognizantSoftvision.Maqs.Utilities.Logging.TestResultType.PASS;

            if (string.Equals(Config.GetValueForSection(ConfigSection.AppiumMaqs, "RunOnBrowserstack"), "YES", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    ((IJavaScriptExecutor)this.AppiumDriver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"" + (passed ? "passed" : "failed") + "\", \"reason\": \"\"}}");
                }
                catch (Exception e)
                {
                    this.Log.LogMessage(CognizantSoftvision.Maqs.Utilities.Logging.MessageType.WARNING, "Failed to set BrowserStack Result because: " + e.Message);
                }
            }
            base.MaqsTeardown();
        }
    }
}
