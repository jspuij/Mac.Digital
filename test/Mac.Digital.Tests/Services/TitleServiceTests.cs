// <copyright file="TitleServiceTests.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Tests.Services
{
    using System;
    using FluentAssertions;
    using Mac.Digital.Services;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="TitleService"/> class.
    /// </summary>
    public class TitleServiceTests
    {
        private TitleService testClass;

        /// <summary>
        /// Initializes a new instance of the <see cref="TitleServiceTests"/> class.
        /// </summary>
        public TitleServiceTests()
        {
            this.testClass = new TitleService();
        }

        /// <summary>
        /// Can construct the TitleService class.
        /// </summary>
        [Fact]
        public void CanConstruct()
        {
            var instance = new TitleService();
            instance.Should().NotBeNull();
        }

        /// <summary>
        /// Can call the SetTitleMethod.
        /// </summary>
        [Fact]
        public void CanCallSetTitleAndCanGetTitle()
        {
            string actual = null;

            var title = "TestValue1470170382";

            this.testClass.Title.Subscribe(t => actual = t);
            this.testClass.SetTitle(title);
            actual.Should().Be(title);
        }

        /// <summary>
        /// Cannot call SetTitle with an Invalid Title.
        /// </summary>
        /// <param name="value">An invalid title value.</param>
        [Theory]
        [InlineData(null)]
        public void CannotCallSetTitleWithInvalidTitle(string value)
        {
            Action act = () => this.testClass.SetTitle(value);
            act.Should().Throw<ArgumentNullException>();
        }
    }
}