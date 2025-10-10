using NUnit.Framework;
using System.Threading.Tasks;
using XPathion;
using XPathion.Interfaces;
namespace UIAutomation
{
    public class ButtonUnitTests
    {
        IBrowserManager _browser = new UIBrowserManager();
        
        private const string TestUrl = "https://codebeautify.org/html-textarea-generator"; // Update as needed
        private const string Selector = "#defaultAction"; // Replace with actual button selector
        IButton _button; 

        [SetUp]
        public async Task Setup()
        {
            await _browser.LaunchBrowser();
            _browser.Page.SetDefaultTimeout(30000); // strict 30s timeout
            await _browser.Page.GotoAsync(TestUrl);
            _button = new UIButton(_browser.Page, Selector);
        }

        [TearDown]
        public async Task Cleanup()
        {
            if (_browser != null)
                await _browser.CloseBrowser();
        }

        // ---------------- Actions ----------------
        [Test]
        public async Task Click_Test()
        {
            await _button!.Click();
            Assert.Pass("Click executed successfully");
        }

        [Test]
        public async Task DoubleClick_Test()
        {
            await _button!.DoubleClick();
            Assert.Pass("DoubleClick executed successfully");
        }

        [Test]
        public async Task Focus_Test()
        {
            await _button!.Focus();
            bool isFocused = await _browser.Page!.EvaluateAsync<bool>(
                $"el => document.activeElement === document.querySelector('{Selector}')");
            Assert.That(isFocused, Is.True);
        }

        [Test]
        public async Task Hover_Test()
        {
            await _button!.Hover();
            Assert.Pass("Hover executed successfully");
        }

        [Test]
        public async Task RightClick_Test()
        {
            await _button!.RightClick();
            Assert.Pass("RightClick executed successfully");
        }

        [Test]
        public async Task ScrollIntoView_Test()
        {
            await _button!.ScrollIntoView();
            bool visible = await _button.IsVisible();
            Assert.That(visible, Is.True);
        }

        [Test]
        public async Task Submit_Test()
        {
            await _button!.Submit();
            Assert.Pass("Submit executed successfully");
        }

        // ---------------- Properties ----------------
        [Test]
        public async Task GetAriaLabel_Test()
        {
            var ariaLabel = await _button!.GetAriaLabel();
            Assert.That(ariaLabel, Is.Null.Or.Not.Empty);
        }

        [Test]
        public async Task GetBounds_Test()
        {
            var box = await _button!.GetBounds();
            Assert.That(box.Width, Is.GreaterThan(0));
            Assert.That(box.Height, Is.GreaterThan(0));
        }

        [Test]
        public async Task GetCssClass_Test()
        {
            var cssClass = await _button!.GetCssClass();
            Assert.That(cssClass, Is.Null.Or.Not.Empty);
        }

        [Test]
        public async Task GetTagName_Test()
        {
            var tagName = await _button!.GetTagName();
            Assert.That(tagName.ToLower(), Is.EqualTo("button"));
        }

        [Test]
        public async Task GetText_Test()
        {
            var text = await _button!.GetText();
            Assert.That(text, Is.Not.Null.Or.Empty);
        }

        [Test]
        public async Task GetToolTip_Test()
        {
            var tooltip = await _button!.GetToolTip();
            Assert.That(tooltip, Is.Null.Or.Not.Empty);
        }

        [Test]
        public async Task IsEnabled_Test()
        {
            bool enabled = await _button!.IsEnabled();
            Assert.That(enabled, Is.True);
        }

        [Test]
        public async Task IsVisible_Test()
        {
            bool visible = await _button!.IsVisible();
            Assert.That(visible, Is.True);
        }
    }
}
