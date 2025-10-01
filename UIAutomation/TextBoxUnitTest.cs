using Microsoft.Playwright;
using NUnit.Framework;
using System.Drawing;
using System.Threading.Tasks;
using XPathion;

namespace UIAutomation
{
    [TestFixture]
    public class UITextBoxTests
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;
        private IBrowserContext _context;
        private UITextBox _textBox;

        private const string TestUrl = "https://the-internet.herokuapp.com/inputs";
        private const string Selector = "input[type='number']";

        [SetUp]
        public async Task Setup()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true // change to false if you want to see browser UI
            });

            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();

            await _page.GotoAsync(TestUrl);

            _textBox = new UITextBox(_page, Selector);
        }

        [Test]
        public async Task GetTextAsync_ShouldReturnEnteredValue()
        {
            await _textBox.EnterTextAsync("123");
            var text = await _textBox.GetTextAsync();
            Assert.That(text, Is.EqualTo("123"));
        }
        [Test]
        public async Task GetToolTipAsync_ShouldReturnNull_WhenNoTitle()
        {
            var tooltip = await _textBox.GetToolTipAsync();
            Assert.That(tooltip, Is.Null);
        }


        [Test]
        public async Task IsEnabledAsync_ShouldReturnTrue()
        {
            var enabled = await _textBox.IsEnabledAsync();
            Assert.That(enabled, Is.True);
        }

        [Test]
        public async Task IsReadOnlyAsync_ShouldReturnFalse()
        {
            // Navigate to a stable test page with an editable input
            await _page.GotoAsync("https://demo.playwright.dev/todomvc/",
                new PageGotoOptions { WaitUntil = WaitUntilState.DOMContentLoaded });

            // Use the main todo input field
            var textBox = new UITextBox(_page, "input.new-todo");

            var isReadOnly = await textBox.IsReadOnlyAsync();
            Assert.That(isReadOnly, Is.False);
        }


        [Test]
        public async Task IsDisplayedAsync_ShouldReturnTrue()
        {
            var displayed = await _textBox.IsDisplayedAsync();
            Assert.That(displayed, Is.True);
        }

        [Test]
        public async Task GetMaxLengthAsync_ShouldReturnMinusOne_WhenNotSet()
        {
            var maxLength = await _textBox.GetMaxLengthAsync();
            Assert.That(maxLength, Is.EqualTo(-1));
        }
        [Test]
        public async Task GetPlaceholderAsync_ShouldReturnNull_WhenNotSet()
        {
            var placeholder = await _textBox.GetPlaceholderAsync();
            Assert.That(placeholder, Is.Null);
        }


        [Test]
        public async Task GetCssClassAsync_ShouldReturnNull_WhenNoClass()
        {
            var cssClass = await _textBox.GetCssClassAsync();
            Assert.That(cssClass, Is.Null);
        }


        [Test]
        public async Task GetBoundsAsync_ShouldReturnRectangle()
        {
            Rectangle rect = await _textBox.GetBoundsAsync();
            Assert.That(rect.Width, Is.GreaterThan(0));
            Assert.That(rect.Height, Is.GreaterThan(0));
        }

        [Test]
        public async Task ClickAsync_ShouldFocusTextBox()
        {
            await _textBox.ClickAsync();
            bool isFocused = await _page.Locator(Selector).EvaluateAsync<bool>("el => document.activeElement === el");
            Assert.That(isFocused, Is.True);
        }

        [Test]
        public async Task ClearAsync_ShouldClearValue()
        {
            await _textBox.EnterTextAsync("456");
            await _textBox.ClearAsync();
            var value = await _textBox.GetTextAsync();
            Assert.That(value, Is.EqualTo(""));
        }

        [Test]
        public async Task EnterTextAsync_ShouldSetValue()
        {
            await _textBox.EnterTextAsync("789");
            var value = await _textBox.GetTextAsync();
            Assert.That(value, Is.EqualTo("789"));
        }

        [Test]
        public async Task AppendTextAsync_ShouldAddValue()
        {
            await _textBox.EnterTextAsync("10");
            await _textBox.AppendTextAsync("0");
            var value = await _textBox.GetTextAsync();
            Assert.That(value, Is.EqualTo("100"));
        }

        [Test]
        public async Task PressEnterAsync_ShouldTriggerKeyEvent()
        {
            await _textBox.EnterTextAsync("42");
            await _textBox.PressEnterAsync();
            // no action on this page, just checking no exception
            Assert.Pass("Enter key pressed without error.");
        }

        [Test]
        public async Task PressTabAsync_ShouldMoveFocus()
        {
            await _textBox.ClickAsync();
            await _textBox.PressTabAsync();
            bool isFocused = await _page.Locator(Selector).EvaluateAsync<bool>("el => document.activeElement === el");
            Assert.That(isFocused, Is.False); // focus moved away
        }

        [Test]
        public async Task ScrollIntoViewAsync_ShouldScrollElement()
        {
            await _textBox.ScrollIntoViewAsync();
            // Verify element is visible
            bool visible = await _textBox.IsDisplayedAsync();
            Assert.That(visible, Is.True);
        }

        [Test]
        public async Task GetAttributeAsync_ShouldReturnTypeNumber()
        {
            var typeAttr = await _textBox.GetAttributeAsync("type");
            Assert.That(typeAttr, Is.EqualTo("number"));
        }

        [TearDown]
        public async Task Cleanup()
        {
            await _browser.CloseAsync();
            _playwright.Dispose();
        }
    }
}
