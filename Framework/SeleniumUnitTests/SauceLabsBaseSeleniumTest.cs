using OpenMaqs.BaseSeleniumTest;
using OpenMaqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SeleniumUnitTests
{
    [ExcludeFromCodeCoverage]
    public class SauceLabsBaseSeleniumTest : BaseSeleniumTest
    {
        private static readonly string BuildDate = DateTime.Now.ToString("MMddyyyy hhmmss");

        protected override IWebDriver GetBrowser()
        {
            if (string.Equals(Config.GetValueForSection(ConfigSection.SeleniumMaqs, "RunOnSauceLabs"), "YES", StringComparison.OrdinalIgnoreCase))
            {
                var name = this.TestContext.FullyQualifiedTestClassName + "." + this.TestContext.TestName;
                var options = SeleniumConfig.GetRemoteCapabilitiesAsObjects();

                var sauceOptions = options["bstack:options"] as Dictionary<string, object>;
                sauceOptions.Add("resolution", "1280x1024");
                sauceOptions.Add("buildName", string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SAUCE_BUILD_NAME")) ? BuildDate : Environment.GetEnvironmentVariable("SAUCE_BUILD_NAME"));
                sauceOptions.Add("sessionName", name);

                var browserOptions = new ChromeOptions
                {
                    PlatformName = "WINDOWS",
                    BrowserVersion = "latest"
                };

                browserOptions.SetDriverOptions(options);

                var remoteCapabilities = browserOptions.ToCapabilities();

                return new RemoteWebDriver(new Uri(Config.GetValueForSection(ConfigSection.SeleniumMaqs, "HubUrl")), remoteCapabilities, SeleniumConfig.GetCommandTimeout());
            }

            return base.GetBrowser();
        }

        [TestCleanup]
        public void Cleanup()
        {
            var passed = this.GetResultType() == OpenMaqs.Utilities.Logging.TestResultType.PASS;

            if (string.Equals(Config.GetValueForSection(ConfigSection.SeleniumMaqs, "RunOnSauceLabs"), "YES", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    ((IJavaScriptExecutor)this.WebDriver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"" + (passed ? "passed" : "failed") + "\", \"reason\": \"\"}}");
                }
                catch (Exception e)
                {
                    this.Log.LogMessage(OpenMaqs.Utilities.Logging.MessageType.WARNING, "Failed to set Sauce Result because: " + e.Message);
                }
            }
            base.MaqsTeardown();
        }
    }
}
