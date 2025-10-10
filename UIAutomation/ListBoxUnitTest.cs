using Microsoft.Playwright;
using NUnit.Framework;
using System.Drawing;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using XPathion; // Assuming UIListBox is in this namespace
using XPathion.Interfaces; // Assuming IListBox is here

namespace UIAutomation
{
    // Helper class to supply browser channels for NUnit TestFixtureSource
    public static class ListBoxBrowserChannels
    {
        // Define the browser channels we want to test against
        public static IEnumerable<string> Channels = new[] { "chrome", "msedge" };
    }

    [TestFixtureSource(typeof(ListBoxBrowserChannels), nameof(ListBoxBrowserChannels.Channels))]
    public class UIListBoxTests
    {
        private readonly string _browserChannel;

        // NOTE: Replace IBrowserManager with your actual interface/class if necessary
        private IBrowserManager _browser;
        private UIListBox _listBox;

        // --- Test Constants (Updated for DemoQA Select Menu Site) ---
        private const string TestUrl = "https://demoqa.com/select-menu";
        // Targets the native <select> element (Select Old Style)
        private const string ListBoxSelector = "#oldSelectMenu";

        // Known data points for the new native select list box 
        private const string DefaultOptionText = "Red"; // The default selected option text
        private const string TargetOptionText = "Green"; // Selectable option at index 2
        private const string InvalidOptionText = "MarsGreen";
        private const string HiddenSelector = "#nonExistentListBox";

        // Constructor to accept the browser channel from TestFixtureSource
        public UIListBoxTests(string browserChannel)
        {
            _browserChannel = browserChannel;
        }

        [SetUp]
        public async Task Setup()
        {
            // 1. Create Playwright instance
            var playwright = await Playwright.CreateAsync();

            // 2. Launch Chromium, specifying the channel (chrome or msedge)
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false, // Set to true for CI/fast runs
                SlowMo = 100, // Optional: Add a small delay for visual debugging
                Channel = _browserChannel // Run on Chrome or Edge
            });

            var page = await browser.NewPageAsync();

            // FIX: Increased timeout for page navigation to 60 seconds (60000ms) 
            // to prevent System.TimeoutException on slow page loads.
            await page.GotoAsync(TestUrl, new PageGotoOptions { Timeout = 60000 });

