using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;
using XPathion;
using XPathion.Interfaces;

namespace UIAutomation
{
    [TestFixture]
    public class UIButtonTests
    {
        IBrowserManager _browser = new UIBrowserManager();
        private UIButton _button;

        [SetUp]
        public async Task Setup()
        {
            // Initialize Playwright and launch browser
            await _browser.LaunchBrowserAsync();

            // Navigate to test page
            await _browser.NavigateToAsync("https://artoftesting.com/samplesiteforselenium");

            // Initialize UIButton with a selector (replace with actual button selector)
            _button = new UIButton(_browser.Page, "#submitbtn"); // Replace with the actual button ID or CSS selector

        }

        [Test]
        public async Task ButtonText_ShouldBeCorrect()
        {
            var text = await _button.GetTextAsync();
            Assert.That(text, Is.EqualTo("Submit"), "Button text should be correct");
        }

        [Test]
        public async Task Button_ShouldBeVisible()
        {
            var visible = await _button.IsVisibleAsync();
            Assert.That(visible, Is.True, "Button should be visible");
        }

        [Test]
        public async Task Button_ShouldBeEnabled()
        {
            var enabled = await _button.IsEnabledAsync();
            Assert.That(enabled, Is.True, "Button should be enabled");
        }

        [Test]
        public async Task ButtonCssClass_ShouldReturnValue()
        { 
            var cssClass = await _button.GetCssClassAsync();
            Assert.That(cssClass, Is.Not.Null, "CssClass should return a value");
        }

        [Test]
        public async Task ButtonToolTip_ShouldReturnValue()
        {
            var tooltip = await _button.GetToolTipAsync();
            Assert.That(tooltip, Is.EqualTo("Hovering over me!!"), "Tooltip should return correct value");
        }

        [Test]
        public async Task DoubleClickAsync_ShouldWork()
        {
            await _button.DoubleClickAsync();
            Assert.Pass("DoubleClickAsync executed successfully");
        }

        [Test]
        public async Task ButtonAriaLabel_ShouldReturnValue()
        {
            var ariaLabel = await _button.GetAriaLabelAsync();
            Assert.That(ariaLabel, Is.Not.Null.And.Not.Empty, "AriaLabel should return a value");
        }

        [Test]
        public async Task ButtonTagName_ShouldReturnTag()
        {
            var tagName = await _button.GetTagNameAsync();
            Assert.That(tagName.ToLower(), Is.EqualTo("button"), "TagName should match the element");
        }

        [Test]
        public async Task ButtonBounds_ShouldReturnRectangle()
        {
            var bounds = await _button.GetBoundsAsync();
            Assert.That(bounds.Width, Is.GreaterThan(0), "Button width should be greater than 0");
            Assert.That(bounds.Height, Is.GreaterThan(0), "Button height should be greater than 0");
        }

        [Test]
        public async Task Button_Click_ShouldWork()
        {
            await _button.ClickAsync();
            Assert.Pass("ClickAsync executed successfully");
        }

        [Test]
        public async Task Button_Hover_ShouldWork()
        {
            await _button.HoverAsync();
            Assert.Pass("HoverAsync executed successfully");
        }

        [Test]
        public async Task Button_Focus_ShouldWork()
        {
            await _button.FocusAsync();
            Assert.Pass("FocusAsync executed successfully");
        }

        [Test]
        public async Task Button_RightClick_ShouldWork()
        {
            await _button.RightClickAsync();
            Assert.Pass("RightClickAsync executed successfully");
        }

        [Test]
        public async Task Button_ScrollIntoView_ShouldWork()
        {
            await _button.ScrollIntoViewAsync();
            Assert.Pass("ScrollIntoViewAsync executed successfully");
        }

        [Test]
        public async Task Button_Submit_ShouldWork()
        {
            await _button.SubmitAsync();
            Assert.Pass("SubmitAsync executed successfully");
        }

        [TearDown]
        public async Task Cleanup()
        {
            await _browser.CloseBrowserAsync();
        }
    }
}
