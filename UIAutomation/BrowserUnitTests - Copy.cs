
using XPathion;
using XPathion.Interfaces;
namespace UIAutomation
{
    public class BrowserUnitTests
    {
        IBrowserManager _browser = new UIBrowserManager();

        [SetUp]
        public async Task Setup()
        {
             await _browser.LaunchBrowserAsync();
        }

        [Test]
        public async Task ExampleDotCom_Should_HaveCorrectTitle()
        {
            await _browser.NavigateToAsync("https://example.com");
            Thread.Sleep(2000);
            Assert.That("Example Domain" == _browser.Title, "Expected value is not correct");
        }

        [Test]
        public async Task GetTitleAsync_ShouldReturnCorrectTitle()
        {
            await _browser.NavigateToAsync("https://example.com");
            var title = await _browser.GetTitleAsync();
            Assert.That(title, Is.EqualTo("Example Domain"));
        }

        [Test]
        public async Task Url_ShouldReturnCorrectUrl()
        {
            await _browser.NavigateToAsync("https://example.com");
            Assert.That(_browser.Url, Is.EqualTo("https://example.com/"));
        }

        [Test]
        public void Port_ShouldGetAndSetPortCorrectly()
        {
            _browser.Port = 8080;
            Assert.That(_browser.Port, Is.EqualTo(8080));
        }

        [Test]
        public async Task CurrentWindowHandle_ShouldReturnHandle()
        {
            await _browser.NavigateToAsync("https://example.com");
            Assert.That(_browser.CurrentWindowHandle, Is.Not.Null);
        }

        [TearDown]
        public async Task Cleanup()
        {
            await _browser.CloseBrowserAsync();
        }
    }
}      