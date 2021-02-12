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

        private readonly BehaviorSubject<bool> poweredOn;
        private readonly BehaviorSubject<decimal> powerInWatts = new BehaviorSubject<decimal>(0);
        private readonly BehaviorSubject<decimal> pressure;
        private readonly BehaviorSubject<decimal> targetPressure;
        private readonly BehaviorSubject<decimal> temperature;
        private readonly BehaviorSubject<decimal> pressureOffset;
        private readonly BehaviorSubject<decimal> temperatureOffset;
        private readonly BehaviorSubject<bool> protection;
        private readonly BehaviorSubject<bool> heating = new BehaviorSubject<bool>(false);
        private readonly System.Threading.Timer timer;
        private readonly CancellationTokenSource disposeToken = new CancellationTokenSource();
        private readonly Random random = new Random();
        private readonly BehaviorSubject<int> tick = new BehaviorSubject<int>(0);
        private readonly SynchronizationContext synchronizationContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceSimulation"/> class.
        /// </summary>
        /// <param name="synchronizationContext">The synchronization Context to use.</param>
        public ServiceSimulation(SynchronizationContext synchronizationContext)
            : this(1000, false, 0m, 0m, 0m, 0m, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceSimulation"/> class with
        /// the specified initial values.
        /// </summary>
        /// <param name="synchronizationContext">The synchronization Context to use.</param>
        /// <param name="tickDuration">Tick duration in ms.</param>
        /// <param name="power">Initial power.</param>
        /// <param name="temperature">Initial temperature.</param>
        /// <param name="targetPressure">Initial target pressure.</param>
        /// <param name="pressureOffset">Initial target pressure offset.</param>
        /// <param name="temperatureOffset">Initial temperature offset.</param>
        /// <param name="protection">Initial protection.</param>
        public ServiceSimulation(
            SynchronizationContext synchronizationContext,
            int tickDuration,
            bool power,
            decimal temperature,
            decimal targetPressure,
            decimal pressureOffset,
            decimal temperatureOffset,
            bool protection)
        {
            this.poweredOn = new BehaviorSubject<bool>(power);
            this.pressure = new BehaviorSubject<decimal>(temperature);
            this.targetPressure = new BehaviorSubject<decimal>(targetPressure);
            this.pressureOffset = new BehaviorSubject<decimal>(pressureOffset);
            this.temperatureOffset = new BehaviorSubject<decimal>(temperatureOffset);
            this.protection = new BehaviorSubject<bool>(protection);
            this.heating = new BehaviorSubject<bool>(false);

            // random room temperature between 19.5 and 21.5 °C
            var roomTemperature = this.random.Next(195, 215) / 10m;

            // random atmospheric pressure between 1000 and 1026.
            var atmosphericPressure = this.random.Next(1000, 1026) / 1000m;

            // either choose room temperature or initial temperature, whichever is higher.
            this.temperature = new BehaviorSubject<decimal>(Math.Max(temperature, roomTemperature));

            // boiler pressure is a function of temperature.
            this.temperature.Select(t => Math.Max(0, TemperaturePressureConverter.Pressure(t) - atmosphericPressure)).DistinctUntilChanged().Subscribe(this.pressure, this.disposeToken.Token);

            this.timer = new Timer(
                o => this.synchronizationContext.Post(
                    oo =>
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

                            // protection.
                            if (newTemperature > ProtectionTemperature)
                            {
                                this.protection.OnNext(true);
                            }
                        }

                        // turn off heating in case of protection.
                        if (this.heating.Value && this.protection.Value)
                        {
                            this.heating.OnNext(false);
                            this.powerInWatts.OnNext(1);
                        }

                        // heat loss.
                        newTemperature = Math.Max(roomTemperature, newTemperature - BoilerHeatLoss);

                        this.temperature.OnNext(newTemperature);

                        this.tick.OnNext(this.tick.Value + 1);
                    },
                    null),
                null,
                0,
                tickDuration);
            this.synchronizationContext = synchronizationContext ?? throw new ArgumentNullException(nameof(synchronizationContext));
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
            if (!this.poweredOn.Value)
            {
                return;
            }

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
            if (this.poweredOn.Value)
            {
                return;
            }

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
            if (!this.protection.Value)
            {
                return;
            }

            if (this.temperature.Value > ProtectionTemperature)
            {
                throw new BoilerException(string.Format(Properties.Resources.ProtectionTemperatureExceeded, ProtectionTemperature));
            }

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
            if (targetOffset >= 1.000m || targetOffset <= -1.000m)
            {
                throw new ArgumentOutOfRangeException(nameof(targetOffset));
            }

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
            if (targetPressure >= 2.000m || targetPressure <= 0.000m)
            {
                throw new ArgumentOutOfRangeException(nameof(targetPressure));
            }

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
            if (targetOffset >= 5.000m || targetOffset <= -5.000m)
            {
                throw new ArgumentOutOfRangeException(nameof(targetOffset));
            }

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
