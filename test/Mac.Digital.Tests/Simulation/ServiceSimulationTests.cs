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
        private readonly ITestOutputHelper output;
        private readonly ServiceSimulation testClass;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceSimulationTests"/> class.
        /// </summary>
        /// <param name="output">The output parameter.</param>
        public ServiceSimulationTests(ITestOutputHelper output)
        {
            this.testClass = new ServiceSimulation();
            this.output = output;
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

            // make sure it's off.
            await this.testClass.PowerOff(CancellationToken.None);

            this.testClass.Pressure.Subscribe(p => actualPressure = p);
            this.testClass.Pressure.Subscribe(o => actualOffset = o);

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

            // make sure it's off.
            await this.testClass.PowerOff(CancellationToken.None);

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

            // make sure it's off.
            await this.testClass.PowerOff(CancellationToken.None);

            // reset offsetCalled because it's a behaviorsubject that will always trigger.
            offsetCalled = false;

            Func<Task> act = () => this.testClass.SetPressureOffset(1m, CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
            offsetCalled.Should().BeFalse();
        }

        /*




                [Fact]
                public async Task CanCallSetTargetPressure()
                {
                    var targetPressure = 1379525623.74M;
                    var cancellationToken = CancellationToken.None;
                    await testClass.SetTargetPressure(targetPressure, cancellationToken);
                    Assert.True(false, "Create or modify test");
                }

                [Fact]
                public async Task CanCallSetTemperatureOffset()
                {
                    var targetOffset = 939574617.3M;
                    var cancellationToken = CancellationToken.None;
                    await testClass.SetTemperatureOffset(targetOffset, cancellationToken);
                    Assert.True(false, "Create or modify test");
                }

                [Fact]
                public void CanGetPoweredOn()
                {
                    Assert.IsType<IObservable<bool>>(testClass.PoweredOn);
                    Assert.True(false, "Create or modify test");
                }

                [Fact]
                public void CanGetPowerInWatts()
                {
                    Assert.IsType<IObservable<decimal>>(testClass.PowerInWatts);
                    Assert.True(false, "Create or modify test");
                }

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