// <copyright file="ServiceSimulation.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Simulation
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Mac.Digital.Services;

    /// <summary>
    /// A simulation of all services.
    /// </summary>
    public sealed class ServiceSimulation : IPowerService, IBoilerService, IDisposable
    {
        /// <summary>
        /// The amount of energy neccessary to heat a kg of water 1 °C.
        /// </summary>
        private const decimal JoulesPerKgPerDegreeCelcius = 4168m;

        /// <summary>
        /// Boiler heat loss when idle in °C/s.
        /// </summary>
        private const decimal BoilerHeatLoss = 0.0088666666m;

        /// <summary>
        /// Boiler content in kg.
        /// </summary>
        private const decimal BoilerContent = 5m;

        /// <summary>
        /// Protection temperature in °C.
        /// </summary>
        private const decimal ProtectionTemperature = 135m;

        /// <summary>
        /// The power of the heating element in watts.
        /// </summary>
        private const int HeatingElementWatts = 1800;

        private readonly BehaviorSubject<bool> poweredOn = new BehaviorSubject<bool>(false);
        private readonly BehaviorSubject<decimal> powerInWatts = new BehaviorSubject<decimal>(0);
        private readonly BehaviorSubject<decimal> pressure = new BehaviorSubject<decimal>(0);
        private readonly BehaviorSubject<decimal> targetPressure = new BehaviorSubject<decimal>(1.20m);
        private readonly BehaviorSubject<decimal> temperature;
        private readonly BehaviorSubject<decimal> pressureOffset = new BehaviorSubject<decimal>(0m);
        private readonly BehaviorSubject<decimal> temperatureOffset = new BehaviorSubject<decimal>(0m);
        private readonly BehaviorSubject<bool> protection = new BehaviorSubject<bool>(false);
        private readonly BehaviorSubject<bool> heating = new BehaviorSubject<bool>(false);
        private readonly System.Threading.Timer timer;
        private readonly CancellationTokenSource disposeToken = new CancellationTokenSource();
        private readonly Random random = new Random();
        private readonly BehaviorSubject<int> tick = new BehaviorSubject<int>(0);

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceSimulation"/> class.
        /// </summary>
        public ServiceSimulation()
        {
            var roomTemperature = this.random.Next(195, 215) / 10m;
            var atmosphericPressure = this.random.Next(1000, 1026) / 1000m;

            // random room temperature between 19.5 and 21.5 °C
            this.temperature = new BehaviorSubject<decimal>(roomTemperature);

            // boiler pressure is a function of temperature.
            this.temperature.Select(t => Math.Max(0, TemperaturePressureConverter.Pressure(t) - atmosphericPressure)).DistinctUntilChanged().Subscribe(this.pressure, this.disposeToken.Token);

            this.timer = new Timer(
                new TimerCallback(o =>
                {
                    var pressure = this.pressure.Value + this.pressureOffset.Value;
                    var watts = this.random.Next((HeatingElementWatts * 100) - 2000, (HeatingElementWatts * 100) + 2000) / 100m;
                    var newTemperature = this.temperature.Value;

                    if (this.poweredOn.Value && !this.protection.Value)
                    {
                        if (pressure < this.targetPressure.Value - 0.03m)
                        {
                            // turn heating on.
                            this.heating.OnNext(true);
                        }
                        else if (pressure > this.targetPressure.Value + 0.03m)
                        {
                            // turn heating off.
                            this.heating.OnNext(false);
                            this.powerInWatts.OnNext(1);
                        }

                        // heat addition by the boiler ellement.
                        if (this.heating.Value)
                        {
                            this.powerInWatts.OnNext(1 + watts);
                            newTemperature += this.powerInWatts.Value / (BoilerContent * JoulesPerKgPerDegreeCelcius);
                        }

                        // heat loss.
                        newTemperature = Math.Max(roomTemperature, newTemperature - BoilerHeatLoss);

                        this.temperature.OnNext(newTemperature);

                        // protection.
                        if (newTemperature > ProtectionTemperature)
                        {
                            this.protection.OnNext(true);
                        }
                    }

                    this.tick.OnNext(this.tick.Value + 1);
                }),
                null,
                0,
                10);
        }

        /// <inheritdoc />
        public IObservable<bool> PoweredOn => this.poweredOn.DistinctUntilChanged();

        /// <inheritdoc />
        public IObservable<decimal> PowerInWatts => this.powerInWatts.DistinctUntilChanged();

        /// <inheritdoc />
        public IObservable<decimal> Pressure => this.pressure.CombineLatest(this.pressureOffset, (p, o) => Math.Round(p + o, 3)).DistinctUntilChanged();

        /// <inheritdoc />
        public IObservable<decimal> PressureOffset => this.pressureOffset;

        /// <inheritdoc />
        public IObservable<decimal> TargetPressure => this.targetPressure;

        /// <inheritdoc />
        public IObservable<decimal> Temperature => this.temperature.CombineLatest(this.temperatureOffset, (t, o) => Math.Round(t + o, 2)).DistinctUntilChanged();

        /// <inheritdoc />
        public IObservable<decimal> TemperatureOffset => this.temperatureOffset;

        /// <inheritdoc />
        public IObservable<bool> Protection => this.protection.DistinctUntilChanged();

        /// <inheritdoc />
        public IObservable<bool> Heating => this.heating.DistinctUntilChanged();

        /// <summary>
        /// Gets an observable value indicating the tick.
        /// </summary>
        public IObservable<int> Tick => this.tick;

        /// <summary>
        /// Cleans up managed and unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.timer.Dispose();
            this.disposeToken.Cancel();
        }

        /// <inheritdoc />
        public async Task PowerOff(CancellationToken cancellationToken)
        {
            await this.CommunicationDelay(cancellationToken);
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            this.poweredOn.OnNext(false);
        }

        /// <inheritdoc />
        public async Task PowerOn(CancellationToken cancellationToken)
        {
            await this.CommunicationDelay(cancellationToken);
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            this.poweredOn.OnNext(true);
        }

        /// <inheritdoc />
        public async Task ResetProtection(CancellationToken cancellationToken)
        {
            await this.CommunicationDelay(cancellationToken);
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            this.protection.OnNext(false);
        }

        /// <inheritdoc />
        public async Task SetPressureOffset(decimal targetOffset, CancellationToken cancellationToken)
        {
            await this.CommunicationDelay(cancellationToken);
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            this.pressureOffset.OnNext(targetOffset);
        }

        /// <inheritdoc />
        public async Task SetTargetPressure(decimal targetPressure, CancellationToken cancellationToken)
        {
            await this.CommunicationDelay(cancellationToken);
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            this.targetPressure.OnNext(targetPressure);
        }

        /// <inheritdoc />
        public async Task SetTemperatureOffset(decimal targetOffset, CancellationToken cancellationToken)
        {
            await this.CommunicationDelay(cancellationToken);
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            this.temperatureOffset.OnNext(targetOffset);
        }

        /// <summary>
        /// Introduces a modeled communication delay for a command between 100 and 300ms.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task CommunicationDelay(CancellationToken cancellationToken)
        {
            await Task.Delay(this.random.Next(100, 300), cancellationToken);
        }
    }
}
