// <copyright file="MachineOffCheckTests.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Tests.WebUI.Shared
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Blazorise;
    using Bunit;
    using FluentAssertions;
    using Mac.Digital.Policies;
    using Mac.Digital.Services;
    using Mac.Digital.Tests.Helpers;
    using Mac.Digital.ViewModels;
    using Mac.Digital.WebUI.Shared;
    using Microsoft.AspNetCore.Components;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="MachineOffCheck"/> component.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class MachineOffCheckTests : TestComponentBase
    {
        /// <summary>
        /// Setup for the RendersCardToTurnMachineOnWhenOff test.
        /// </summary>
        /// <param name="fixture">The fixture to use for the test.</param>
        private void SetupRendersCardToTurnMachineOnWhenOff(Fixture fixture)
        {
            fixture.JSInterop.Mode = JSRuntimeMode.Loose;
            TestContextFixture.SetupDependencyInjection(fixture.Services);
        }

        /// <summary>
        /// Execution of the RendersCardToTurnMachineOnWhenOff test.
        /// </summary>
        /// <param name="fixture">The fixture to use for the test.</param>
        private void RendersCardToTurnMachineOnWhenOff(Fixture fixture)
        {
            var cut = fixture.GetComponentUnderTest<MachineOffCheck>();
            var card = cut.FindComponent<Card>();
            var anchor = card.Find("a.card-link");
            anchor.Attributes.Should().Contain(a => a.Name == "blazor:onclick");
        }

        /// <summary>
        /// Setup for the RendersChildContentWhenMachineOn test.
        /// </summary>
        /// <param name="fixture">The fixture to use for the test.</param>
        private void SetupRendersChildContentWhenMachineOn(Fixture fixture)
        {
            fixture.JSInterop.Mode = JSRuntimeMode.Loose;
            TestContextFixture.SetupDependencyInjection(fixture.Services);
        }

        /// <summary>
        /// Execution of the RendersChildContentWhenMachineOn test.
        /// </summary>
        /// <param name="fixture">The fixture to use for the test.</param>
        private void RendersChildContentWhenMachineOn(Fixture fixture)
        {
            var cut = fixture.GetComponentUnderTest<MachineOffCheck>();
            var card = cut.FindComponent<Card>();
            var anchor = card.Find("a.card-link");
            anchor.Click();
            cut.WaitForAssertion(() => cut.MarkupMatches("<p>This is a test.</p>"));
        }
    }
}