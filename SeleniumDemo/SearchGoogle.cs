using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SeleniumDemo
{

    [TestClass]
    public class SearchGoogle
    {
        public static IWebDriver driver;
        private static TestContext testContextInstance;

        [ClassInitialize]
        public static void ClassInitialise(TestContext testContext)
        {
            testContextInstance = testContext;
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TeardownDriver();
        }

        [TestMethod]
        public void SearchForCheese()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");
            var driverPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var envChromeWebDriver = Environment.GetEnvironmentVariable("ChromeWebDriver");
            if (!string.IsNullOrEmpty(envChromeWebDriver) &&
               File.Exists(Path.Combine(envChromeWebDriver, "chromedriver.exe")))
            {
                driverPath = envChromeWebDriver;
            }
            ChromeDriverService defaultService = ChromeDriverService.CreateDefaultService(driverPath);
            defaultService.HideCommandPromptWindow = true;
            driver = new ChromeDriver(defaultService, chromeOptions);


            //Notice navigation is slightly different than the Java version
            //This is because 'get' is a keyword in C#
            driver.Navigate().GoToUrl("http://www.google.com/");

            // Find the text input element by its name
            IWebElement query = driver.FindElement(By.Name("q"));

            // Enter something to search for
            query.SendKeys("Cheese");

            // Now submit the form. WebDriver will find the form for us from the element
            query.Submit();

            // Google's search is rendered dynamically with JavaScript.
            // Wait for the page to load, timeout after 10 seconds
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Title.StartsWith("cheese", StringComparison.OrdinalIgnoreCase));

            // Should see: "Cheese - Google Search" (for an English locale)
            Assert.AreEqual(driver.Title, "Cheese - Google Search");


        }

        [TestMethod]
        public void SearchForSelenium()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");
            var driverPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var envChromeWebDriver = Environment.GetEnvironmentVariable("ChromeWebDriver");
            if (!string.IsNullOrEmpty(envChromeWebDriver) &&
               File.Exists(Path.Combine(envChromeWebDriver, "chromedriver.exe")))
            {
                driverPath = envChromeWebDriver;
            }
            ChromeDriverService defaultService = ChromeDriverService.CreateDefaultService(driverPath);
            defaultService.HideCommandPromptWindow = true;
            driver = new ChromeDriver(defaultService, chromeOptions);


            //Notice navigation is slightly different than the Java version
            //This is because 'get' is a keyword in C#
            driver.Navigate().GoToUrl("http://www.google.com/");

            // Find the text input element by its name
            IWebElement query = driver.FindElement(By.Name("q"));

            // Enter something to search for
            query.SendKeys("Selenium");

            // Now submit the form. WebDriver will find the form for us from the element
            query.Submit();

            // Google's search is rendered dynamically with JavaScript.
            // Wait for the page to load, timeout after 10 seconds
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Title.StartsWith("Selenium", StringComparison.OrdinalIgnoreCase));

            // Should see: "Cheese - Google Search" (for an English locale)
            Assert.AreEqual(driver.Title, "Selenium - Google Search");


        }

        [TestMethod]
        public void SearchForSeleniumFAILED()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");
            var driverPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var envChromeWebDriver = Environment.GetEnvironmentVariable("ChromeWebDriver");
            if (!string.IsNullOrEmpty(envChromeWebDriver) &&
               File.Exists(Path.Combine(envChromeWebDriver, "chromedriver.exe")))
            {
                driverPath = envChromeWebDriver;
            }
            ChromeDriverService defaultService = ChromeDriverService.CreateDefaultService(driverPath);
            defaultService.HideCommandPromptWindow = true;
            driver = new ChromeDriver(defaultService, chromeOptions);


            //Notice navigation is slightly different than the Java version
            //This is because 'get' is a keyword in C#
            driver.Navigate().GoToUrl("http://www.google.com/");

            // Find the text input element by its name
            IWebElement query = driver.FindElement(By.Name("q"));

            // Enter something to search for
            query.SendKeys("PASSED");

            // Now submit the form. WebDriver will find the form for us from the element
            query.Submit();

            // Google's search is rendered dynamically with JavaScript.
            // Wait for the page to load, timeout after 10 seconds
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Title.StartsWith("PASSED", StringComparison.OrdinalIgnoreCase));

            // Should see: "Cheese - Google Search" (for an English locale)
            Assert.AreEqual(driver.Title, "FAILED - Google Search");
        }

        private static void TeardownDriver()
        {
            EndProcessTree("chromedriver.exe");
        }

        private static void TakeScreenshot(string fileName)
        {
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            string path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            ss.SaveAsFile(path);

            testContextInstance.AddResultFile(path);
        }

        private static void EndProcessTree(string imageName)
        {
            // Inspiration
            // https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/taskkill
            // https://stackoverflow.com/questions/5901679/kill-process-tree-programmatically-in-c-sharp
            // https://stackoverflow.com/questions/36729512/internet-explorer-11-does-not-close-after-selenium-test

            // /f - force process to terminate
            // /fi <Filter> - /fi \"pid gt 0 \" - select all processes
            // /im <ImageName> - select only processes with this image name
            Process.Start(new ProcessStartInfo
            {
                FileName = "taskkill",
                Arguments = $"/f /fi \"pid gt 0\" /im {imageName}",
                CreateNoWindow = true,
                UseShellExecute = false
            })?.WaitForExit();
        }

        [TestCleanup]
        public void TestCleanup()
        {

            if (testContextInstance.CurrentTestOutcome != UnitTestOutcome.Passed)
            {
                TakeScreenshot($"{testContextInstance.TestName}.png");
            }
			
			Environment.SetEnvironmentVariable("VARIABLE_NAME", "change 545245 Today");
            Environment.SetEnvironmentVariable("sauseTest2", "change in code 01012");

            driver.Quit();
        }
    }
}
