// <copyright file="HeaderViewModelTests.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Tests.ViewModels
{
    using System;
    using System.Reactive;
    using FluentAssertions;
    using Mac.Digital.Policies;
    using Mac.Digital.Services;
    using Mac.Digital.ViewModels;
    using Microsoft.AspNetCore.Components;
    using Moq;
    using ReactiveUI;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="HeaderViewModel"/> class.
    /// </summary>
    public class HeaderViewModelTests
    {
        private readonly HeaderViewModel testClass;
        private readonly IPowerService powerService;
        private readonly ITitleService titleService;
        private readonly ICommandPolicyProvider policyProvider;
        private readonly NavigationManager navigationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderViewModelTests"/> class.
        /// </summary>
        public HeaderViewModelTests()
        {
            this.powerService = new Mock<IPowerService>().Object;
            this.titleService = new Mock<ITitleService>().Object;
            this.policyProvider = new Mock<ICommandPolicyProvider>().Object;
            this.navigationManager = new Mock<NavigationManager>().Object;
            this.testClass = new HeaderViewModel(
                this.powerService,
                this.titleService,
                this.policyProvider,
                this.navigationManager);
        }

        /// <summary>
        /// Can construct a new instance of the <see cref="HeaderViewModel"/> class.
        /// </summary>
        [Fact]
        public void CanConstruct()
        {
            var instance = new HeaderViewModel(
                this.powerService,
                this.titleService,
                this.policyProvider,
                this.navigationManager);
            instance.Should().NotBeNull();
        }

        /// <summary>
        /// Cannot construct with a null power service.
        /// </summary>
        [Fact]
        public void CannotConstructWithNullPowerService()
        {
            Action act = () => new HeaderViewModel(
                default,
                this.titleService,
                this.policyProvider,
                this.navigationManager);

            act.Should().Throw<ArgumentNullException>();
        }

        /// <summary>
        /// Cannot construct with a null title service.
        /// </summary>
        [Fact]
        public void CannotConstructWithNullTitleService()
        {
            Action act = () => new HeaderViewModel(
                this.powerService,
                default,
                this.policyProvider,
                this.navigationManager);

            act.Should().Throw<ArgumentNullException>();
        }

        /// <summary>
        /// Cannot construct wuth a null policy provider.
        /// </summary>
        [Fact]
        public void CannotConstructWithNullPolicyProvider()
        {
            Action act = () => new HeaderViewModel(
                this.powerService,
                this.titleService,
                default,
                this.navigationManager);

            act.Should().Throw<ArgumentNullException>();
        }

        /// <summary>
        /// Cannot construct with a null navigation manager.
        /// </summary>
        [Fact]
        public void CannotConstructWithNullNavigationManager()
        {
            Action act = () => new HeaderViewModel(
                this.powerService,
                this.titleService,
                this.policyProvider,
                default);

            act.Should().Throw<ArgumentNullException>();
        }

        /// <summary>
        /// Can call dispose.
        /// </summary>
        [Fact]
        public void CanCallDispose()
        {
            this.testClass.Dispose();
            Func<string> act = () => this.testClass.Title;
            act.Should().Throw<ObjectDisposedException>();
        }

        /*
        [Fact]
        public void CanGetPoweredOn()
        {
            Assert.IsType<bool>(test.PoweredOn);
            Assert.True(false, "Create or modify test");
        }

        [Fact]
        public void CanGetTitle()
        {
            Assert.IsType<string>(test.Title);
            Assert.True(false, "Create or modify test");
        }

        [Fact]
        public void CanGetTogglePower()
        {
            Assert.IsType<ReactiveCommand<Unit, Unit>>(test.TogglePower);
            Assert.True(false, "Create or modify test");
        }

        [Fact]
        public void CanGetSettings()
        {
            Assert.IsType<ReactiveCommand<Unit, Unit>>(test.Settings);
            Assert.True(false, "Create or modify test");
        }*/
    }
}