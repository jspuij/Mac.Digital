// <copyright file="HeaderTests.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Tests.WebUI.Shared
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Bunit;
    using FluentAssertions;
    using Mac.Digital.Services;
    using Mac.Digital.Tests.Helpers;
    using Mac.Digital.WebUI.Shared;
    using Microsoft.AspNetCore.Components;
    using Xunit;

    /// <summary>
    /// Unit tests for the Header component.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class HeaderTests
    {
        private readonly TestContext testContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderTests"/> class.
        /// </summary>
        public HeaderTests()
        {
            // don't share between tests.
            this.testContext = new TestContextFixture().TestContext;
        }

        /// <summary>
        /// Checks that the View receives a viewmodel after activation.
        /// </summary>
        [Fact]
        public void HasViewModel()
        {
            var cut = this.testContext.RenderComponent<Header>();
            cut.Instance.ViewModel.Should().NotBeNull();
        }

        /// <summary>
        /// Tests that the component renders correctly.
        /// </summary>
        [Fact]
        public void RendersTitle()
        {
            var expected = "This is a test Title";
            var titleService = this.testContext.Services.GetService<ITitleService>();
            titleService.SetTitle(expected);

            var cut = this.testContext.RenderComponent<Header>();

            var hr = cut.Find("h3");
            hr.MarkupMatches($"<h3 class:ignore>{expected}</h3>");
        }

        /// <summary>
        /// Tests that you can click the power button and that it turns green.
        /// </summary>
        [Fact]
        public void CanClickPowerOn()
        {
            var cut = this.testContext.RenderComponent<Header>();
            var powerButton = cut.FindAll("button").First();

            powerButton.Click();
            cut.WaitForAssertion(() =>
            {
                powerButton = cut.FindAll("button").First();
                powerButton.ClassList.Should().Contain("btn-success");
            });
        }

        /// <summary>
        /// Tests that you the button disables during execution.
        /// </summary>
        [Fact]
        public void PowerOnDisablesDuringExecution()
        {
            var cut = this.testContext.RenderComponent<Header>();
            var powerButton = cut.FindAll("button").First();

            powerButton.Click();
            cut.WaitForAssertion(() =>
            {
                powerButton = cut.FindAll("button").First();
                powerButton.Attributes.Should().Contain(a => a.Name == "disabled");
            });
        }

        /// <summary>
        /// Tests that you can click the power button off and that it turns back to outline and primary color.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CanClickPowerOff()
        {
            var cut = this.testContext.RenderComponent<Header>();
            var powerButton = cut.FindAll("button").First();
            powerButton.Click();
            await Task.Delay(20);

            powerButton.Click();
            cut.WaitForAssertion(() =>
            {
                powerButton = cut.FindAll("button").First();
                powerButton.ClassList.Should().Contain("btn-outline-primary");
            });
        }

        /// <summary>
        /// Tests that you can click the settings button and that it will navigate to the settings page.
        /// </summary>
        [Fact]
        public void CanClickSettings()
        {
            var cut = this.testContext.RenderComponent<Header>();
            var navigationManager = this.testContext.Services.GetService<NavigationManager>() as TestableNavigationManager;
            var settingsButton = cut.FindAll("button").Last();
            settingsButton.Click();
            navigationManager.LastUri.Should().Be("/Settings");
        }
    }
}
