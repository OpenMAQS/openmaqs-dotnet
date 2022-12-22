//--------------------------------------------------
// <copyright file="ActionBuilder.cs" company="MAQS">
//  Copyright 2022 MAQS, All rights Reserved
// </copyright>
// <summary>Utilities class for generic accessibility methods</summary>
//--------------------------------------------------
namespace BaseSeleniumTest
{
    public class AccessiblityTest
    {
         /// <summary>
        /// Create a HTML accessibility report for an entire web page
        /// </summary>
        /// <param name="webDriver">The WebDriver</param>
        /// <param name="testObject">The TestObject to associate the report with</param>
        /// <param name="throwOnViolation">Should violations cause and exception to be thrown</param>
        /// <param name="reportTypes">What type of results do you want in the report</param>
        /// <returns>Path to the HTML report</returns>
        public static string CreateAccessibilityHtmlReport(this IWebDriver webDriver, ISeleniumTestObject testObject, bool throwOnViolation = false, ReportTypes reportTypes = ReportTypes.All)
        {
            return CreateAccessibilityHtmlReport(webDriver, testObject, () => webDriver.Analyze(), throwOnViolation, reportTypes);
        }

        /// <summary>
        /// Create a HTML accessibility report for a specific web element and all of it's children
        /// </summary>
        /// <param name="webDriver">The WebDriver</param>
        /// <param name="testObject">The TestObject to associate the report with</param>
        /// <param name="element">The WebElement you want to use as the root for your accessibility scan</param>
        /// <param name="throwOnViolation">Should violations cause and exception to be thrown</param>
        /// <param name="reportTypes">What type of results do you want in the report</param>
        /// <returns>Path to the HTML report</returns>
        public static string CreateAccessibilityHtmlReport(this IWebDriver webDriver, ISeleniumTestObject testObject, IWebElement element, bool throwOnViolation = false, ReportTypes reportTypes = ReportTypes.All)
        {
            return CreateAccessibilityHtmlReport(element, testObject, () => webDriver.Analyze(element), throwOnViolation, reportTypes);
        }

        /// <summary>
        /// Create a HTML accessibility report
        /// </summary>
        /// <param name="context">The scan context, this is either a web driver or web element</param>
        /// <param name="testObject">The TestObject to associate the report with</param>
        /// <param name="getResults">Function for getting the accessibility scan results</param>
        /// <param name="throwOnViolation">Should violations cause and exception to be thrown</param>
        /// <param name="reportTypes">What type of results do you want in the report</param>
        /// <returns>Path to the HTML report</returns>
        public static string CreateAccessibilityHtmlReport(this ISearchContext context, ISeleniumTestObject testObject, Func<AxeResult> getResults, bool throwOnViolation = false, ReportTypes reportTypes = ReportTypes.All)
        {
            // Check to see if the logger is not verbose and not already suspended
            bool restoreLogging = testObject.Log.GetLoggingLevel() != MessageType.VERBOSE && testObject.Log.GetLoggingLevel() != MessageType.SUSPENDED;

            AxeResult results;
            string report = GetAccessibilityReportPath(testObject);
            testObject.Log.LogMessage(MessageType.INFORMATION, "Running accessibility check");

            try
            {
                // Suspend logging if we are not verbose or already suspended
                if (restoreLogging)
                {
                    testObject.Log.SuspendLogging();
                }

                results = getResults();
                context.CreateAxeHtmlReport(results, report, reportTypes);
            }
            finally
            {
                // Restore logging if we suspended it
                if (restoreLogging)
                {
                    testObject.Log.ContinueLogging();
                }
            }

            // Add the report
            testObject.AddAssociatedFile(report);
            testObject.Log.LogMessage(MessageType.INFORMATION, $"Ran accessibility check and created HTML report: {report} ");

            // Throw exception if we found violations and we want that to cause an error
            if (throwOnViolation && results.Violations.Length > 0)
            {
                throw new InvalidOperationException($"Accessibility violations, see: {report} for more details.");
            }

            // Throw exception if the accessibility check had any errors
            if (results.Error?.Length > 0)
            {
                throw new InvalidOperationException($"Accessibility check failure, see: {report} for more details.");
            }

            // Return report path
            return report;
        }

        /// <summary>
        /// Run axe accessibility and log the results
        /// </summary>
        /// <param name="testObject">The test object which contains the web driver and logger you wish to use</param>
        /// <param name="throwOnViolation">Should violations cause and exception to be thrown</param>
        public static void CheckAccessibility(this ISeleniumTestObject testObject, bool throwOnViolation = false)
        {
            CheckAccessibility(testObject.WebDriver, testObject.Log, throwOnViolation);
        }

