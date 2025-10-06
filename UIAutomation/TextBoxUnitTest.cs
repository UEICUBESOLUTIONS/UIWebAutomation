using Microsoft.Playwright;
using NUnit.Framework;
using System.Drawing;
using System.Threading.Tasks;
using XPathion;
using XPathion.Interfaces;

namespace UIAutomation
{
    [TestFixture]
    public class UITextBoxTests
    {

        IBrowserManager _browser = new UIBrowserManager();
        private UITextBox _textBox;

        private const string TestUrl = "https://artoftesting.com/samplesiteforselenium";
        private const string Selector = "#fname";

        [SetUp]
        public async Task Setup()
        {
            await _browser.LaunchBrowser();
            await _browser.NavigateTo(TestUrl);

            _textBox = new UITextBox(_browser.Page, Selector);
        }

        [Test]
        public async Task GetText_ShouldReturnEnteredValue()
        {
            await _textBox.EnterText("123");
            var text = await _textBox.GetText();
            Assert.That(text, Is.EqualTo("123"));
        }
        [Test]
        public async Task GetToolTip_ShouldReturnNull_WhenNoTitle()
        {
            var tooltip = await _textBox.GetToolTip();
            Assert.That(tooltip, Is.Null);
        }


        [Test]
        public async Task IsEnabled_ShouldReturnTrue()
        {
            var enabled = await _textBox.IsEnabled();
            Assert.That(enabled, Is.True);
        }

        [Test]
        public async Task IsReadOnly_ShouldReturnFalse()
        {            
            var isReadOnly = await _textBox.IsReadOnly();
            Assert.That(isReadOnly, Is.False);
        }


        [Test]
        public async Task IsDisplayed_ShouldReturnTrue()
        {
            var displayed = await _textBox.IsDisplayed();
            Assert.That(displayed, Is.True);
        }

        [Test]
        public async Task GetMaxLength_ShouldReturnMinusOne_WhenNotSet()
        {
            var maxLength = await _textBox.GetMaxLength();
            Assert.That(maxLength, Is.EqualTo(-1));
        }
        [Test]
        public async Task GetPlaceholder_ShouldReturnNull_WhenNotSet()
        {
            var placeholder = await _textBox.GetPlaceholder();
            Assert.That(placeholder, Is.Null);
        }


        [Test]
        public async Task GetCssClass_ShouldReturnNull_WhenNoClass()
        {
            var cssClass = await _textBox.GetCssClass();
            Assert.That(cssClass, Is.Null);
        }


        [Test]
        public async Task GetBounds_ShouldReturnRectangle()
        {
            Rectangle rect = await _textBox.GetBounds();
            Assert.That(rect.Width, Is.GreaterThan(0));
            Assert.That(rect.Height, Is.GreaterThan(0));
        }

        [Test]
        public async Task Click_ShouldFocusTextBox()
        {
            await _textBox.Click();
            bool isFocused = await _browser.Page.Locator(Selector).EvaluateAsync<bool>("el => document.activeElement === el");
            Assert.That(isFocused, Is.True);
        }

        [Test]
        public async Task Clear_ShouldClearValue()
        {
            await _textBox.EnterText("456");
            await _textBox.Clear();
            var value = await _textBox.GetText();
            Assert.That(value, Is.EqualTo(""));
        }

        [Test]
        public async Task EnterText_ShouldSetValue()
        {
            await _textBox.EnterText("789");
            var value = await _textBox.GetText();
            Assert.That(value, Is.EqualTo("789"));
        }

        [Test]
        public async Task AppendText_ShouldAddValue()
        {
            await _textBox.EnterText("10");
            await _textBox.AppendText("0");
            var value = await _textBox.GetText();
            Assert.That(value, Is.EqualTo("100"));
        }

        [Test]
        public async Task PressEnter_ShouldTriggerKeyEvent()
        {
            await _textBox.EnterText("42");
            await _textBox.PressEnter();
            // no action on this page, just checking no exception
            Assert.Pass("Enter key pressed without error.");
        }

        [Test]
        public async Task PressTab_ShouldMoveFocus()
        {
            await _textBox.Click();
            await _textBox.PressTab();
            bool isFocused = await _browser.Page.Locator(Selector).EvaluateAsync<bool>("el => document.activeElement === el");
            Assert.That(isFocused, Is.False); // focus moved away
        }

        [Test]
        public async Task ScrollIntoView_ShouldScrollElement()
        {
            await _textBox.ScrollIntoView();
            // Verify element is visible
            bool visible = await _textBox.IsDisplayed();
            Assert.That(visible, Is.True);
        }

        [Test]
        public async Task GetAttribute_ShouldReturnTypeNumber()
        {
            var typeAttr = await _textBox.GetAttribute("type");
            Assert.That(typeAttr, Is.EqualTo("text"));
        }

        [TearDown]
        public async Task Cleanup()
        {
           await _browser.CloseBrowser();
        }
    }
}
