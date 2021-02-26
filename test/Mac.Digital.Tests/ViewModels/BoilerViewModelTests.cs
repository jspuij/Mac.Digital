// <copyright file="BoilerViewModelTests.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Tests.ViewModels
{
    using System;
    using System.Reactive.Subjects;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Mac.Digital;
    using Mac.Digital.Policies;
    using Mac.Digital.Services;
    using Mac.Digital.ViewModels;
    using Moq;
    using Polly;
    using ReactiveUI;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="BoilerViewModel"/> class.
    /// </summary>
    public class BoilerViewModelTests
    {
        private Mock<IBoilerService> boilerService;
        private Mock<ICommandPolicyProvider> policyProvider;
        private BehaviorSubject<decimal> pressure;
        private BehaviorSubject<decimal> targetPressure;
        private BehaviorSubject<decimal> temperature;
        private BehaviorSubject<bool> heating;
        private BehaviorSubject<bool> protection;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoilerViewModelTests"/> class.
        /// </summary>
        public BoilerViewModelTests()
        {
            this.boilerService = new Mock<IBoilerService>();
            this.policyProvider = new Mock<ICommandPolicyProvider>();
            this.policyProvider.Setup(x => x.GetPolicy()).Returns(Policy.TimeoutAsync(1));
            this.pressure = new BehaviorSubject<decimal>(0m);
            this.targetPressure = new BehaviorSubject<decimal>(0m);
            this.temperature = new BehaviorSubject<decimal>(0m);
            this.heating = new BehaviorSubject<bool>(false);
            this.protection = new BehaviorSubject<bool>(false);

            this.boilerService.Setup(x => x.Pressure).Returns(this.pressure);
            this.boilerService.Setup(x => x.TargetPressure).Returns(this.targetPressure);
            this.boilerService.Setup(x => x.Temperature).Returns(this.temperature);
            this.boilerService.Setup(x => x.Heating).Returns(this.heating);
            this.boilerService.Setup(x => x.Protection).Returns(this.protection);
            this.boilerService
                .Setup(x => x.SetTargetPressure(It.IsAny<decimal>(), It.IsAny<CancellationToken>()))
                .Returns<decimal, CancellationToken>((d, c) =>
                {
                    this.targetPressure.OnNext(d);
                    return Task.CompletedTask;
                });
        }

        /// <summary>
        /// Can construct an instance of the ViewModel.
        /// </summary>
        [Fact]
        public void CanConstruct()
        {
            var instance = new BoilerViewModel(this.boilerService.Object, this.policyProvider.Object);
            Assert.NotNull(instance);
        }

        /// <summary>
        /// Cannot construct with null boiler service.
        /// </summary>
        [Fact]
        public void CannotConstructWithNullBoilerService()
        {
            Assert.Throws<ArgumentNullException>(() => new BoilerViewModel(default(IBoilerService), new Mock<ICommandPolicyProvider>().Object));
        }

        /// <summary>
        /// Cannot construct with null policy provider.
        /// </summary>
        [Fact]
        public void CannotConstructWithNullPolicyProvider()
        {
            Assert.Throws<ArgumentNullException>(() => new BoilerViewModel(new Mock<IBoilerService>().Object, default(ICommandPolicyProvider)));
        }

        /// <summary>
        /// Can get the pressure observable.
        /// </summary>
        [Fact]
        public void CanGetPressure()
        {
            var expected = 1.5m;
            this.pressure.OnNext(expected);

            var instance = new BoilerViewModel(this.boilerService.Object, this.policyProvider.Object);
            instance.Activator.Activate();

            instance.Pressure.Should().Be(expected);
        }

        /// <summary>
        /// Can get and set the target pressure.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CanSetAndGetTargetPressure()
        {
            var expected = 1.2m;
            var set = 0m;

            this.targetPressure.Subscribe(x => set = x);

            var instance = new BoilerViewModel(this.boilerService.Object, this.policyProvider.Object);
            instance.Activator.Activate();

            instance.TargetPressure = expected;

            instance.TargetPressure.Should().Be(expected, "TargetPressure should immediately reflect set value.");
            set.Should().Be(0, "TargetPressure did not debounce.");

            await Task.Delay(2000);
            set.Should().Be(expected, "TargetPressure did not propagate.");
        }

        /// <summary>
        /// Can get the temperature.
        /// </summary>
        [Fact]
        public void CanGetTemperature()
        {
            var expected = 123m;
            this.temperature.OnNext(expected);

            var instance = new BoilerViewModel(this.boilerService.Object, this.policyProvider.Object);
            instance.Activator.Activate();

            instance.Temperature.Should().Be(expected);
        }

        /// <summary>
        /// Can get the temperature trend.
        /// </summary>
        [Fact]
        public void CanGetTemperatureTrend()
        {
            var instance = new BoilerViewModel(this.boilerService.Object, this.policyProvider.Object);
            instance.Activator.Activate();

            // set twice
            this.temperature.OnNext(25m);
            this.temperature.OnNext(25m);

            instance.TemperatureTrend.Should().Be(Trend.Stable, "2 consecutive values, trend should be stable.");

            this.temperature.OnNext(26m);
            instance.TemperatureTrend.Should().Be(Trend.Rising, "Trend should be rising.");

            this.temperature.OnNext(24m);
            instance.TemperatureTrend.Should().Be(Trend.Falling, "Trend should be falling.");
        }

        /// <summary>
        /// Can Get protection.
        /// </summary>
        [Fact]
        public void CanGetProtection()
        {
            var expected = true;
            this.protection.OnNext(expected);

            var instance = new BoilerViewModel(this.boilerService.Object, this.policyProvider.Object);
            instance.Activator.Activate();

            instance.Protection.Should().Be(expected);
        }

        /// <summary>
        /// Can get the heating value.
        /// </summary>
        [Fact]
        public void CanGetHeating()
        {
            var expected = true;
            this.heating.OnNext(expected);

            var instance = new BoilerViewModel(this.boilerService.Object, this.policyProvider.Object);
            instance.Activator.Activate();

            instance.Heating.Should().Be(expected);
        }

        /// <summary>
        /// Has default values when not activated yet.
        /// </summary>
        [Fact]
        public void HasDefaultValues()
        {
            var instance = new BoilerViewModel(this.boilerService.Object, this.policyProvider.Object);

            instance.Heating.Should().BeFalse();
            instance.Protection.Should().BeFalse();
            instance.Temperature.Should().Be(0m);
            instance.Pressure.Should().Be(0m);
            instance.TargetPressure.Should().Be(0m);
            instance.TemperatureTrend.Should().Be(Trend.Stable);
        }
    }
}