        /// <summary>
        /// Run axe accessibility and log the results
        /// </summary>
        /// <param name="webDriver">The web driver that is on the page you want to run the accessibility check on</param>
        /// <param name="logger">Where you want the check logged to</param>
        /// <param name="throwOnViolation">Should violations cause and exception to be thrown</param>
        public static void CheckAccessibility(this IWebDriver webDriver, ILogger logger, bool throwOnViolation = false)
        {
            MessageType type = logger.GetLoggingLevel();

            // Look at passed
            if (type >= MessageType.SUCCESS)
            {
                CheckAccessibilityPasses(webDriver, logger, MessageType.SUCCESS);
            }

            // Look at incomplete
            if (type >= MessageType.INFORMATION)
            {
                CheckAccessibilityIncomplete(webDriver, logger, MessageType.INFORMATION);
            }

            // Look at inapplicable
            if (type >= MessageType.VERBOSE)
            {
                CheckAccessibilityInapplicable(webDriver, logger, MessageType.VERBOSE);
            }

            // Look at violations
            MessageType messageType = throwOnViolation ? MessageType.ERROR : MessageType.WARNING;
            CheckAccessibilityViolations(webDriver, logger, messageType, throwOnViolation);
        }

        /// <summary>
        /// Run axe accessibility and log the results
        /// </summary>
        /// <param name="webDriver">The web driver that is on the page you want to run the accessibility check on</param>
        /// <param name="logger">Where you want the check logged to</param>
        /// <param name="checkType">What kind of check is being run</param>
        /// <param name="getResults">Function for getting Axe results</param>
        /// <param name="loggingLevel">What level should logging the check take, this gets used if the check doesn't throw an exception</param>
        /// <param name="throwOnResults">Throw error if any results are found</param>
        public static void CheckAccessibility(this IWebDriver webDriver, ILogger logger, string checkType, Func<AxeResultItem[]> getResults, MessageType loggingLevel, bool throwOnResults = false)
        {
            logger.LogMessage(MessageType.INFORMATION, "Running accessibility check");

            if (GetReadableAxeResults(checkType, webDriver, getResults(), out string axeText) && throwOnResults)
            {
                throw new InvalidOperationException(axeText);
            }
            else
            {
                logger.LogMessage(loggingLevel, axeText);
            }
        }

        /// <summary>
        /// Run axe accessibility and log the results
        /// </summary>
        /// <param name="webDriver">The web driver that is on the page you want to run the accessibility check on</param>
        /// <param name="logger">Where you want the check logged to</param>
        /// <param name="loggingLevel">What level should logging the check take, this gets used if the check doesn't throw an exception</param>
        public static void CheckAccessibilityPasses(this IWebDriver webDriver, ILogger logger, MessageType loggingLevel)
        {
            CheckAccessibility(webDriver, logger, AccessibilityCheckType.Passes.ToString(), () => webDriver.Analyze().Passes, loggingLevel);
        }

        ///AccessibilityCheckType
        ///
        /// <summary>
        /// Run axe accessibility and log the results
        /// </summary>
        /// <param name="webDriver">The web driver that is on the page you want to run the accessibility check on</param>
        /// <param name="logger">Where you want the check logged to</param>
        /// <param name="loggingLevel">What level should logging the check take, this gets used if the check doesn't throw an exception</param>
        /// <param name="throwOnInapplicable">Should inapplicable cause and exception to be thrown</param>
        public static void CheckAccessibilityInapplicable(this IWebDriver webDriver, ILogger logger, MessageType loggingLevel, bool throwOnInapplicable = false)
        {
            CheckAccessibility(webDriver, logger, AccessibilityCheckType.Inapplicable.ToString(), () => webDriver.Analyze().Inapplicable, loggingLevel, throwOnInapplicable);
        }

        ///AccessibilityCheckType
        ///
        /// <summary>
        /// Run axe accessibility and log the results
        /// </summary>
        /// <param name="webDriver">The web driver that is on the page you want to run the accessibility check on</param>
        /// <param name="logger">Where you want the check logged to</param>
        /// <param name="loggingLevel">What level should logging the check take, this gets used if the check doesn't throw an exception</param>
        /// <param name="throwOnIncomplete">Should incomplete cause and exception to be thrown</param>
        public static void CheckAccessibilityIncomplete(this IWebDriver webDriver, ILogger logger, MessageType loggingLevel, bool throwOnIncomplete = false)
        {
            CheckAccessibility(webDriver, logger, AccessibilityCheckType.Incomplete.ToString(), () => webDriver.Analyze().Incomplete, loggingLevel, throwOnIncomplete);
        }

