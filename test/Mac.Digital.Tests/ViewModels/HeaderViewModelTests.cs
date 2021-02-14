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
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using FluentAssertions;
    using Mac.Digital.Policies;
    using Mac.Digital.Services;
    using Mac.Digital.Tests.Helpers;
    using Mac.Digital.ViewModels;
    using Microsoft.AspNetCore.Components;
    using Moq;
    using Moq.Protected;
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
        private readonly TestableNavigationManager navigationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderViewModelTests"/> class.
        /// </summary>
        public HeaderViewModelTests()
        {
            this.powerService = new Mock<IPowerService>();

            var subject = new BehaviorSubject<bool>(false);
            this.powerService.Setup(x => x.PoweredOn).Returns(subject);
            this.powerService.Setup(x => x.PowerOn(It.IsAny<CancellationToken>())).Returns(() =>
            {
                subject.OnNext(true);
                return Task.CompletedTask;
            });
            this.powerService.Setup(x => x.PowerOff(It.IsAny<CancellationToken>())).Returns(() =>
            {
                subject.OnNext(false);
                return Task.CompletedTask;
            });

            this.titleService = new Mock<ITitleService>();
            this.titleService.Setup(x => x.Title).Returns(new BehaviorSubject<string>("Test"));
            this.policyProvider = new Mock<ICommandPolicyProvider>();
            this.policyProvider.Setup(x => x.GetPolicy()).Returns(Policy.TimeoutAsync(1));
            this.navigationManager = new TestableNavigationManager();
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
                this.titleService.Object,
                this.policyProvider.Object,
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
                this.powerService.Object,
                default,
                this.policyProvider.Object,
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
                this.powerService.Object,
                this.titleService.Object,
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
             this.navigationManager);
            Action act = () => instance.Dispose();
            act.Should().NotThrow();
        }

        /// <summary>
        /// Can get the PoweredOn value.
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
               this.navigationManager);
            subject.OnNext(true);
            instance.PoweredOn.Should().BeTrue();
        }

        /// <summary>
        /// Can get the title from the ViewModel.
        /// </summary>
        [Fact]
        public void CanGetTitle()
        {
            var instance = new HeaderViewModel(
               this.powerService.Object,
               this.titleService.Object,
               this.policyProvider.Object,
               this.navigationManager);
            instance.Title.Should().Be("Test");
        }

        /// <summary>
        /// Can toggle the power.
        /// </summary>
        [Fact]
        public void CanTogglePower()
        {
            var instance = new HeaderViewModel(
              this.powerService.Object,
              this.titleService.Object,
              this.policyProvider.Object,
              this.navigationManager);

            ((ICommand)instance.TogglePower).Execute(null);
            instance.PoweredOn.Should().BeTrue();

            ((ICommand)instance.TogglePower).Execute(null);
            instance.PoweredOn.Should().BeFalse();
        }

        /// <summary>
        /// Toggle will time out through the policy.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task ToggleWillTimeout()
        {
            bool cancelled = false;

            this.powerService
                .Setup(x => x.PowerOn(It.IsAny<CancellationToken>()))
                .Returns<CancellationToken>(async (t) =>
                {
                    while (!t.IsCancellationRequested)
                    {
                        await Task.Delay(100);
                    }

                    cancelled = t.IsCancellationRequested;
                });

            var instance = new HeaderViewModel(
              this.powerService.Object,
              this.titleService.Object,
              this.policyProvider.Object,
              this.navigationManager);

            ((ICommand)instance.TogglePower).Execute(null);

            await Task.Delay(2000);

            cancelled.Should().BeTrue();
        }

        /// <summary>
        /// Can execute the settings command.
        /// </summary>
        [Fact]
        public void CanExecuteSettings()
        {
            var instance = new HeaderViewModel(
            this.powerService.Object,
            this.titleService.Object,
            this.policyProvider.Object,
            this.navigationManager);
            ((ICommand)instance.Settings).Execute(null);
            this.navigationManager.LastUri.Should().Be("/Settings");
            this.navigationManager.LastForceLoad.Should().BeFalse();
        }
    }
}