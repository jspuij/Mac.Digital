// <copyright file="MachineOffCheckViewModelTests.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Tests.ViewModels
{
    using System;
    using System.Reactive;
    using System.Reactive.Subjects;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using FluentAssertions;
    using Mac.Digital.Policies;
    using Mac.Digital.Services;
    using Mac.Digital.ViewModels;
    using Moq;
    using Polly;
    using ReactiveUI;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="MachineOffCheckViewModel"/> class.
    /// </summary>
    public class MachineOffCheckViewModelTests
    {
        private readonly Mock<IPowerService> powerService;
        private readonly Mock<ICommandPolicyProvider> policyProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MachineOffCheckViewModelTests"/> class.
        /// </summary>
        public MachineOffCheckViewModelTests()
        {
            this.powerService = new Mock<IPowerService>();

            var subject = new BehaviorSubject<bool>(false);
            this.powerService.Setup(x => x.PoweredOn).Returns(subject);
            this.powerService.Setup(x => x.PowerOn(It.IsAny<CancellationToken>())).Returns(async () =>
            {
                await Task.Delay(100);
                subject.OnNext(true);
            });
            this.powerService.Setup(x => x.PowerOff(It.IsAny<CancellationToken>())).Returns(async () =>
            {
                await Task.Delay(100);
                subject.OnNext(false);
            });

            this.policyProvider = new Mock<ICommandPolicyProvider>();
            this.policyProvider.Setup(x => x.GetPolicy()).Returns(Policy.TimeoutAsync(1));
        }

        /// <summary>
        /// Can construct a new instance of the <see cref="HeaderViewModel"/> class.
        /// </summary>
        [Fact]
        public void CanConstruct()
        {
            var instance = new MachineOffCheckViewModel(
                this.powerService.Object,
                this.policyProvider.Object);
            instance.Should().NotBeNull();
        }

        /// <summary>
        /// Cannot construct with a null <see cref="IPowerService" /> instance.
        /// </summary>
        [Fact]
        public void CannotConstructWithNullPowerService()
        {
            Action act = () => new MachineOffCheckViewModel(
                default(IPowerService),
                this.policyProvider.Object);
            act.Should().Throw<ArgumentNullException>();
        }

        /// <summary>
        /// Cannot construct with a null <see cref="ICommandPolicyProvider" /> instance.
        /// </summary>
        [Fact]
        public void CannotConstructWithNullPolicyProvider()
        {
            Action act = () => new MachineOffCheckViewModel(
                     this.powerService.Object,
                     default(ICommandPolicyProvider));
            act.Should().Throw<ArgumentNullException>();
        }

        /// <summary>
        /// Can get the PoweredOn value.
        /// </summary>
        [Fact]
        public void CanGetPoweredOn()
        {
            var subject = new BehaviorSubject<bool>(false);
            this.powerService.Setup(x => x.PoweredOn).Returns(subject);
            var instance = new MachineOffCheckViewModel(
                this.powerService.Object,
                this.policyProvider.Object);
            instance.Activator.Activate();

            subject.OnNext(true);
            instance.PoweredOn.Should().BeTrue();
        }

        /// <summary>
        /// Can turn on the power.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CanTurnOnPower()
        {
            var instance = new MachineOffCheckViewModel(
                this.powerService.Object,
                this.policyProvider.Object);
            instance.Activator.Activate();

            ((ICommand)instance.TurnOn).Execute(null);
            await Task.Delay(200);
            instance.PoweredOn.Should().BeTrue();
        }

        /// <summary>
        /// Cannot turn off the power.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CannotTurnOffPower()
        {
            var instance = new MachineOffCheckViewModel(
                this.powerService.Object,
                this.policyProvider.Object);
            instance.Activator.Activate();

            ((ICommand)instance.TurnOn).Execute(null);
            await Task.Delay(200);
            ((ICommand)instance.TurnOn).CanExecute(null).Should().BeFalse();
        }

        /// <summary>
        /// Toggle will time out through the policy.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TurnOnWillTimeout()
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

            var instance = new MachineOffCheckViewModel(
                this.powerService.Object,
                this.policyProvider.Object);
            instance.Activator.Activate();

            ((ICommand)instance.TurnOn).Execute(null);

            await Task.Delay(2000);

            cancelled.Should().BeTrue();
        }

        /// <summary>
        /// Has default values when not activated yet.
        /// </summary>
        [Fact]
        public void HasDefaultValues()
        {
            var instance = new MachineOffCheckViewModel(
               this.powerService.Object,
               this.policyProvider.Object);

            instance.PoweredOn.Should().BeFalse();
            instance.CanExecuteTurnOn.Should().BeFalse();
        }
    }
}