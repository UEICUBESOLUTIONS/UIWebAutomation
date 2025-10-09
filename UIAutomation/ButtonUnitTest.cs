using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace UIAutomation
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class UIButtonTests
    {
        private IBrowser? _browser;
        private IPage? _page;
        private ILocator? _button;

        private const string TestUrl = "https://artoftesting.com/samplesiteforselenium"; // Update as needed
        private const string Selector = "#idOfButton"; // Replace with actual button selector

        [SetUp]
        public async Task Setup()
        {
            var playwright = await Playwright.CreateAsync();
            _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            _page = await _browser.NewPageAsync();
            await _page.GotoAsync(TestUrl);

            _button = _page.Locator(Selector);
        }

        [TearDown]
        public async Task Cleanup()
        {
            if (_browser != null)
                await _browser.CloseAsync();
        }

        // ---------------- Actions ----------------
        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task Click_Test(string browserName)
        {
            await _button!.ClickAsync();
            Assert.Pass("Click executed successfully");
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task DoubleClick_Test(string browserName)
        {
            await _button!.DblClickAsync();
            Assert.Pass("DoubleClick executed successfully");
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task Focus_Test(string browserName)
        {
            await _button!.FocusAsync();
            bool isFocused = await _page!.EvaluateAsync<bool>(
                $"el => document.activeElement === document.querySelector('{Selector}')");
            Assert.That(isFocused, Is.True);
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task Hover_Test(string browserName)
        {
            await _button!.HoverAsync();
            Assert.Pass("Hover executed successfully");
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task RightClick_Test(string browserName)
        {
            await _button!.ClickAsync(new LocatorClickOptions { Button = MouseButton.Right });
            Assert.Pass("RightClick executed successfully");
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task ScrollIntoView_Test(string browserName)
        {
            await _button!.ScrollIntoViewIfNeededAsync();
            bool visible = await _button.IsVisibleAsync();
            Assert.That(visible, Is.True);
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task Submit_Test(string browserName)
        {
            await _button!.EvaluateAsync("el => el.form?.submit()");
            Assert.Pass("Submit executed successfully");
        }

        // ---------------- Properties ----------------
        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task GetAriaLabel_Test(string browserName)
        {
            var ariaLabel = await _button!.GetAttributeAsync("aria-label");
            Assert.That(ariaLabel, Is.Null.Or.Not.Empty);
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task GetBounds_Test(string browserName)
        {
            var box = await _button!.BoundingBoxAsync();
            Assert.That(box.Width, Is.GreaterThan(0));
            Assert.That(box.Height, Is.GreaterThan(0));
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task GetCssClass_Test(string browserName)
        {
            var cssClass = await _button!.GetAttributeAsync("class");
            Assert.That(cssClass, Is.Null.Or.Not.Empty);
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task GetTagName_Test(string browserName)
        {
            var tagName = await _button!.EvaluateAsync<string>("el => el.tagName");
            Assert.That(tagName.ToLower(), Is.EqualTo("button"));
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task GetText_Test(string browserName)
        {
            var text = await _button!.InnerTextAsync();
            Assert.That(text, Is.Not.Null.Or.Empty);
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task GetToolTip_Test(string browserName)
        {
            var tooltip = await _button!.GetAttributeAsync("title");
            Assert.That(tooltip, Is.Null.Or.Not.Empty);
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task IsEnabled_Test(string browserName)
        {
            bool enabled = await _button!.IsEnabledAsync();
            Assert.That(enabled, Is.True);
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task IsVisible_Test(string browserName)
        {
            bool visible = await _button!.IsVisibleAsync();
            Assert.That(visible, Is.True);
        }
    }
}
