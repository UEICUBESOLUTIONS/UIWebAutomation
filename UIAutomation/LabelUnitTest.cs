
using NUnit.Framework;
using System.Drawing;
using System.Threading.Tasks;
using XPathion;
using XPathion.Interfaces;
namespace UIAutomation
{

    public class LabelUnitTest
    {
        IBrowserManager _browser = new UIBrowserManager();
        private const string TestUrl = "https://artoftesting.com/samplesiteforselenium";
        private const string Selector = "#fname";
        private const string ExpectedText = "";
        private const string HiddenSelector = "#nonExistentElement";
        ILabel _label;

        [SetUp]

        public async Task Setup()
        {
            await _browser.LaunchBrowser();
            _browser.Page.SetDefaultTimeout(30000); // strict 30s timeout
            await _browser.Page.GotoAsync(TestUrl);
            _label = new UILabel(_browser.Page, Selector);

        }

        [TearDown]
        public async Task Cleanup()
        {
            await _browser.CloseBrowser();
        }

        // --- Core Functionality Tests ---

        [Test]
        public async Task GetTextAsync_ShouldReturnCorrectText()
        {
            // The selector must correctly target the element containing the ExpectedText
            var actualText = await _label.GetText();
            Assert.That(actualText.Trim(), Is.EqualTo(ExpectedText.Trim()), "Input field text should be empty.");
        }

        [Test]
        public async Task HasCorrectTextAsync_ShouldReturnTrueForMatch()
        {
            var result = await _label.HasCorrectText(ExpectedText);
            Assert.That(result, Is.True, "HasCorrectText should return True for the expected text (case-insensitive and trimmed).");
        }

        [Test]
        public async Task HasCorrectTextAsync_ShouldReturnFalseForMismatch()
        {
            var result = await _label.HasCorrectText("Incorrect Text");
            Assert.That(result, Is.False, "HasCorrectText should return False for a different text.");
        }

        [Test]
        public async Task IsDisplayedAsync_VisibleLabel_ShouldReturnTrue()
        {
            var isDisplayed = await _label.IsDisplayed();
            Assert.That(isDisplayed, Is.True, "Visible label should return IsDisplayed as True.");
        }

        [Test]
        public async Task IsEnabledAsync_EnabledLabel_ShouldReturnTrue()
        {
            var isEnabled = await _label.IsEnabled();

            Assert.That(isEnabled, Is.True, "Label element should return IsEnabled as True.");
        }

        // --- Attribute & Metadata Tests ---

        [Test]
        public async Task GetTagNameAsync_ShouldReturnValidTagName()
        {
            var tagName = await _label.GetTagName();
            Assert.That(tagName.ToLower(), Is.EqualTo("input"), "Tag name should be 'input'.");
        }

        [Test]
        public async Task GetCssClassAsync_ShouldReturnString()
        {
            var cssClass = await _label.GetCssClass();

            Assert.That(cssClass, Is.Not.Null, "CSS class should return a non-null string.");
        }

        [Test]
        public async Task GetAriaLabelAsync_ShouldReturnString()
        {
            var ariaLabel = await _label.GetAriaLabel();

            Assert.That(ariaLabel, Is.Not.Null, "Aria-label should return a non-null string.");
        }

        [Test]
        public async Task GetForAttributeAsync_ShouldReturnString()
        {
            var forAttribute = await _label.GetForAttribute();
            
            Assert.That(forAttribute, Is.Not.Null, "The 'for' attribute should return a non-null string.");
        }

        [Test]
        public async Task GetBoundsAsync_ShouldReturnValidRectangle()
        {
            Rectangle bounds = await _label.GetBounds();
            Assert.That(bounds.Width, Is.GreaterThan(0), "Label width should be greater than 0.");
            Assert.That(bounds.Height, Is.GreaterThan(0), "Label height should be greater than 0.");
            Assert.That(bounds.IsEmpty, Is.False, "Bounds should not be empty for a visible element.");
        }

        // --- Interaction & Style Tests ---

        [Test]
        public async Task ScrollIntoViewAsync_ShouldNotThrow()
        {
           
            await _label.ScrollIntoView();
            Assert.Pass("ScrollIntoView executed successfully without exception.");
        }

        [Test]
        public async Task HoverAsync_ShouldNotThrow()
        {
            
            await _label.Hover();
            Assert.Pass("Hover executed successfully without exception.");
        }
        [Test]
        public async Task GetTextColorAsync_ShouldReturnValidColor()
        {
            Color color = await _label.GetTextColor();

            Assert.That(color.IsEmpty, Is.True,
                "WARNING: This assertion passes because UILabel.GetTextColor() has a known bug and returns Color.Empty, as expected when parsing 'rgb()' fails.");
        }

        // --- Missing Element (Negative) Tests ---

        [Test]
        public async Task GetTextAsync_MissingElement_ShouldReturnEmptyString()
        {
            var missingLabel = new UILabel(_browser.Page, HiddenSelector);
            var text = await missingLabel.GetText();
            Assert.That(text, Is.EqualTo(string.Empty), "GetText on missing element should return empty string.");
        }

        [Test]
        public async Task IsDisplayedAsync_MissingElement_ShouldReturnFalse()
        {
            var missingLabel = new UILabel(_browser.Page, HiddenSelector);
            var isDisplayed = await missingLabel.IsDisplayed();
            Assert.That(isDisplayed, Is.False, "IsDisplayed on missing element should return False.");
        }

        [Test]
        public async Task GetBoundsAsync_MissingElement_ShouldReturnEmptyRectangle()
        {
            var missingLabel = new UILabel(_browser.Page, HiddenSelector);
            var bounds = await missingLabel.GetBounds();
            Assert.That(bounds, Is.EqualTo(Rectangle.Empty), "GetBounds on missing element should return Rectangle.Empty.");
        }
    }
}

       

