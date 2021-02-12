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
    using Nito.AsyncEx;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// Unit tests for the <see cref="ServiceSimulation"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ServiceSimulationTests
    {
        /// <summary>
        /// Can Construct an instance of the simulation model.
        /// </summary>
        [Fact]
        public void CanConstruct()
        {
            AsyncContext.Run(() =>
            {
                var instance = new ServiceSimulation(SynchronizationContext.Current);
                instance.Should().NotBeNull();
            });
        }

        /// <summary>
        /// Can Construct an instance of the simulation model with arguments.
        /// </summary>
        [Fact]
        public void CanConstructWithArguments()
        {
            AsyncContext.Run(() =>
            {
                var instance = new ServiceSimulation(
                    SynchronizationContext.Current,
                    1000,
                    false,
                    0m,
                    1.3m,
                    0.1m,
                    0.1m,
                    true);
                instance.Should().NotBeNull();
            });
        }

        /// <summary>
        /// Can call dispose.
        /// </summary>
        [Fact]
        public void CanCallDispose()
        {
            AsyncContext.Run(() =>
            {
                var instance = new ServiceSimulation(SynchronizationContext.Current);
                Action act = () => instance.Dispose();
                act.Should().NotThrow();
            });
        }

        /// <summary>
        /// Can call PowerOff.
        /// </summary>
        [Fact]
        public void CanCallPowerOff()
        {
            AsyncContext.Run(async () =>
            {
                bool powerOffCalled = false;
                var cancellationToken = CancellationToken.None;

                var instance = new ServiceSimulation(
                    SynchronizationContext.Current,
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
            });
        }

        /// <summary>
        /// Can call PowerOn.
        /// </summary>
        [Fact]
        public void CanCallPowerOn()
        {
            AsyncContext.Run(async () =>
            {
                bool powerOnCalled = false;
                var cancellationToken = CancellationToken.None;

                var instance = new ServiceSimulation(SynchronizationContext.Current);

                instance.PoweredOn.Subscribe(x =>
                {
                    if (x)
                    {
                        powerOnCalled = true;
                    }
                });
                await instance.PowerOn(cancellationToken);
                powerOnCalled.Should().BeTrue();
            });
        }

        /// <summary>
        /// Can call reset protection.
        /// </summary>
        [Fact]
        public void CanCallResetProtection()
        {
            AsyncContext.Run(async () =>
            {
                bool resetCalled = false;

                var cancellationToken = CancellationToken.None;

                var instance = new ServiceSimulation(
                    SynchronizationContext.Current,
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
            });
        }

        /// <summary>
        /// Cannot call reset protection when temperature is still to high.
        /// </summary>
        [Fact]
        public void CannotCallResetProtectionTooHighTemperature()
        {
            AsyncContext.Run(async () =>
            {
                bool resetCalled = false;
                var cancellationToken = CancellationToken.None;

                var instance = new ServiceSimulation(
                    SynchronizationContext.Current,
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
            });
        }

        /// <summary>
        /// Can call SetPressureOffset.
        /// </summary>
        [Fact]
        public void CanCallSetPressureOffset()
        {
            AsyncContext.Run(async () =>
            {
                var instance = new ServiceSimulation(SynchronizationContext.Current);

                decimal actualPressure = 0m;
                decimal actualOffset = 0m;

                instance.Pressure.Subscribe(p => actualPressure = p);
                instance.PressureOffset.Subscribe(o => actualOffset = o);

                var targetOffset = 0.5m;
                var cancellationToken = CancellationToken.None;

                await instance.SetPressureOffset(targetOffset, cancellationToken);

                actualPressure.Should().Be(targetOffset);
                actualOffset.Should().Be(targetOffset);
            });
        }

        /// <summary>
        /// Cannot call SetPressureOffset with a too low value.
        /// </summary>
        [Fact]
        public void CannotCallSetPressureOffsetTooLow()
        {
            AsyncContext.Run(async () =>
            {
                bool offsetCalled;

                var instance = new ServiceSimulation(SynchronizationContext.Current);

                instance.Pressure.Subscribe(_ => offsetCalled = true);

                // reset offsetCalled because it's a behaviorsubject that will always trigger.
                offsetCalled = false;

                Func<Task> act = () => instance.SetPressureOffset(-1m, CancellationToken.None);

                await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
                offsetCalled.Should().BeFalse();
            });
        }

        /// <summary>
        /// Cannot call SetPressureOffset with a too high value.
        /// </summary>
        [Fact]
        public void CannotCallSetPressureOffsetTooHigh()
        {
            AsyncContext.Run(async () =>
            {
                bool offsetCalled;

                var instance = new ServiceSimulation(SynchronizationContext.Current);

                instance.Pressure.Subscribe(_ => offsetCalled = true);

                // reset offsetCalled because it's a behaviorsubject that will always trigger.
                offsetCalled = false;

                Func<Task> act = () => instance.SetPressureOffset(1m, CancellationToken.None);

                await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
                offsetCalled.Should().BeFalse();
            });
        }

        /// <summary>
        /// Can call set target pressure.
        /// </summary>
        [Fact]
        public void CanCallSetTargetPressure()
        {
            AsyncContext.Run(async () =>
            {
                decimal actualTargetPressure = 0m;
                var targetPressure = 1.4m;

                var instance = new ServiceSimulation(SynchronizationContext.Current);

                instance.TargetPressure.Subscribe(tp => actualTargetPressure = tp);

                await instance.SetTargetPressure(targetPressure, CancellationToken.None);
                actualTargetPressure.Should().Be(targetPressure);
            });
        }

        /// <summary>
        /// Cannot call SetTargetPressure with a too low argument.
        /// </summary>
        [Fact]
        public void CannotCallSetTargetPressureTooLow()
        {
            AsyncContext.Run(async () =>
            {
                bool targetPressureCalled;
                var instance = new ServiceSimulation(SynchronizationContext.Current);

                instance.Pressure.Subscribe(_ => targetPressureCalled = true);

                // reset offsetCalled because it's a behaviorsubject that will always trigger.
                targetPressureCalled = false;

                Func<Task> act = () => instance.SetTargetPressure(0m, CancellationToken.None);

                await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
                targetPressureCalled.Should().BeFalse();
            });
        }

        /// <summary>
        /// Cannot call SetTargetPressure with a too high argument.
        /// </summary>
        [Fact]
        public void CannotCallSetTargetPressureTooHigh()
        {
            AsyncContext.Run(async () =>
            {
                bool targetPressureCalled;
                var instance = new ServiceSimulation(SynchronizationContext.Current);

                instance.Pressure.Subscribe(_ => targetPressureCalled = true);

                // reset offsetCalled because it's a behaviorsubject that will always trigger.
                targetPressureCalled = false;

                Func<Task> act = () => instance.SetTargetPressure(2m, CancellationToken.None);

                await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
                targetPressureCalled.Should().BeFalse();
            });
        }

        /// <summary>
        /// Can call SetTemperatureOffset.
        /// </summary>
        [Fact]
        public void CanCallSetTemperatureOffset()
        {
            AsyncContext.Run(async () =>
            {
                var instance = new ServiceSimulation(SynchronizationContext.Current);

                decimal actualOffset = 0m;

                instance.TemperatureOffset.Subscribe(o => actualOffset = o);

                var targetOffset = 0.5m;
                var cancellationToken = CancellationToken.None;

                await instance.SetTemperatureOffset(targetOffset, cancellationToken);
                actualOffset.Should().Be(targetOffset);
            });
        }

        /// <summary>
        /// Cannot call SetTemperatureOffset with a too low value.
        /// </summary>
        [Fact]
        public void CannotCallSetTemperatureOffsetTooLow()
        {
            AsyncContext.Run(async () =>
            {
                bool offsetCalled;
                var instance = new ServiceSimulation(SynchronizationContext.Current);

                instance.Temperature.Subscribe(_ => offsetCalled = true);

                // reset offsetCalled because it's a behaviorsubject that will always trigger.
                offsetCalled = false;

                Func<Task> act = () => instance.SetTemperatureOffset(-5m, CancellationToken.None);

                await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
                offsetCalled.Should().BeFalse();
            });
        }

        /// <summary>
        /// Cannot call SetTemperatureOffset with a too high value.
        /// </summary>
        [Fact]
        public void CannotCallSetTemperatureOffsetTooHigh()
        {
            AsyncContext.Run(async () =>
            {
                bool offsetCalled;
                var instance = new ServiceSimulation(SynchronizationContext.Current);
                instance.Temperature.Subscribe(_ => offsetCalled = true);

                // reset offsetCalled because it's a behaviorsubject that will always trigger.
                offsetCalled = false;

                Func<Task> act = () => instance.SetTemperatureOffset(5m, CancellationToken.None);

                await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
                offsetCalled.Should().BeFalse();
            });
        }

        /// <summary>
        /// Can get the power in watts.
        /// </summary>
        [Fact]
        public void CanGetPowerInWatts()
        {
            AsyncContext.Run(async () =>
            {
                var instance = new ServiceSimulation(SynchronizationContext.Current);
                decimal powerInWatts = 0m;

                instance.PowerInWatts.Subscribe(p => powerInWatts = p);

                // pre-check
                powerInWatts.Should().BeInRange(0m, 2m);

                // test
                await instance.PowerOn(CancellationToken.None);

                // wait at least a tick for the heater to turn on.
                await Task.Delay(1200);

                // assert
                powerInWatts.Should().BeInRange(1700m, 1900m);
            });
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