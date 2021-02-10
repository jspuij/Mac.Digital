// <copyright file="ServiceSimulation.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Simulation
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Subjects;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Mac.Digital.Services;

    /// <summary>
    /// A simulation of all services.
    /// </summary>
    public class ServiceSimulation : IPowerService, IBoilerService
    {
        /// <summary>
        /// The amount of energy neccessary to heat a kg of water 1 °C.
        /// </summary>
        private const decimal JoulesPerKgPerDegreeCelcius = 4168m;

        /// <summary>
        /// Boiler content in kg.
        /// </summary>
        private const decimal BoilerContent = 5m;

        /// <summary>
        /// The power of the heating element in watts.
        /// </summary>
        private const decimal HeatingElementWatts = 1800m;

        private readonly BehaviorSubject<bool> poweredOn = new BehaviorSubject<bool>(false);
        private readonly BehaviorSubject<decimal> powerInWatts = new BehaviorSubject<decimal>(0);
        private readonly BehaviorSubject<decimal> targetPressure = new BehaviorSubject<decimal>(1.20m);
        private readonly BehaviorSubject<decimal> temperature;

        private readonly Random random = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceSimulation"/> class.
        /// </summary>
        public ServiceSimulation()
        {
            var roomTemperature = this.random.Next(195, 215) / 10m;

            // random room temperature between 19.5 and 21.5 °C
            this.temperature = new BehaviorSubject<decimal>(roomTemperature);
        }

        /// <inheritdoc />
        public IObservable<bool> PoweredOn => this.poweredOn;

        /// <inheritdoc />
        public IObservable<decimal> PowerInWatts => this.powerInWatts;

        /// <inheritdoc />
        public IObservable<decimal> Pressure => throw new NotImplementedException();

        /// <inheritdoc />
        public IObservable<decimal> PressureOffset => throw new NotImplementedException();

        /// <inheritdoc />
        public IObservable<decimal> TargetPressure => this.targetPressure;

        /// <inheritdoc />
        public IObservable<decimal> Temperature => this.temperature;

        /// <inheritdoc />
        public IObservable<decimal> TemperatureOffset => throw new NotImplementedException();

        /// <inheritdoc />
        public IObservable<bool> Protection => throw new NotImplementedException();

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
        public Task ResetProtection(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task SetPressureOffset(decimal targetOffset, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task SetTargetPressure(decimal targetPressure, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task SetTemperatureOffset(decimal targetOffset, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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
