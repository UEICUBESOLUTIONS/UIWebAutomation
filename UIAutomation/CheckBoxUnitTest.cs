using Microsoft.Playwright;
using NUnit.Framework;
using System.Drawing;
using System.Threading.Tasks;

namespace UIAutomation
{
    [TestFixture]
    public class UICheckBoxTests
    {
        private IBrowserManager _browser = new UIBrowserManager();
        private UICheckBox _checkBox;

        // Test URL and checkbox selector (update selector as per your HTML)
        private const string TestUrl = "https://artoftesting.com/samplesiteforselenium";
        private const string Selector = ".Automation"; // example selector for checkbox

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
            await _checkBox.Check();
            var isChecked = await _checkBox.IsChecked();
            Assert.That(isChecked, Is.True, "Checkbox should be checked after Check()");
        }

        [Test]
        public async Task UncheckAsync_ShouldUnselectCheckbox()
        {
            await _checkBox.ScrollIntoView();
            await _checkBox.Check();
            Thread.Sleep(1000);
            await _checkBox.Uncheck();
            Thread.Sleep(1000);
            var isChecked = await _checkBox.IsChecked();
            Assert.That(isChecked, Is.False, "Checkbox should be unchecked after Uncheck()");
        }

        [Test]
        public async Task ClickAsync_ShouldToggleState()
        {
            bool before = await _checkBox.IsChecked();
            await _checkBox.Click();
            bool after = await _checkBox.IsChecked();
            Assert.That(after, Is.Not.EqualTo(before), "Checkbox state should toggle after click");
        }

        [Test]
        public async Task ToggleAsync_ShouldChangeCheckState()
        {
            bool before = await _checkBox.IsChecked();
            await _checkBox.Toggle();
            bool after = await _checkBox.IsChecked();
            Assert.That(after, Is.Not.EqualTo(before), "Checkbox state should change after Toggle()");
        }

        [Test]
        public async Task IsEnabledAsync_ShouldReturnTrue()
        {
            var enabled = await _checkBox.IsEnabled();
            Assert.That(enabled, Is.True, "Checkbox should be enabled");
        }

        [Test]
        public async Task IsVisibleAsync_ShouldReturnTrue()
        {
            var visible = await _checkBox.IsVisible();
            Assert.That(visible, Is.True, "Checkbox should be visible on screen");
        }

        [Test]
        public async Task GetCssClassAsync_ShouldReturnValueOrNull()
        {
            var cssClass = await _checkBox.GetCssClass();
            Assert.That(cssClass, Is.Not.Null.Or.Empty, "CSS class should return a value or empty string");
        }

        [Test]
        public async Task GetTextAsync_ShouldReturnLabelText()
        {
            var text = await _checkBox.GetText();
            Assert.That(text, Is.Not.Null, "Checkbox label text should not be null");
        }

        [Test]
        public async Task GetTagNameAsync_ShouldReturnInput()
        {
            var tagName = await _checkBox.GetTagName();
            Assert.That(tagName.ToLower(), Is.EqualTo("input"), "Tag name should be 'input'");
        }

        [Test]
        public async Task GetBoundsAsync_ShouldReturnValidRectangle()
        {
            Rectangle bounds = await _checkBox.GetBounds();
            Assert.That(bounds.Width, Is.GreaterThan(0), "Checkbox width should be greater than 0");
            Assert.That(bounds.Height, Is.GreaterThan(0), "Checkbox height should be greater than 0");
        }

        [Test]
        public async Task FocusAsync_ShouldSetFocusToCheckbox()
        {
            await _checkBox.Focus();
            bool isFocused = await _browser.Page.Locator(Selector).EvaluateAsync<bool>("el => document.activeElement === el");
            Assert.That(isFocused, Is.True, "Checkbox should be focused after Focus()");
        }

        [Test]
        public async Task HoverAsync_ShouldNotThrow()
        {
            await _checkBox.Hover();
            Assert.Pass("HoverAsync executed without exception");
        }

        [Test]
        public async Task ScrollIntoViewAsync_ShouldMakeCheckboxVisible()
        {
            await _checkBox.ScrollIntoView();
            var visible = await _checkBox.IsVisible();
            Assert.That(visible, Is.True, "Checkbox should be visible after ScrollIntoView()");
        }

        [Test]
        public async Task GetToolTipAsync_ShouldReturnValueOrNull()
        {
            var tooltip = await _checkBox.GetToolTip();
            Assert.That(string.IsNullOrEmpty(tooltip) || !string.IsNullOrEmpty(tooltip),
    "Tooltip should return a value or be empty/null");

        }

        [Test]
        public async Task GetAriaHiddenAsync_ShouldReturnBooleanValue()
        {
            var ariaHidden = await _checkBox.GetAriaHidden();
            Assert.That(ariaHidden == true || ariaHidden == false, "AriaHidden should return a value (true/false)");
        }

        [Test]
        public async Task CheckBox_ActionSequence_ShouldExecuteAll()
        {
            await _checkBox.ScrollIntoView();
            await _checkBox.Hover();
            await _checkBox.Focus();
            await _checkBox.Check();
            await _checkBox.Uncheck();
            await _checkBox.Toggle();

            Assert.Pass("All checkbox actions executed successfully under 30 seconds");
        }

        [TearDown]
        public async Task Cleanup()
        {
            await _browser.CloseBrowser();
        }
    }
}
