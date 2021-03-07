// <copyright file="BoilerTests.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Tests.WebUI.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Bunit;
    using FluentAssertions;
    using Mac.Digital.Services;
    using Mac.Digital.Simulation;
    using Mac.Digital.Tests.Helpers;
    using Mac.Digital.WebUI.Shared;
    using Microsoft.AspNetCore.Components;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="Boiler"/> view.
    /// </summary>
    public class BoilerTests
    {
        private TestContext testContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoilerTests"/> class.
        /// </summary>
        public BoilerTests()
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
            var cut = this.testContext.RenderComponent<Boiler>();
            cut.Instance.ViewModel.Should().NotBeNull();
        }

        /// <summary>
        /// Checks that the View renders the pressure.
        /// </summary>
        [Fact]
        public void RendersPressure()
        {
            var cut = this.testContext.RenderComponent<Boiler>();
            var actual = cut.Find(".fa-cloudscale").Parent.TextContent;

            actual.Should().Match($"*{CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator}* bar");
        }

        /// <summary>
        /// Checks that the View renders the temperature.
        /// </summary>
        [Fact]
        public void RendersTemperature()
        {
            var cut = this.testContext.RenderComponent<Boiler>();
            var actual = cut.Find(".fa-thermometer-half").Parent.TextContent;

            actual.Should().Match($"*{CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator}* °C");
        }

        /// <summary>
        /// Checks that the View renders the boiler title.
        /// </summary>
        [Fact]
        public void RendersBoilerTitle()
        {
            var cut = this.testContext.RenderComponent<Boiler>();
            cut.Find("h5").MarkupMatches(@"<h5 class=""card-title text-left"">Boiler</h5>");
        }

        /// <summary>
        /// Checks that the View renders the rising trend.
        /// </summary>
        [Fact]
        public void RendersRisingTrend()
        {
            var cut = this.testContext.RenderComponent<Boiler>();
            var powerService = this.testContext.Services.GetService<IPowerService>();
            powerService.PowerOn(CancellationToken.None);

            cut.WaitForAssertion(() =>
            {
                cut.Find(".fa-long-arrow-alt-up").MarkupMatches(@"<i class=""fas fa-long-arrow-alt-up text-secondary""></i>");
            });
        }

        /// <summary>
        /// Checks that the View renders the falling trend.
        /// </summary>
        [Fact]
        public void RendersFallingTrend()
        {
            this.testContext = new TestContextFixture(new ServiceSimulation(
                SynchronizationContext.Current, 10, false, 50m, 1.2m, 0m, 0m, false)).TestContext;
            var cut = this.testContext.RenderComponent<Boiler>();

            cut.WaitForAssertion(() =>
            {
                cut.Find(".fa-long-arrow-alt-down").MarkupMatches(@"<i class=""fas fa-long-arrow-alt-down text-secondary""></i>");
            });
        }

        /// <summary>
        /// Checks that the View renders the protectiond.
        /// </summary>
        [Fact]
        public void RendersProtection()
        {
            this.testContext = new TestContextFixture(new ServiceSimulation(
                SynchronizationContext.Current, 10, false, 20m, 1.2m, 0m, 0m, true)).TestContext;
            var cut = this.testContext.RenderComponent<Boiler>();

            cut.Find(".fa-exclamation-triangle").MarkupMatches(@"<i class=""fas fa-exclamation-triangle text-warning""></i>");
        }

        /// <summary>
        /// Checks that the View supports disabling the protection.
        /// </summary>
        [Fact]
        public void DisablesProtection()
        {
            this.testContext = new TestContextFixture(new ServiceSimulation(
                SynchronizationContext.Current, 10, true, 20m, 1.2m, 0m, 0m, true)).TestContext;
            var cut = this.testContext.RenderComponent<Boiler>();

            var button = cut.Find("a:has(.fa-exclamation-triangle)");
            button.Click();

            cut.WaitForAssertion(() =>
            {
                cut.Markup.Should().NotContain("fa-exclamation-triangle");
            });
        }

        /// <summary>
        /// Checks that the View renders the flame when heating is off.
        /// </summary>
        [Fact]
        public void RendersFlameOff()
        {
            var cut = this.testContext.RenderComponent<Boiler>();
            cut.Find(".fa-burn").MarkupMatches(@"<i class=""fas fa-burn text-secondary""></i>");
        }

        /// <summary>
        /// Checks that the View renders the flame when heating is ön.
        /// </summary>
        [Fact]
        public void RendersFlameOn()
        {
            var cut = this.testContext.RenderComponent<Boiler>();
            var powerService = this.testContext.Services.GetService<IPowerService>();
            powerService.PowerOn(CancellationToken.None);

            cut.WaitForAssertion(() =>
            {
                cut.Find(".fa-burn").MarkupMatches(@"<i class=""fas fa-burn text-danger""></i>");
            });
        }

        /// <summary>
        /// Checks that the View renders the target pressure adjustment controls.
        /// </summary>
        [Fact]
        public void RendersPressureAdjustment()
        {
            var cut = this.testContext.RenderComponent<Boiler>();
            var input = cut.Find("div:has(label) input");
            input.GetAttribute("value").Should().Be("1.2");
        }

        /// <summary>
        /// Checks that the View can alter the target pressure.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task AdjustsTargetPressure()
        {
            var actual = 0.0m;

            var s = this.testContext.Services.GetService<IBoilerService>();
            s.TargetPressure.Subscribe(p => actual = p);
            var cut = this.testContext.RenderComponent<Boiler>();
            var input = cut.Find("div:has(label) input");

            input.Change(new ChangeEventArgs()
            {
                Value = "1.3",
            });

            // await debounce.
            await Task.Delay(2000);
            actual.Should().Be(1.3m);
        }
    }
}