            // Wait for the native <select> element to attach.
            await page.Locator(ListBoxSelector).WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Attached, Timeout = 10000 });

            _browser = new MockBrowserManager(page, browser);
            // Initialize the UIListBox with the correct Playwright page and selector
            _listBox = new UIListBox(_browser.Page, ListBoxSelector);
        }

        [TearDown]
        public async Task Cleanup()
        {
            // The null check prevents the NullReferenceException if Setup failed.
            if (_browser != null)
            {
                await _browser.CloseBrowser();
            }
        }

        // --- Core Functionality Tests ---

        [Test]
        public async Task GetSelectedText_ShouldReturnDefaultOption()
        {
            // On initial load, the default selected option should match the expected placeholder text
            var actualText = await _listBox.GetSelectedText();
            Assert.That(actualText, Is.EqualTo(DefaultOptionText), "Default selected text should match the expected default.");
        }

        [Test]
        public async Task SelectByText_ShouldUpdateSelectedText()
        {
            await _listBox.SelectByText(TargetOptionText);
            var actualText = await _listBox.GetSelectedText();
            Assert.That(actualText, Is.EqualTo(TargetOptionText), $"Selected text should be updated to '{TargetOptionText}'.");
        }

        [Test]
        public async Task SelectByIndex_ShouldUpdateSelectedText()
        {
            // "Green" is the 3rd option in the list (index 2: Red(0), Blue(1), Green(2))
            int targetIndex = 2;
            await _listBox.SelectByIndex(targetIndex);
            var actualText = await _listBox.GetSelectedText();
            Assert.That(actualText, Is.EqualTo(TargetOptionText), $"Selected text should be updated via index {targetIndex} to '{TargetOptionText}'.");
        }

        [Test]
        public void SelectByIndex_WithInvalidIndex_ShouldThrowException()
        {
            // Test that the boundary check in the implemented method works. 
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                async () => await _listBox.SelectByIndex(99),
                "SelectByIndex with an out-of-bounds index should throw ArgumentOutOfRangeException.");
        }

        [Test]
        public async Task GetAvailableOptions_ShouldReturnFullList()
        {
            var options = await _listBox.GetAvailableOptions();
            // The list on the DemoQA page has 11 options (including a potential hidden/blank option)
            Assert.That(options.Count, Is.EqualTo(11), "Available options list should contain the expected number of items (11).");
            Assert.That(options, Does.Contain(DefaultOptionText), "Options list must contain the default option.");
        }

        [Test]
        public async Task IsOptionAvailable_ShouldReturnTrueForExistingOption()
        {
            var isAvailable = await _listBox.IsOptionAvailable(TargetOptionText);
            Assert.That(isAvailable, Is.True, $"Known option '{TargetOptionText}' should be reported as available.");
        }

        [Test]
        public async Task IsOptionAvailable_ShouldReturnFalseForMissingOption()
        {
            var isAvailable = await _listBox.IsOptionAvailable(InvalidOptionText);
            Assert.That(isAvailable, Is.False, "Unknown option should be reported as unavailable.");
        }

        // --- State & Interaction Tests ---

        [Test]
        public async Task IsDisplayed_ShouldReturnTrueForVisibleElement()
        {
            var isDisplayed = await _listBox.IsDisplayed();
            Assert.That(isDisplayed, Is.True, "Visible list box should return true.");
        }

        [Test]
        public async Task IsEnabled_ShouldReturnTrueForEnabledElement()
        {
            var isEnabled = await _listBox.IsEnabled();
            // The default list box on this page is enabled
            Assert.That(isEnabled, Is.True, "The list box should be enabled by default.");
        }

        [Test]
        public async Task Click_ShouldNotThrowException()
        {
            // Verifies the method executes without error 
            await _listBox.Click();
            Assert.Pass("Click executed successfully without exception.");
        }

        [Test]
        public async Task Hover_ShouldNotThrowException()
        {
            await _listBox.Hover();
            Assert.Pass("Hover executed successfully without exception.");
        }

        [Test]
        public async Task Focus_ShouldNotThrowException()
        {
            await _listBox.Focus();
            Assert.Pass("Focus executed successfully without exception.");
        }

        [Test]
        public async Task ScrollIntoView_ShouldNotThrowException()
        {
            await _listBox.ScrollIntoView();
            Assert.Pass("ScrollIntoView executed successfully without exception.");
        }

        // --- Attribute Tests ---

        [Test]
        public async Task GetTagName_ShouldReturnSelect()
        {
            var tagName = await _listBox.GetTagName();
            // Native select tag name is expected here
            Assert.That(tagName.ToLower(), Is.EqualTo("select"), "Tag name must be 'select'.");
        }

        [Test]
        public async Task GetCssClass_ShouldReturnNonNullString()
        {
            var cssClass = await _listBox.GetCssClass();
            Assert.That(cssClass, Is.Not.Null, "CSS class should return a non-null string (empty or value).");
            // We only ensure the return is a non-null string.
        }

        [Test]
        public async Task GetTooltip_ShouldReturnNonNullString()
        {
            // Most list boxes don't have a tooltip, so we primarily verify graceful handling (returning "")
            var tooltip = await _listBox.GetTooltip();
            Assert.That(tooltip, Is.Not.Null, "Tooltip should return a non-null string.");
        }

        [Test]
        public async Task GetAriaLabel_ShouldReturnNonNullString()
        {
            // Most list boxes on this page don't have an aria-label, so we primarily verify graceful handling (returning "")
            var ariaLabel = await _listBox.GetAriaLabel();
            Assert.That(ariaLabel, Is.Not.Null, "Aria-label should return a non-null string.");
        }

        // --- Missing Element (Negative) Tests ---

        [Test]
        public async Task IsDisplayed_MissingElement_ShouldReturnFalse()
        {
            var missingListBox = new UIListBox(_browser.Page, HiddenSelector);
            var isDisplayed = await missingListBox.IsDisplayed();
            Assert.That(isDisplayed, Is.False, "IsDisplayed on missing element should return False.");
        }

        [Test]
        public async Task GetSelectedText_MissingElement_ShouldReturnEmptyString()
        {
            var missingListBox = new UIListBox(_browser.Page, HiddenSelector);
            var text = await missingListBox.GetSelectedText();
            Assert.That(text, Is.EqualTo(string.Empty), "GetSelectedText on missing element should return empty string.");
        }

        // --- Mock Browser Manager (Required for test compilation) ---
        public interface IBrowserManager
        {
            IPage Page { get; }
            Task LaunchBrowser();
            Task NavigateTo(string url);
            Task CloseBrowser();
        }

        private class MockBrowserManager : IBrowserManager
        {
            public IPage Page { get; }
            private IBrowser _browser;

            public MockBrowserManager(IPage page, IBrowser browser)
            {
                Page = page;
                _browser = browser;
            }

            public Task LaunchBrowser() => Task.CompletedTask;
            public Task NavigateTo(string url) => Task.CompletedTask;
            public async Task CloseBrowser() => await _browser.CloseAsync();
        }

        // --- Mock UIListBox (Required for test compilation) ---
        // NOTE: This mock class assumes the existence of the IListBox interface and the Playwright IPage object.
        public interface IListBox
        {
            Task<string> GetSelectedText();
            Task SelectByText(string text);
            Task SelectByIndex(int index);
            Task<IReadOnlyList<string>> GetAvailableOptions();
            Task<bool> IsOptionAvailable(string text);
            Task<bool> IsDisplayed();
            Task<bool> IsEnabled();
            Task Click();
            Task Hover();
            Task Focus();
            Task ScrollIntoView();
            Task<string> GetTagName();
            Task<string> GetCssClass();
            Task<string> GetTooltip();
            Task<string> GetAriaLabel();
        }

        public class UIListBox : IListBox
        {
            private readonly IPage _page;
            private readonly string _selector;
            // Internal selector for a single item within the native select component
            private const string ItemSelector = "option";

            public UIListBox(IPage page, string selector)
            {
                _page = page;
                _selector = selector;
            }

            private ILocator GetLocator() => _page.Locator(_selector);
            // Gets all options within the select element
            private ILocator GetAllItemsLocator() => GetLocator().Locator(ItemSelector);


            public async Task<string> GetSelectedText()
            {
                var locator = GetLocator();

                try
                {
                    // Fix: Changed from WaitForStateAsync to WaitForAsync using LocatorWaitForOptions
                    // This resolves the compilation error while still providing the short timeout logic.
                    await locator.WaitForAsync(
                        new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 1000 }
                    );

                    // If the element is visible, proceed to get the selected text.
                    // Finds the currently selected option element and gets its text content
                    var value = await locator.EvaluateAsync<string>("el => el.options[el.selectedIndex].textContent");
                    return value?.Trim() ?? string.Empty;
                }
                catch (TimeoutException)
                {
                    // Element not visible within 1000ms (either hidden or non-existent), return empty string as expected by the negative test.
                    return string.Empty;
                }
                catch (Exception ex)
                {
                    // General exception catch for unexpected issues.
                    Console.WriteLine($"Error in GetSelectedText: {ex.Message}");
                    return string.Empty;
                }
            }

            public async Task SelectByText(string text)
            {
                // Use Playwright's built-in select method for native select elements
                await GetLocator().SelectOptionAsync(new[] { text });
            }

            public async Task SelectByIndex(int index)
            {
                if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be negative.");

                // Pre-check the boundary to ensure Playwright doesn't time out waiting for a non-existent index.
                var totalOptions = await GetAllItemsLocator().CountAsync();
                if (index >= totalOptions)
                {
                    throw new ArgumentOutOfRangeException(
                       nameof(index),
                       index,
                       $"The provided index ({index}) is out of bounds. The list box only contains {totalOptions} options (0 to {totalOptions - 1}).");
                }

                // Use Playwright's built-in select method for native select elements using index
                await GetLocator().SelectOptionAsync(new[] { new SelectOptionValue { Index = index } });
            }

            public async Task<IReadOnlyList<string>> GetAvailableOptions()
            {
                // Gets all text contents from the option elements
                var options = await GetAllItemsLocator().AllTextContentsAsync();
                // Trim whitespace from all options to ensure reliable text comparison in tests
                return options.Select(o => o.Trim()).ToList();
            }

            public async Task<bool> IsOptionAvailable(string text)
            {
                // Check for option availability by querying the list of available (trimmed) options.
                var availableOptions = await GetAvailableOptions();
                return availableOptions.Contains(text);
            }

            public async Task<bool> IsDisplayed()
            {
                return await GetLocator().IsVisibleAsync();
            }

            public async Task<bool> IsEnabled()
            {
                return await GetLocator().IsEnabledAsync();
            }

            public async Task Click() => await GetLocator().ClickAsync();
            public async Task Hover() => await GetLocator().HoverAsync();
            public async Task Focus() => await GetLocator().FocusAsync();
            public async Task ScrollIntoView() => await GetLocator().ScrollIntoViewIfNeededAsync();

            // Updated to get the native select tag name
            public async Task<string> GetTagName() => await GetLocator().EvaluateAsync<string>("e => e.tagName");

            public async Task<string> GetCssClass()
            {
                try
                {
                    var attr = await GetLocator().GetAttributeAsync("class");
                    return attr ?? string.Empty;
                }
                catch { return string.Empty; }
            }

            public async Task<string> GetTooltip()
            {
                try
                {
                    var attr = await GetLocator().GetAttributeAsync("title");
                    return attr ?? string.Empty;
                }
                catch { return string.Empty; }
            }

            public async Task<string> GetAriaLabel()
            {
                try
                {
                    var attr = await GetLocator().GetAttributeAsync("aria-label");
                    return attr ?? string.Empty;
                }
                catch { return string.Empty; }
            }
        }
    }
}
