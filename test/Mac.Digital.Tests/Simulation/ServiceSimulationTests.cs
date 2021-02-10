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
        private ServiceSimulation testClass;

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
        /// Can Constructe.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task CanConstruct()
        {
            var instance = new ServiceSimulation();

            instance.PowerInWatts.Subscribe(p => this.output.WriteLine($"Power: {p}"));
            instance.Temperature.Subscribe(t => this.output.WriteLine($"Temperature: {t}"));
            instance.Pressure.Subscribe(p => this.output.WriteLine($"Pressure: {p}"));
            instance.Tick.Subscribe(t => this.output.WriteLine($"Tick: {t}"));

            await instance.PowerOn(CancellationToken.None);
            await Task.Delay(50 * 1000);
            Assert.NotNull(instance);
        }

/*
        [Fact]
        public void CanCallDispose()
        {
            testClass.Dispose();
            Assert.True(false, "Create or modify test");
        }

        [Fact]
        public async Task CanCallPowerOff()
        {
            var cancellationToken = CancellationToken.None;
            await testClass.PowerOff(cancellationToken);
            Assert.True(false, "Create or modify test");
        }

        [Fact]
        public async Task CanCallPowerOn()
        {
            var cancellationToken = CancellationToken.None;
            await testClass.PowerOn(cancellationToken);
            Assert.True(false, "Create or modify test");
        }

        [Fact]
        public async Task CanCallResetProtection()
        {
            var cancellationToken = CancellationToken.None;
            await testClass.ResetProtection(cancellationToken);
            Assert.True(false, "Create or modify test");
        }

        [Fact]
        public async Task CanCallSetPressureOffset()
        {
            var targetOffset = 524142241.92M;
            var cancellationToken = CancellationToken.None;
            await testClass.SetPressureOffset(targetOffset, cancellationToken);
            Assert.True(false, "Create or modify test");
        }

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