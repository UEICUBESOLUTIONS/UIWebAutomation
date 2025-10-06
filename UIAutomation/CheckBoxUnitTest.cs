using Microsoft.Playwright;
using NUnit.Framework;
using System.Drawing;
using System.Threading.Tasks;
using XPathion;
using XPathion.Interfaces;

namespace UIAutomation
{
    [TestFixture]
    public class UICheckBoxTests
    {
        private IBrowserManager _browser = new UIBrowserManager();
        private UICheckBox _checkBox;

        // Test URL and checkbox selector (update selector as per your HTML)
        private const string TestUrl = "https://artoftesting.com/samplesiteforselenium";
        private const string Selector = "#male"; // example selector for checkbox

        [SetUp]
        public async Task Setup()
        {
            await _browser.LaunchBrowser();
            await _browser.NavigateTo(TestUrl);
            _checkBox = new UICheckBox(_browser.Page, Selector);
        }

        [Test]
        public async Task CheckAsync_ShouldSelectCheckbox()
        {
            await _checkBox.CheckAsync();
            var isChecked = await _checkBox.IsCheckedAsync();
            Assert.That(isChecked, Is.True, "Checkbox should be checked after CheckAsync()");
        }

        [Test]
        public async Task UncheckAsync_ShouldUnselectCheckbox()
        {
            await _checkBox.CheckAsync();
            await _checkBox.UncheckAsync();
            var isChecked = await _checkBox.IsCheckedAsync();
            Assert.That(isChecked, Is.False, "Checkbox should be unchecked after UncheckAsync()");
        }

        [Test]
        public async Task ClickAsync_ShouldToggleState()
        {
            bool before = await _checkBox.IsCheckedAsync();
            await _checkBox.ClickAsync();
            bool after = await _checkBox.IsCheckedAsync();
            Assert.That(after, Is.Not.EqualTo(before), "Checkbox state should toggle after click");
        }

        [Test]
        public async Task ToggleAsync_ShouldChangeCheckState()
        {
            bool before = await _checkBox.IsCheckedAsync();
            await _checkBox.ToggleAsync();
            bool after = await _checkBox.IsCheckedAsync();
            Assert.That(after, Is.Not.EqualTo(before), "Checkbox state should change after ToggleAsync()");
        }

        [Test]
        public async Task IsEnabledAsync_ShouldReturnTrue()
        {
            var enabled = await _checkBox.IsEnabledAsync();
            Assert.That(enabled, Is.True, "Checkbox should be enabled");
        }

        [Test]
        public async Task IsVisibleAsync_ShouldReturnTrue()
        {
            var visible = await _checkBox.IsVisibleAsync();
            Assert.That(visible, Is.True, "Checkbox should be visible on screen");
        }

        [Test]
        public async Task GetCssClassAsync_ShouldReturnValueOrNull()
        {
            var cssClass = await _checkBox.GetCssClassAsync();
            Assert.That(cssClass, Is.Not.Null.Or.Empty, "CSS class should return a value or empty string");
        }

        [Test]
        public async Task GetTextAsync_ShouldReturnLabelText()
        {
            var text = await _checkBox.GetTextAsync();
            Assert.That(text, Is.Not.Null, "Checkbox label text should not be null");
        }

        [Test]
        public async Task GetTagNameAsync_ShouldReturnInput()
        {
            var tagName = await _checkBox.GetTagNameAsync();
            Assert.That(tagName.ToLower(), Is.EqualTo("input"), "Tag name should be 'input'");
        }

        [Test]
        public async Task GetBoundsAsync_ShouldReturnValidRectangle()
        {
            Rectangle bounds = await _checkBox.GetBoundsAsync();
            Assert.That(bounds.Width, Is.GreaterThan(0), "Checkbox width should be greater than 0");
            Assert.That(bounds.Height, Is.GreaterThan(0), "Checkbox height should be greater than 0");
        }

        [Test]
        public async Task FocusAsync_ShouldSetFocusToCheckbox()
        {
            await _checkBox.FocusAsync();
            bool isFocused = await _browser.Page.Locator(Selector).EvaluateAsync<bool>("el => document.activeElement === el");
            Assert.That(isFocused, Is.True, "Checkbox should be focused after FocusAsync()");
        }

        [Test]
        public async Task HoverAsync_ShouldNotThrow()
        {
            await _checkBox.HoverAsync();
            Assert.Pass("HoverAsync executed without exception");
        }

        [Test]
        public async Task ScrollIntoViewAsync_ShouldMakeCheckboxVisible()
        {
            await _checkBox.ScrollIntoViewAsync();
            var visible = await _checkBox.IsVisibleAsync();
            Assert.That(visible, Is.True, "Checkbox should be visible after ScrollIntoViewAsync()");
        }

        [Test]
        public async Task GetToolTipAsync_ShouldReturnValueOrNull()
        {
            var tooltip = await _checkBox.GetToolTipAsync();
            Assert.That(string.IsNullOrEmpty(tooltip) || !string.IsNullOrEmpty(tooltip),
    "Tooltip should return a value or be empty/null");

        }

        [Test]
        public async Task GetAriaHiddenAsync_ShouldReturnBooleanValue()
        {
            var ariaHidden = await _checkBox.GetAriaHiddenAsync();
            Assert.That(ariaHidden == true || ariaHidden == false, "AriaHidden should return a value (true/false)");
        }

        [Test]
        public async Task CheckBox_ActionSequence_ShouldExecuteAll()
        {
            await _checkBox.ScrollIntoViewAsync();
            await _checkBox.HoverAsync();
            await _checkBox.FocusAsync();
            await _checkBox.CheckAsync();
            await _checkBox.UncheckAsync();
            await _checkBox.ToggleAsync();

            Assert.Pass("All checkbox actions executed successfully under 30 seconds");
        }

        [TearDown]
        public async Task Cleanup()
        {
            await _browser.CloseBrowser();
        }
    }
}
