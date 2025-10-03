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
            _button = new UIButton(_browser.Page, "#idOfButton"); // Example selector; replace with your button
        }

        [Test]
        public async Task ButtonText_ShouldBeCorrect()
        {
            var text = await _button.GetTextAsync();
            await _button.ScrollIntoViewAsync();
            Thread.Sleep(1000);
            Assert.That(text,Is.EqualTo("Submit"));
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
            Assert.That(tooltip, Is.Not.Null, "Tooltip should return a value");
        }

        [Test]
        public async Task ButtonAriaLabel_ShouldReturnValue()
        {
            var ariaLabel = await _button.GetAriaLabelAsync();
            Assert.That(ariaLabel, Is.Not.Null, "AriaLabel should return a value");
        }

        [Test]
        public async Task ButtonTagName_ShouldReturnTag()
        {
            var tagName = await _button.GetTagNameAsync();
            Assert.That(tagName, Is.EqualTo("button"), "TagName should match the element"); // replace expected tag
        }

        [Test]
        public async Task ButtonBounds_ShouldReturnRectangle()
        {
            var bounds = await _button.GetBoundsAsync();
            Assert.That(bounds.Width, Is.GreaterThan(0), "Button width should be greater than 0");
            Assert.That(bounds.Height, Is.GreaterThan(0), "Button height should be greater than 0");
        }

        [Test]
        public async Task ButtonActions_ShouldWork()
        {
            // Test click
            await _button.ClickAsync();

            // Test double-click
            await _button.DoubleClickAsync();

            // Test hover
            await _button.HoverAsync();

            // Test focus
            await _button.FocusAsync();

            // Test right-click
            await _button.RightClickAsync();

            // Test scroll into view
            await _button.ScrollIntoViewAsync();

            // Test submit (will only work if button is in a form)
            await _button.SubmitAsync();

            Assert.Pass("All button actions executed without exceptions");
        }

        [TearDown]
        public async Task Cleanup()
        {
            await _browser.CloseBrowserAsync();
        }
    }
}
