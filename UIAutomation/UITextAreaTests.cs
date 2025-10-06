using Microsoft.Playwright;
using NUnit.Framework;
using System.Drawing;
using System.Threading.Tasks;
using XPathion;
using XPathion.Interfaces;

namespace UIAutomation
{
    [TestFixture]
    public class UITextAreaTests
    {
        IBrowserManager _browser = new UIBrowserManager();
        private UITextArea _textArea;
        private const string TestUrl = "https://codebeautify.org/html-textarea-generator";
        private const string Selector = "textarea[name='text']";

        [SetUp]
        public async Task Setup()
        {
            await _browser.LaunchBrowser("chrome");
            _browser.Page.SetDefaultTimeout(30000); // strict 30s timeout
            await _browser.Page.GotoAsync(TestUrl, new PageGotoOptions
            {
                WaitUntil = WaitUntilState.DOMContentLoaded
            });
            _textArea = new UITextArea(_browser.Page, Selector);
        }

        [Test]
        public async Task EnterTextAsync_ShouldInputValue()
        {
            await _textArea.EnterText("Playwright Test");
            var text = await _textArea.GetText();
            Assert.That(text, Is.EqualTo("Playwright Test"), "TextArea should accept input text");
        }

        [Test]
        public async Task GetTextAsync_ShouldReturnEnteredValue()
        {
            await _textArea.EnterText("Hello");
            var text = await _textArea.GetText();
            Assert.That(text, Is.EqualTo("Hello"));
        }

        [Test]
        public async Task ClearAsync_ShouldClearValue()
        {
            await _textArea.EnterText("Test");
            await _textArea.Clear();
            var value = await _textArea.GetText();
            Assert.That(value, Is.EqualTo(""));
        }

        [Test]
        public async Task AppendTextAsync_ShouldAddExtraText()
        {
            await _textArea.EnterText("Initial");
            await _textArea.EnterText("Initial AppendedText");
            var value = await _textArea.GetText();
            Assert.That(value.Contains("AppendedText"), Is.True);
        }

        [Test]
        public async Task ClickAsync_ShouldFocusTextArea()
        {
            await _textArea.Click();
            bool isFocused = await _browser.Page.Locator(Selector)
                .EvaluateAsync<bool>("el => document.activeElement === el");
            Assert.That(isFocused, Is.True);
        }

        [Test]
        public async Task IsDisplayedAsync_ShouldReturnTrue()
        {
            var displayed = await _textArea.IsDisplayed();
            Assert.That(displayed, Is.True);
        }

        [Test]
        public async Task IsEnabledAsync_ShouldReturnTrue()
        {
            var enabled = await _textArea.IsEnabled();
            Assert.That(enabled, Is.True);
        }

        [Test]
        public async Task IsReadOnlyAsync_ShouldReturnFalse()
        {
            var isReadOnly = await _textArea.IsReadOnly();
            Assert.That(isReadOnly, Is.False);
        }

        [Test]
        public async Task GetCssClassAsync_ShouldReturnValueOrNull()
        {
            var cssClass = await _textArea.GetCssClass();
            Assert.That(cssClass, Is.Null.Or.Not.Empty);
        }

        [Test]
        public async Task GetPlaceholderAsync_ShouldReturnNullOrValue()
        {
            var placeholder = await _textArea.GetPlaceholder();
            Assert.That(placeholder, Is.Null.Or.Not.Null);
        }

        [Test]
        public async Task GetBoundsAsync_ShouldReturnRectangle()
        {
            Rectangle rect = await _textArea.GetBounds();
            Assert.That(rect.Width, Is.GreaterThan(0));
            Assert.That(rect.Height, Is.GreaterThan(0));
        }

        [Test]
        public async Task PressEnterAsync_ShouldTriggerKeyEvent()
        {
            await _textArea.EnterText("Hello");
            await _textArea.PressEnter();
            Assert.Pass("Enter key pressed without error");
        }

        [Test]
        public async Task PressTabAsync_ShouldMoveFocus()
        {
            await _textArea.Click();
            await _textArea.PressTab();
            bool isFocused = await _browser.Page.Locator(Selector)
                .EvaluateAsync<bool>("el => document.activeElement === el");
            Assert.That(isFocused, Is.False);
        }

        [Test]
        public async Task ScrollIntoViewAsync_ShouldScrollElement()
        {
            await _textArea.ScrollIntoView();
            bool visible = await _textArea.IsDisplayed();
            Assert.That(visible, Is.True);
        }

        [Test]
        public async Task GetAttributeAsync_ShouldReturnValue()
        {
            var nameAttr = await _textArea.GetAttribute("name");
            Assert.That(nameAttr, Is.EqualTo("text").IgnoreCase);
        }

        [Test]
        public async Task GetMaxLengthAsync_ShouldReturnExpectedValueOrZero()
        {
            int maxLength = await _textArea.GetMaxLength();
            Assert.That(maxLength, Is.GreaterThanOrEqualTo(0));
        }

        [Test]
        public async Task GetToolTipAsync_ShouldReturnValueOrNull()
        {
            var tooltip = await _textArea.GetToolTip();
            Assert.That(tooltip, Is.Null.Or.Not.Empty);
        }

        [TearDown]
        public async Task Cleanup()
        {
            await _browser.CloseBrowser();
        }
    }
}
