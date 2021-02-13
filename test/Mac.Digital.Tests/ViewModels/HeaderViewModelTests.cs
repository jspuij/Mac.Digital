// <copyright file="HeaderViewModelTests.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Tests.ViewModels
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reactive;
    using System.Reactive.Subjects;
    using FluentAssertions;
    using Mac.Digital.Policies;
    using Mac.Digital.Services;
    using Mac.Digital.ViewModels;
    using Microsoft.AspNetCore.Components;
    using Moq;
    using Polly;
    using ReactiveUI;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="HeaderViewModel"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class HeaderViewModelTests
    {
        private readonly Mock<IPowerService> powerService;
        private readonly Mock<ITitleService> titleService;
        private readonly Mock<ICommandPolicyProvider> policyProvider;
        private readonly Mock<NavigationManager> navigationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderViewModelTests"/> class.
        /// </summary>
        public HeaderViewModelTests()
        {
            this.powerService = new Mock<IPowerService>();
            this.powerService.Setup(x => x.PoweredOn).Returns(new BehaviorSubject<bool>(false));
            this.titleService = new Mock<ITitleService>();
            this.titleService.Setup(x => x.Title).Returns(new BehaviorSubject<string>("Test"));
            this.policyProvider = new Mock<ICommandPolicyProvider>();
            this.policyProvider.Setup(x => x.GetPolicy()).Returns(Policy.TimeoutAsync(3));
            this.navigationManager = new Mock<NavigationManager>();
        }

        /// <summary>
        /// Can construct a new instance of the <see cref="HeaderViewModel"/> class.
        /// </summary>
        [Fact]
        public void CanConstruct()
        {
            var instance = new HeaderViewModel(
                this.powerService.Object,
                this.titleService.Object,
                this.policyProvider.Object,
                this.navigationManager.Object);
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
                this.titleService.Object,
                this.policyProvider.Object,
                this.navigationManager.Object);

            act.Should().Throw<ArgumentNullException>();
        }

        /// <summary>
        /// Cannot construct with a null title service.
        /// </summary>
        [Fact]
        public void CannotConstructWithNullTitleService()
        {
            Action act = () => new HeaderViewModel(
                this.powerService.Object,
                default,
                this.policyProvider.Object,
                this.navigationManager.Object);

            act.Should().Throw<ArgumentNullException>();
        }

        /// <summary>
        /// Cannot construct wuth a null policy provider.
        /// </summary>
        [Fact]
        public void CannotConstructWithNullPolicyProvider()
        {
            Action act = () => new HeaderViewModel(
                this.powerService.Object,
                this.titleService.Object,
                default,
                this.navigationManager.Object);

            act.Should().Throw<ArgumentNullException>();
        }

        /// <summary>
        /// Cannot construct with a null navigation manager.
        /// </summary>
        [Fact]
        public void CannotConstructWithNullNavigationManager()
        {
            Action act = () => new HeaderViewModel(
                this.powerService.Object,
                this.titleService.Object,
                this.policyProvider.Object,
                default);

            act.Should().Throw<ArgumentNullException>();
        }

        /// <summary>
        /// Can call dispose.
        /// </summary>
        [Fact]
        public void CanCallDispose()
        {
            var instance = new HeaderViewModel(
             this.powerService.Object,
             this.titleService.Object,
             this.policyProvider.Object,
             this.navigationManager.Object);
            Action act = () => instance.Dispose();
            act.Should().NotThrow();
        }

        /// <summary>
        /// Can get the PoweredOn variable.
        /// </summary>
        [Fact]
        public void CanGetPoweredOn()
        {
            var subject = new BehaviorSubject<bool>(false);
            this.powerService.Setup(x => x.PoweredOn).Returns(subject);
            var instance = new HeaderViewModel(
               this.powerService.Object,
               this.titleService.Object,
               this.policyProvider.Object,
               this.navigationManager.Object);
            subject.OnNext(true);
            instance.PoweredOn.Should().BeTrue();
        }

        /*
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