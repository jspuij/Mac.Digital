// <copyright file="BoilerTests.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Tests.WebUI.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Bunit;
    using FluentAssertions;
    using Mac.Digital.Tests.Helpers;
    using Mac.Digital.WebUI.Shared;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="Boiler"/> view.
    /// </summary>
    public class BoilerTests
    {
        private readonly TestContext testContext;

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
    }
}
