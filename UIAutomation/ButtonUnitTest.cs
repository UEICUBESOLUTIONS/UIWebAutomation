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

        UIBrowserManager _browser = new UIBrowserManager();

        private UIButton _cButton;

        private UIButton _button;



        [SetUp]

        public async Task Setup()

        {

            // Initialize Playwright and launch browser

            await _browser.LaunchBrowser();



            // Navigate to test page

            await _browser.NavigateTo("https://artoftesting.com/samplesiteforselenium");



            // Initialize UIButton with a selector (replace with actual button selector)

            _button = new UIButton(_browser.Page, "#idOfButton"); // Example selector; replace with your button
            _cButton = new UIButton(_browser.Page,"#ConfirmBox");
        }


        [Test]
        public async Task Button_AcceptAlert()
        {

            await _cButton.Click();
            Thread.Sleep(1000);
            await _browser.AcceptAlert();
            Thread.Sleep(3000);
            //Assert.That(text, Is.Not.Null.And.Not.Empty, "Button text should not be null or empty");

        }


        [Test]
        public async Task Button_DismissAlert()
        {
            await _cButton.Click();
            Thread.Sleep(1000);
            await _browser.DismissAlert();
            Assert.That(_browser.GetAlertText, Is.EqualTo("Press a button!"),"Not Matching");
            //Assert.That(text, Is.Not.Null.And.Not.Empty, "Button text should not be null or empty");

        }

        [Test]

        public async Task ButtonText_ShouldBeCorrect()

        {

            var text = await _button.GetText();

            Assert.That(text, Is.Not.Null.And.Not.Empty, "Button text should not be null or empty");

        }



        [Test]

        public async Task Button_ShouldBeVisible()

        {

            var visible = await _button.IsVisible();

            Assert.That(visible, Is.True, "Button should be visible");

        }



        [Test]

        public async Task Button_ShouldBeEnabled()

        {

            var enabled = await _button.IsEnabled();

            Assert.That(enabled, Is.True, "Button should be enabled");

        }



        [Test]

        public async Task ButtonCssClass_ShouldReturnValue()

        {

            var cssClass = await _button.GetCssClass();

            Assert.That(cssClass, Is.Not.Null, "CssClass should return a value");

        }



        [Test]

        public async Task ButtonToolTip_ShouldReturnValue()

        {

            var tooltip = await _button.GetToolTip();

            Assert.That(tooltip, Is.Not.Null, "Tooltip should return a value");

        }



        [Test]

        public async Task ButtonAriaLabel_ShouldReturnValue()

        {

            var ariaLabel = await _button.GetAriaLabel();

            Assert.That(ariaLabel, Is.Not.Null, "AriaLabel should return a value");

        }



        [Test]

        public async Task ButtonTagName_ShouldReturnTag()

        {

            var tagName = await _button.GetTagName();

            Assert.That(tagName, Is.EqualTo("button"), "TagName should match the element"); // replace expected tag

        }



        [Test]

        public async Task ButtonBounds_ShouldReturnRectangle()

        {

            var bounds = await _button.GetBounds();

            Assert.That(bounds.Width, Is.GreaterThan(0), "Button width should be greater than 0");

            Assert.That(bounds.Height, Is.GreaterThan(0), "Button height should be greater than 0");

        }



        [Test]

        public async Task ButtonActions_ShouldWork()

        {

            // Test click

            await _button.Click();



            // Test double-click

            await _button.DoubleClick();



            // Test hover

            await _button.Hover();



            // Test focus

            await _button.Focus();



            // Test right-click

            await _button.RightClick();



            // Test scroll into view

            await _button.ScrollIntoView();



            // Test submit (will only work if button is in a form)

            await _button.Submit();



            Assert.Pass("All button actions executed without exceptions");

        }



        [TearDown]

        public async Task Cleanup()

        {

            await _browser.CloseBrowser();

        }

    }

}

