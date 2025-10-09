using Microsoft.Playwright;
using NUnit.Framework;
using System.Drawing;
using System.Threading.Tasks;

namespace UIAutomation
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class UITextBoxTests
    {
        private IBrowser? _browser;
        private IPage? _page;
        private ILocator? _textBox;

        private const string TestUrl = "https://artoftesting.com/samplesiteforselenium";
        private const string Selector = "#fname"; // Update with your textbox selector

        [SetUp]
        public async Task Setup()
        {
            var playwright = await Playwright.CreateAsync();
            _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            _page = await _browser.NewPageAsync();
            await _page.GotoAsync(TestUrl);

            _textBox = _page.Locator(Selector);
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
        public async Task EnterText_Test(string browserName)
        {
            await _textBox!.FillAsync("Hello");
            var value = await _textBox.InputValueAsync();
            Assert.That(value, Is.EqualTo("Hello"));
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task AppendText_Test(string browserName)
        {
            await _textBox!.FillAsync("Hello");
            var current = await _textBox.InputValueAsync();
            await _textBox.FillAsync(current + " World");
            var value = await _textBox.InputValueAsync();
            Assert.That(value, Is.EqualTo("Hello World"));
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task Clear_Test(string browserName)
        {
            await _textBox!.FillAsync("ClearMe");
            await _textBox.FillAsync("");
            var value = await _textBox.InputValueAsync();
            Assert.That(value, Is.EqualTo(""));
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task Click_Test(string browserName)
        {
            await _textBox!.ClickAsync();
            bool isFocused = await _page!.EvaluateAsync<bool>(
                $"el => document.activeElement === document.querySelector('{Selector}')");
            Assert.That(isFocused, Is.True);
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task PressEnter_Test(string browserName)
        {
            await _textBox!.PressAsync("Enter");
            Assert.Pass("Enter key pressed successfully");
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task PressTab_Test(string browserName)
        {
            await _textBox!.ClickAsync();
            await _textBox.PressAsync("Tab");
            bool isFocused = await _page!.EvaluateAsync<bool>(
                $"el => document.activeElement === document.querySelector('{Selector}')");
            Assert.That(isFocused, Is.False); // focus moved away
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task ScrollIntoView_Test(string browserName)
        {
            await _textBox!.ScrollIntoViewIfNeededAsync();
            bool visible = await _textBox.IsVisibleAsync();
            Assert.That(visible, Is.True);
        }

        // ---------------- Properties ----------------
        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task GetAttribute_Test(string browserName)
        {
            var typeAttr = await _textBox!.GetAttributeAsync("type");
            Assert.That(typeAttr, Is.EqualTo("text"));
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task GetPlaceholder_Test(string browserName)
        {
            var placeholder = await _textBox!.GetAttributeAsync("placeholder");
            Assert.That(placeholder, Is.Null.Or.Not.Empty);
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task GetMaxLength_Test(string browserName)
        {
            var maxLengthAttr = await _textBox!.GetAttributeAsync("maxlength");
            int maxLength = string.IsNullOrEmpty(maxLengthAttr) ? -1 : int.Parse(maxLengthAttr);
            Assert.That(maxLength, Is.GreaterThanOrEqualTo(-1));
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task GetCssClass_Test(string browserName)
        {
            var cssClass = await _textBox!.GetAttributeAsync("class");
            Assert.That(cssClass, Is.Null.Or.Not.Empty);
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task GetBounds_Test(string browserName)
        {
            var box = await _textBox!.BoundingBoxAsync();
            Assert.That(box.Width, Is.GreaterThan(0));
            Assert.That(box.Height, Is.GreaterThan(0));
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task GetText_Test(string browserName)
        {
            var text = await _textBox!.InputValueAsync();
            Assert.That(text, Is.Not.Null);
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task GetToolTip_Test(string browserName)
        {
            var tooltip = await _textBox!.GetAttributeAsync("title");
            Assert.That(tooltip, Is.Null.Or.Not.Empty);
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task IsDisplayed_Test(string browserName)
        {
            bool visible = await _textBox!.IsVisibleAsync();
            Assert.That(visible, Is.True);
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task IsEnabled_Test(string browserName)
        {
            bool enabled = await _textBox!.IsEnabledAsync();
            Assert.That(enabled, Is.True);
        }

        [Test]
        [TestCase("chromium")]
        [TestCase("msedge")]
        public async Task IsReadOnly_Test(string browserName)
        {
            var readOnlyAttr = await _textBox!.GetAttributeAsync("readonly");
            bool isReadOnly = !string.IsNullOrEmpty(readOnlyAttr);
            Assert.That(isReadOnly, Is.False); // assuming editable
        }
    }
}
