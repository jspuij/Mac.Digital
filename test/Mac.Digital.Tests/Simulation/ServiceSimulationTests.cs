// <copyright file="ServiceSimulationTests.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Tests.Simulation
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Mac.Digital.Simulation;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Unit tests for the <see cref="ServiceSimulation"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ServiceSimulationTests
    {
        private readonly ServiceSimulation testClass;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceSimulationTests"/> class.
        /// </summary>
        /// <param name="output">The output parameter.</param>
        public ServiceSimulationTests(ITestOutputHelper output)
        {
            this.testClass = new ServiceSimulation();
        }

        /// <summary>
        /// Can Construct an instance of the simulation model.
        /// </summary>
        [Fact]
        public void CanConstruct()
        {
            var instance = new ServiceSimulation();
            instance.Should().NotBeNull();
        }

        /// <summary>
        /// Can Construct an instance of the simulation model with arguments.
        /// </summary>
        [Fact]
        public void CanConstructWithArguments()
        {
            var instance = new ServiceSimulation(
                1000,
                false,
                0m,
                1.3m,
                0.1m,
                0.1m,
                true);
            instance.Should().NotBeNull();
        }

        /// <summary>
        /// Can call dispose.
        /// </summary>
        [Fact]
        public void CanCallDispose()
        {
            Action act = () => this.testClass.Dispose();
            act.Should().NotThrow();
        }

        /// <summary>
        /// Can call PowerOff.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CanCallPowerOff()
        {
            bool powerOffCalled = false;

            var cancellationToken = CancellationToken.None;

            var instance = new ServiceSimulation(
             1000,
             true,
             0m,
             1.3m,
             0.0m,
             0.0m,
             false);

            instance.PoweredOn.Subscribe(x =>
            {
                if (!x)
                {
                    powerOffCalled = true;
                }
            });
            await instance.PowerOff(cancellationToken);
            powerOffCalled.Should().BeTrue();
        }

        /// <summary>
        /// Can call PowerOn.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CanCallPowerOn()
        {
            bool powerOnCalled = false;

            var cancellationToken = CancellationToken.None;
            this.testClass.PoweredOn.Subscribe(x =>
            {
                if (x)
                {
                    powerOnCalled = true;
                }
            });
            await this.testClass.PowerOn(cancellationToken);
            powerOnCalled.Should().BeTrue();
        }

        /// <summary>
        /// Can call reset protection.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CanCallResetProtection()
        {
            bool resetCalled = false;

            var cancellationToken = CancellationToken.None;

            var instance = new ServiceSimulation(
               1000,
               true,
               0m,
               1.3m,
               0.0m,
               0.0m,
               true);

            instance.Protection.Subscribe(x =>
            {
                if (!x)
                {
                    resetCalled = true;
                }
            });

            await instance.ResetProtection(cancellationToken);
            resetCalled.Should().BeTrue();
        }

        /// <summary>
        /// Cannot call reset protection when temperature is still to high.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CannotCallResetProtectionTooHighTemperature()
        {
            bool resetCalled = false;

            var cancellationToken = CancellationToken.None;

            var instance = new ServiceSimulation(
               1000,
               true,
               140m,
               1.3m,
               0.0m,
               0.0m,
               true);

            instance.Protection.Subscribe(x =>
            {
                if (!x)
                {
                    resetCalled = true;
                }
            });

            Func<Task> act = () => instance.ResetProtection(cancellationToken);

            await act.Should().ThrowAsync<BoilerException>();
            resetCalled.Should().BeFalse();
        }

        /// <summary>
        /// Can call SetPressureOffset.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CanCallSetPressureOffset()
        {
            decimal actualPressure = 0m;
            decimal actualOffset = 0m;

            this.testClass.Pressure.Subscribe(p => actualPressure = p);
            this.testClass.PressureOffset.Subscribe(o => actualOffset = o);

            var targetOffset = 0.5m;
            var cancellationToken = CancellationToken.None;

            await this.testClass.SetPressureOffset(targetOffset, cancellationToken);

            actualPressure.Should().Be(targetOffset);
            actualOffset.Should().Be(targetOffset);
        }

        /// <summary>
        /// Cannot call SetPressureOffset with a too low value.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CannotCallSetPressureOffsetTooLow()
        {
            bool offsetCalled;

            this.testClass.Pressure.Subscribe(_ => offsetCalled = true);

            // reset offsetCalled because it's a behaviorsubject that will always trigger.
            offsetCalled = false;

            Func<Task> act = () => this.testClass.SetPressureOffset(-1m, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
            offsetCalled.Should().BeFalse();
        }

        /// <summary>
        /// Cannot call SetPressureOffset with a too high value.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CannotCallSetPressureOffsetTooHigh()
        {
            bool offsetCalled;
            this.testClass.Pressure.Subscribe(_ => offsetCalled = true);

            // reset offsetCalled because it's a behaviorsubject that will always trigger.
            offsetCalled = false;

            Func<Task> act = () => this.testClass.SetPressureOffset(1m, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
            offsetCalled.Should().BeFalse();
        }

        /// <summary>
        /// Can call set target pressure.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CanCallSetTargetPressure()
        {
            decimal actualTargetPressure = 0m;
            var targetPressure = 1.4m;

            this.testClass.TargetPressure.Subscribe(tp => actualTargetPressure = tp);

            await this.testClass.SetTargetPressure(targetPressure, CancellationToken.None);
            actualTargetPressure.Should().Be(targetPressure);
        }

        /// <summary>
        /// Cannot call SetTargetPressure with a too low argument.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CannotCallSetTargetPressureTooLow()
        {
            bool targetPressureCalled;

            this.testClass.Pressure.Subscribe(_ => targetPressureCalled = true);

            // reset offsetCalled because it's a behaviorsubject that will always trigger.
            targetPressureCalled = false;

            Func<Task> act = () => this.testClass.SetTargetPressure(0m, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
            targetPressureCalled.Should().BeFalse();
        }

        /// <summary>
        /// Cannot call SetTargetPressure with a too high argument.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CannotCallSetTargetPressureTooHigh()
        {
            bool targetPressureCalled;

            this.testClass.Pressure.Subscribe(_ => targetPressureCalled = true);

            // reset offsetCalled because it's a behaviorsubject that will always trigger.
            targetPressureCalled = false;

            Func<Task> act = () => this.testClass.SetTargetPressure(2m, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
            targetPressureCalled.Should().BeFalse();
        }

        /// <summary>
        /// Can call SetTemperatureOffset.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CanCallSetTemperatureOffset()
        {
            decimal actualOffset = 0m;

            this.testClass.TemperatureOffset.Subscribe(o => actualOffset = o);

            var targetOffset = 0.5m;
            var cancellationToken = CancellationToken.None;

            await this.testClass.SetTemperatureOffset(targetOffset, cancellationToken);
            actualOffset.Should().Be(targetOffset);
        }

        /// <summary>
        /// Cannot call SetTemperatureOffset with a too low value.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CannotCallSetTemperatureOffsetTooLow()
        {
            bool offsetCalled;

            this.testClass.Temperature.Subscribe(_ => offsetCalled = true);

            // reset offsetCalled because it's a behaviorsubject that will always trigger.
            offsetCalled = false;

            Func<Task> act = () => this.testClass.SetTemperatureOffset(-5m, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
            offsetCalled.Should().BeFalse();
        }

        /// <summary>
        /// Cannot call SetTemperatureOffset with a too high value.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CannotCallSetTemperatureOffsetTooHigh()
        {
            bool offsetCalled;
            this.testClass.Temperature.Subscribe(_ => offsetCalled = true);

            // reset offsetCalled because it's a behaviorsubject that will always trigger.
            offsetCalled = false;

            Func<Task> act = () => this.testClass.SetTemperatureOffset(5m, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
            offsetCalled.Should().BeFalse();
        }

        /// <summary>
        /// Can get the power in watts.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CanGetPowerInWatts()
        {
            decimal powerInWatts = 0m;

            this.testClass.PowerInWatts.Subscribe(p => powerInWatts = p);

            // pre-check
            powerInWatts.Should().BeInRange(0m, 2m);

            // test
            await this.testClass.PowerOn(CancellationToken.None);

            // wait a few seconds for the heater to turn on.
            await Task.Delay(3000);

            // assert
            powerInWatts.Should().BeInRange(1700m, 1900m);
        }

        /*



                [Fact]
                public void CanGetPressure()
                {
                    Assert.IsType<IObservable<decimal>>(testClass.Pressure);
                    Assert.True(false, "Create or modify test");
                }

                [Fact]
                public void CanGetPressureOffset()
                {
                    Assert.IsType<IObservable<decimal>>(testClass.PressureOffset);
                    Assert.True(false, "Create or modify test");
                }

                [Fact]
                public void CanGetTargetPressure()
                {
                    Assert.IsType<IObservable<decimal>>(testClass.TargetPressure);
                    Assert.True(false, "Create or modify test");
                }

                [Fact]
                public void CanGetTemperature()
                {
                    Assert.IsType<IObservable<decimal>>(testClass.Temperature);
                    Assert.True(false, "Create or modify test");
                }

                [Fact]
                public void CanGetTemperatureOffset()
                {
                    Assert.IsType<IObservable<decimal>>(testClass.TemperatureOffset);
                    Assert.True(false, "Create or modify test");
                }

                [Fact]
                public void CanGetProtection()
                {
                    Assert.IsType<IObservable<bool>>(testClass.Protection);
                    Assert.True(false, "Create or modify test");
                }

                [Fact]
                public void CanGetHeating()
                {
                    Assert.IsType<IObservable<bool>>(testClass.Heating);
                    Assert.True(false, "Create or modify test");
                }
        */
    }
}