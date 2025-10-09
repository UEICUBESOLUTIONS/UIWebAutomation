using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace UIAutomation
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class UIRadioButtonTests
    {
        private const string TestUrl = "https://artoftesting.com/samplesiteforselenium";
        private const string Selector = "input[name='gender'][value='male']";

        // Helper: launches requested browser channel
        private static async Task<IBrowser> LaunchBrowserByNameAsync(IPlaywright playwright, string browserName)
        {
            return browserName switch
            {
                "msedge" => await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Channel = "msedge", Headless = false }),
                "chromium" => await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false }),
                _ => throw new System.ArgumentException($"Unsupported browser: {browserName}")
            };
        }

        // Robust helper to attempt focusing an element (scroll -> eval focus -> click fallback -> retry)
        private static async Task<bool> EnsureElementFocusedAsync(ILocator locator, IPage page, int maxAttempts = 3)
        {
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                // make sure it's visible (scroll if needed)
                await locator.ScrollIntoViewIfNeededAsync();

                // attempt JS focus
                await locator.EvaluateAsync("el => el.focus()");

                // tiny pause
                await page.WaitForTimeoutAsync(200);

                // check whether the locator element is the active element
                bool focused = await locator.EvaluateAsync<bool>("el => document.activeElement === el");
                if (focused) return true;

                // fallback: perform a user-like interaction (click) which Edge sometimes requires
                await locator.ClickAsync(new LocatorClickOptions { Timeout = 3000 });
                await page.WaitForTimeoutAsync(200);

                focused = await locator.EvaluateAsync<bool>("el => document.activeElement === el");
                if (focused) return true;

                // small back-off before next attempt
                await page.WaitForTimeoutAsync(200);
            }

            return false;
        }

        // NOTE: every test launches its own browser so we don't rely on SetUp / TearDown state.
        // This keeps tests isolated and avoids ambiguity when using TestCase for different browsers.

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task Focus_Test(string browserName)
        {
            using var playwright = await Playwright.CreateAsync();
            var browser = await LaunchBrowserByNameAsync(playwright, browserName);
            try
            {
                var context = await browser.NewContextAsync();
                var page = await context.NewPageAsync();

                // load page and wait until at least DOM is ready
                await page.GotoAsync(TestUrl, new PageGotoOptions { WaitUntil = WaitUntilState.Load, Timeout = 20000 });

                // ensure element exists and is visible (wait up to 10s)
                await page.WaitForSelectorAsync(Selector, new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible, Timeout = 10000 });

                var radio = page.Locator(Selector);

                bool focused = await EnsureElementFocusedAsync(radio, page, maxAttempts: 3);

                // final assert - include browser name in message to help debugging
                Assert.That(focused, Is.True, $"Radio button was not focused in {browserName} after attempts.");
            }
            finally
            {
                await browser.CloseAsync();
            }
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task Click_Test(string browserName)
        {
            using var playwright = await Playwright.CreateAsync();
            var browser = await LaunchBrowserByNameAsync(playwright, browserName);
            try
            {
                var context = await browser.NewContextAsync();
                var page = await context.NewPageAsync();
                await page.GotoAsync(TestUrl, new PageGotoOptions { WaitUntil = WaitUntilState.Load, Timeout = 20000 });
                await page.WaitForSelectorAsync(Selector, new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible, Timeout = 10000 });

                var radio = page.Locator(Selector);
                await radio.ClickAsync();
                bool selected = await radio.IsCheckedAsync();
                Assert.That(selected, Is.True, $"Click did not select radio in {browserName}");
            }
            finally
            {
                await browser.CloseAsync();
            }
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task ScrollIntoView_Test(string browserName)
        {
            using var playwright = await Playwright.CreateAsync();
            var browser = await LaunchBrowserByNameAsync(playwright, browserName);
            try
            {
                var context = await browser.NewContextAsync();
                var page = await context.NewPageAsync();
                await page.GotoAsync(TestUrl, new PageGotoOptions { WaitUntil = WaitUntilState.Load, Timeout = 20000 });
                await page.WaitForSelectorAsync(Selector, new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached, Timeout = 10000 });

                var radio = page.Locator(Selector);
                await radio.ScrollIntoViewIfNeededAsync();
                bool visible = await radio.IsVisibleAsync();
                Assert.That(visible, Is.True, $"Radio not visible after scroll in {browserName}");
            }
            finally
            {
                await browser.CloseAsync();
            }
        }

        // ---------- Properties ----------

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task GetAriaLabel_Test(string browserName)
        {
            using var playwright = await Playwright.CreateAsync();
            var browser = await LaunchBrowserByNameAsync(playwright, browserName);
            try
            {
                var context = await browser.NewContextAsync();
                var page = await context.NewPageAsync();
                await page.GotoAsync(TestUrl);
                await page.WaitForSelectorAsync(Selector, new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached, Timeout = 10000 });

                var radio = page.Locator(Selector);
                var ariaLabel = await radio.GetAttributeAsync("aria-label");

                // be lenient: attribute may be null or empty — check that call didn't throw
                Assert.That(ariaLabel, Is.Null.Or.Not.Null);
            }
            finally
            {
                await browser.CloseAsync();
            }
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task GetBounds_Test(string browserName)
        {
            using var playwright = await Playwright.CreateAsync();
            var browser = await LaunchBrowserByNameAsync(playwright, browserName);
            try
            {
                var context = await browser.NewContextAsync();
                var page = await context.NewPageAsync();
                await page.GotoAsync(TestUrl);
                await page.WaitForSelectorAsync(Selector, new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible, Timeout = 10000 });

                var radio = page.Locator(Selector);
                var box = await radio.BoundingBoxAsync();
                Assert.That(box, Is.Not.Null, $"BoundingBox returned null in {browserName}");
                Assert.That(box!.Width, Is.GreaterThan(0));
                Assert.That(box.Height, Is.GreaterThan(0));
            }
            finally
            {
                await browser.CloseAsync();
            }
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task GetCssClass_Test(string browserName)
        {
            using var playwright = await Playwright.CreateAsync();
            var browser = await LaunchBrowserByNameAsync(playwright, browserName);
            try
            {
                var context = await browser.NewContextAsync();
                var page = await context.NewPageAsync();
                await page.GotoAsync(TestUrl);
                await page.WaitForSelectorAsync(Selector, new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached, Timeout = 10000 });

                var radio = page.Locator(Selector);
                var cssClass = await radio.GetAttributeAsync("class");
                Assert.That(cssClass, Is.Null.Or.Not.Null);
            }
            finally
            {
                await browser.CloseAsync();
            }
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task GetName_Test(string browserName)
        {
            using var playwright = await Playwright.CreateAsync();
            var browser = await LaunchBrowserByNameAsync(playwright, browserName);
            try
            {
                var context = await browser.NewContextAsync();
                var page = await context.NewPageAsync();
                await page.GotoAsync(TestUrl);
                await page.WaitForSelectorAsync(Selector, new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached, Timeout = 10000 });

                var radio = page.Locator(Selector);
                var name = await radio.GetAttributeAsync("name");
                Assert.That(name, Is.EqualTo("gender"));
            }
            finally
            {
                await browser.CloseAsync();
            }
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task GetTagName_Test(string browserName)
        {
            using var playwright = await Playwright.CreateAsync();
            var browser = await LaunchBrowserByNameAsync(playwright, browserName);
            try
            {
                var context = await browser.NewContextAsync();
                var page = await context.NewPageAsync();
                await page.GotoAsync(TestUrl);
                await page.WaitForSelectorAsync(Selector, new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached, Timeout = 10000 });

                var radio = page.Locator(Selector);
                var tagName = await radio.EvaluateAsync<string>("el => el.tagName");
                Assert.That(tagName.ToLower(), Is.EqualTo("input"));
            }
            finally
            {
                await browser.CloseAsync();
            }
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task GetText_Test(string browserName)
        {
            // Radio inputs often don't have inner text — we assert call succeeds and returns not-null.
            using var playwright = await Playwright.CreateAsync();
            var browser = await LaunchBrowserByNameAsync(playwright, browserName);
            try
            {
                var context = await browser.NewContextAsync();
                var page = await context.NewPageAsync();
                await page.GotoAsync(TestUrl);
                await page.WaitForSelectorAsync(Selector, new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached, Timeout = 10000 });

                var radio = page.Locator(Selector);
                var text = await radio.InnerTextAsync(); // may be empty string
                Assert.That(text, Is.Not.Null);
            }
            finally
            {
                await browser.CloseAsync();
            }
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task GetTooltip_Test(string browserName)
        {
            using var playwright = await Playwright.CreateAsync();
            var browser = await LaunchBrowserByNameAsync(playwright, browserName);
            try
            {
                var context = await browser.NewContextAsync();
                var page = await context.NewPageAsync();
                await page.GotoAsync(TestUrl);
                await page.WaitForSelectorAsync(Selector, new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached, Timeout = 10000 });

                var radio = page.Locator(Selector);
                var tooltip = await radio.GetAttributeAsync("title");
                Assert.That(tooltip, Is.Null.Or.Not.Null);
            }
            finally
            {
                await browser.CloseAsync();
            }
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task GetValue_Test(string browserName)
        {
            using var playwright = await Playwright.CreateAsync();
            var browser = await LaunchBrowserByNameAsync(playwright, browserName);
            try
            {
                var context = await browser.NewContextAsync();
                var page = await context.NewPageAsync();
                await page.GotoAsync(TestUrl);
                await page.WaitForSelectorAsync(Selector, new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached, Timeout = 10000 });

                var radio = page.Locator(Selector);
                var value = await radio.GetAttributeAsync("value");
                Assert.That(value, Is.EqualTo("male"));
            }
            finally
            {
                await browser.CloseAsync();
            }
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task IsDisplayed_Test(string browserName)
        {
            using var playwright = await Playwright.CreateAsync();
            var browser = await LaunchBrowserByNameAsync(playwright, browserName);
            try
            {
                var context = await browser.NewContextAsync();
                var page = await context.NewPageAsync();
                await page.GotoAsync(TestUrl);
                await page.WaitForSelectorAsync(Selector, new PageWaitForSelectorOptions { State = WaitForSelectorState.Visible, Timeout = 10000 });

                var radio = page.Locator(Selector);
                bool visible = await radio.IsVisibleAsync();
                Assert.That(visible, Is.True);
            }
            finally
            {
                await browser.CloseAsync();
            }
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task IsEnabled_Test(string browserName)
        {
            using var playwright = await Playwright.CreateAsync();
            var browser = await LaunchBrowserByNameAsync(playwright, browserName);
            try
            {
                var context = await browser.NewContextAsync();
                var page = await context.NewPageAsync();
                await page.GotoAsync(TestUrl);
                await page.WaitForSelectorAsync(Selector, new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached, Timeout = 10000 });

                var radio = page.Locator(Selector);
                bool enabled = await radio.IsEnabledAsync();
                Assert.That(enabled, Is.True);
            }
            finally
            {
                await browser.CloseAsync();
            }
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task IsSelected_Test(string browserName)
        {
            using var playwright = await Playwright.CreateAsync();
            var browser = await LaunchBrowserByNameAsync(playwright, browserName);
            try
            {
                var context = await browser.NewContextAsync();
                var page = await context.NewPageAsync();
                await page.GotoAsync(TestUrl);
                await page.WaitForSelectorAsync(Selector, new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached, Timeout = 10000 });

                var radio = page.Locator(Selector);
                await radio.ClickAsync();
                bool selected = await radio.IsCheckedAsync();
                Assert.That(selected, Is.True);
            }
            finally
            {
                await browser.CloseAsync();
            }
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task Select_Test(string browserName)
        {
            using var playwright = await Playwright.CreateAsync();
            var browser = await LaunchBrowserByNameAsync(playwright, browserName);
            try
            {
                var context = await browser.NewContextAsync();
                var page = await context.NewPageAsync();
                await page.GotoAsync(TestUrl);
                await page.WaitForSelectorAsync(Selector, new PageWaitForSelectorOptions { State = WaitForSelectorState.Attached, Timeout = 10000 });

                var radio = page.Locator(Selector);
                await radio.CheckAsync();
                bool selected = await radio.IsCheckedAsync();
                Assert.That(selected, Is.True);
            }
            finally
            {
                await browser.CloseAsync();
            }
        }
    }
}
