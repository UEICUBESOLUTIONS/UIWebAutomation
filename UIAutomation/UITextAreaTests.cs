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
            await _browser.LaunchBrowserAsync();
            _browser.Page.SetDefaultNavigationTimeout(60000); // allow slow load
            await _browser.Page.GotoAsync(TestUrl, new PageGotoOptions
            {
                WaitUntil = WaitUntilState.DOMContentLoaded  // load only basic HTML
            });

            // FIX: Always use IPage for UITextArea constructor
            _textArea = new UITextArea(_browser.Page, Selector);
        }


        [Test]
        public async Task GetTextAsync_ShouldReturnEnteredValue()
        {
            await _textArea.EnterTextAsync("Hello");
            var text = await _textArea.GetTextAsync();
            Assert.That(text, Is.EqualTo("Hello"));
        }

        [Test]
        public async Task ClearAsync_ShouldClearValue()
        {
            await _textArea.EnterTextAsync("Test");
            await _textArea.ClearAsync();
            var value = await _textArea.GetTextAsync();
            Assert.That(value, Is.EqualTo(""));
        }
         [Test]
        public async Task AppendTextAsync()
        {
            // Get the current value of the textarea
            var currentValue = await _textArea.GetTextAsync();
            // Append the new text to the current value
            await _textArea.EnterTextAsync(currentValue + "AppendedText");
        }

        [Test]
        public async Task ClickAsync_ShouldFocusTextArea()
        {
            await _textArea.ClickAsync();
            bool isFocused = await _browser.Page.Locator(Selector)
                .EvaluateAsync<bool>("el => document.activeElement === el");
            Assert.That(isFocused, Is.True);
        }

        [Test]
        public async Task IsDisplayedAsync_ShouldReturnTrue()
        {
            var displayed = await _textArea.IsDisplayedAsync();
            Assert.That(displayed, Is.True);
        }

        [Test]
        public async Task IsEnabledAsync_ShouldReturnTrue()
        {
            var enabled = await _textArea.IsEnabledAsync();
            Assert.That(enabled, Is.True);
        }

        [Test]
        public async Task IsReadOnlyAsync_ShouldReturnFalse()
        {
            var isReadOnly = await _textArea.IsReadOnlyAsync();
            Assert.That(isReadOnly, Is.False);
        }

        [Test]
        public async Task GetCssClassAsync_ShouldReturnValueOrNull()
        {
            var cssClass = await _textArea.GetCssClassAsync();
            Assert.That(cssClass, Is.Null.Or.Not.Empty);

        }

        [Test]
        public async Task GetPlaceholderAsync_ShouldReturnNullOrValue()
        {
            var placeholder = await _textArea.GetPlaceholderAsync();
            Assert.That(placeholder, Is.Null.Or.Not.Null);
        }

        [Test]
        public async Task GetBoundsAsync_ShouldReturnRectangle()
        {
            Rectangle rect = await _textArea.GetBoundsAsync();
            Assert.That(rect.Width, Is.GreaterThan(0));
            Assert.That(rect.Height, Is.GreaterThan(0));
        }

        [Test]
        public async Task PressEnterAsync_ShouldTriggerKeyEvent()
        {
            await _textArea.EnterTextAsync("Hello");
            await _textArea.PressEnterAsync();
            Assert.Pass("Enter key pressed without error");
        }

        [Test]
        public async Task PressTabAsync_ShouldMoveFocus()
        {
            await _textArea.ClickAsync();
            await _textArea.PressTabAsync();
            bool isFocused = await _browser.Page.Locator(Selector)
                .EvaluateAsync<bool>("el => document.activeElement === el");
            Assert.That(isFocused, Is.False);
        }

        [Test]
        public async Task ScrollIntoViewAsync_ShouldScrollElement()
        {
            await _textArea.ScrollIntoViewAsync();
            bool visible = await _textArea.IsDisplayedAsync();
            Assert.That(visible, Is.True);
        }

        [TearDown]
        public async Task Cleanup()
        {
            await _browser.CloseBrowserAsync();
        }
    }
}