        /// <summary>
        /// Run axe accessibility and log the results
        /// </summary>
        /// <param name="webDriver">The web driver that is on the page you want to run the accessibility check on</param>
        /// <param name="logger">Where you want the check logged to</param>
        /// <param name="loggingLevel">What level should logging the check take, this gets used if the check doesn't throw an exception</param>
        /// <param name="throwOnViolation">Should violations cause and exception to be thrown</param>
        public static void CheckAccessibilityViolations(this IWebDriver webDriver, ILogger logger, MessageType loggingLevel, bool throwOnViolation = false)
        {
            CheckAccessibility(webDriver, logger, AccessibilityCheckType.Violations.ToString(), () => webDriver.Analyze().Violations, loggingLevel, throwOnViolation);
        }

        /// <summary>
        /// Parses scanned accessibility results
        /// </summary>
        /// <param name="typeOfScan">Type of scan</param>
        /// <param name="webDriver">Web driver the scan was run on</param>
        /// <param name="scannedResults">The scan results</param>
        /// <param name="messageString">Pretty scan results</param>
        /// <returns>True if the scan found anything</returns>
        public static bool GetReadableAxeResults(string typeOfScan, IWebDriver webDriver, AxeResultItem[] scannedResults, out string messageString)
        {
            StringBuilder message = new StringBuilder();
            int axeRules = scannedResults.Length;

            message.AppendLine("ACCESSIBILITY CHECK");
            message.AppendLine($"{typeOfScan} check for '{webDriver.Url}'");
            message.AppendLine($"Found {axeRules} items");

            if (axeRules == 0)
            {
                messageString = message.ToString().Trim();
                return false;
            }

            message.AppendLine(string.Empty);

            int loops = 1;

            foreach (var element in scannedResults)
            {
                message.AppendLine($"{loops++}: {element.Help}");
                message.AppendLine($"{"\t"}Description: {element.Description}");
                message.AppendLine($"{"\t"}Help URL: {element.HelpUrl}");
                message.AppendLine($"{"\t"}Impact: {element.Impact}");
                message.AppendLine($"{"\t"}Tags: {string.Join(", ", element.Tags)}");

                foreach (var item in element.Nodes)
                {
                    message.AppendLine($"{"\t"}{"\t"}HTML element: {item.Html}");

                    foreach (var target in item.Target)
                    {
                        message.AppendLine($"{"\t"}{"\t"}Selector: {target}");
                    }
                }

                message.AppendLine(string.Empty);
                message.AppendLine(string.Empty);
            }

            messageString = message.ToString().Trim();
            return true;
        }

        /// <summary>
        /// Get the javaScript executor from a web element or web driver
        /// </summary>
        /// <param name="searchContext">The search context</param>
        /// <returns>The javaScript executor</returns>
        public static IJavaScriptExecutor SearchContextToJavaScriptExecutor(ISearchContext searchContext)
        {
            return (searchContext is IJavaScriptExecutor executor) ? executor : (IJavaScriptExecutor)WebElementToWebDriver((IWebElement)searchContext);
        }

        /// <summary>
        /// Get the web driver from a web element or web driver
        /// </summary>
        /// <param name="searchContext">The search context</param>
        /// <returns>The web driver</returns>
        public static IWebDriver SearchContextToWebDriver(ISearchContext searchContext)
        {
            return (searchContext is IWebDriver driver) ? driver : WebElementToWebDriver((IWebElement)searchContext);
        }

        /// <summary>
        /// Get a unique file name that we can user for the accessibility HTML report
        /// </summary>
        /// <param name="testObject">The TestObject to associate the report with</param>
        /// <returns>A unique HTML file name, includes full path</returns>
        private static string GetAccessibilityReportPath(ISeleniumTestObject testObject)
        {
            string logDirectory = testObject.Log is IFileLogger log ? Path.GetDirectoryName(log.FilePath) : LoggingConfig.GetLogDirectory();
            string reportBaseName = testObject.Log is IFileLogger logger ? $"{Path.GetFileNameWithoutExtension(logger.FilePath)}_Axe" : "AxeReport";
            string reportFile = Path.Combine(logDirectory, $"{reportBaseName}.html");
            int reportNumber = 0;

            while (File.Exists(reportFile))
            {
                reportFile = Path.Combine(logDirectory, $"{reportBaseName}{reportNumber++}.html");
            }

            return reportFile;
        }
    }
}