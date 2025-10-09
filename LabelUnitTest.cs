using Microsoft.Playwright;
using NUnit.Framework;
using System.Drawing;
using System.Threading.Tasks;
using XPathion;
using XPathion.Interfaces;

namespace UIAutomation
{
    [TestFixture]
    public class UILabelTests
    {
        private IBrowserManager _browser = new UIBrowserManager();
        private UILabel _label;

        // Test URL and selector
        private const string TestUrl = "https://letcode.in/test";
        private const string Selector = "label[for='email']"; // Example label selector

        [SetUp]
        public async Task Setup()
        {
            await _browser.LaunchBrowser();
            await _browser.NavigateTo(TestUrl);
            _label = new UILabel(_browser.Page, Selector);

            // Wait for label to be visible
            await _browser.Page.Locator(Selector).WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 5000
            });
        }

        [Test]
        public async Task GetText_ShouldReturnLabelText()
        {
            string text = await _label.GetText();
            Assert.That(text, Is.Not.Null.And.Not.Empty, "Label text should not be null or empty");
        }

        [Test]
        public async Task IsDisplayed_ShouldReturnTrue()
        {
            bool displayed = await _label.IsDisplayed();
            Assert.That(displayed, Is.True, "Label should be displayed");
        }

        [Test]
        public async Task IsEnabled_ShouldReturnTrue()
        {
            bool enabled = await _label.IsEnabled();
            Assert.That(enabled, Is.True, "Label should be enabled");
        }

        [Test]
        public async Task GetTagName_ShouldReturnLabel()
        {
            string tag = await _label.GetTagName();
            Assert.That(tag.ToLower(), Is.EqualTo("label"), "Tag name should be 'label'");
        }

        [Test]
        public async Task GetBounds_ShouldReturnValidRectangle()
        {
            Rectangle bounds = await _label.GetBounds();
            Assert.That(bounds, Is.Not.EqualTo(Rectangle.Empty), "Label bounds should not be empty");
        }

        [Test]
        public async Task GetCssClass_ShouldReturnValueOrEmpty()
        {
            string cls = await _label.GetCssClass();
            Assert.That(cls, Is.Not.Null, "CSS class should not be null");
        }

        [Test]
        public async Task GetAriaLabel_ShouldReturnValueOrEmpty()
        {
            string aria = await _label.GetAriaLabel();
            Assert.That(aria, Is.Not.Null, "Aria-label should not be null");
        }

        [Test]
        public async Task GetForAttribute_ShouldReturnInputId()
        {
            string forAttr = await _label.GetForAttribute();
            Assert.That(forAttr, Is.EqualTo("email"), "Label 'for' attribute should match input ID");
        }

        [Test]
        public async Task HasCorrectText_ShouldReturnTrue()
        {
            bool correct = await _label.HasCorrectText("Email");
            Assert.That(correct, Is.True, "Label text should match expected");
        }

        [Test]
        public async Task GetTextColor_ShouldReturnColor()
        {
            Color color = await _label.GetTextColor();
            Assert.That(color, Is.Not.EqualTo(Color.Empty), "Label color should be valid");
        }

        [Test]
        public async Task ScrollIntoView_ShouldNotThrow()
        {
            Assert.That(async () => await _label.ScrollIntoView(), Throws.Nothing);
        }

        [Test]
        public async Task Hover_ShouldNotThrow()
        {
            Assert.That(async () => await _label.Hover(), Throws.Nothing);
        }

        [TearDown]
        public async Task Cleanup()
        {
            await _browser.CloseBrowser();
        }
    }
}
