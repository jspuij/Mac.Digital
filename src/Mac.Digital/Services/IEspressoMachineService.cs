// <copyright file="IEspressoMachineService.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for controlling the MacDigital espresso machine.
    /// </summary>
    internal interface IEspressoMachineService
    {
        /// <summary>
        /// Gets an observable value reporting the water pressure in bar.
        /// </summary>
        IObservable<decimal> WaterPressure { get; }

        /// <summary>
        /// Gets an observable value reporting the water pressure offset in bar.
        /// </summary>
        IObservable<decimal> WaterPressureOffset { get; }

        /// <summary>
        /// Gets an observable value reporting the group head temperature in °C.
        /// </summary>
        IObservable<decimal> GroupHeadTemperature { get; }

        /// <summary>
        /// Gets an observable value reporting the group head temperature offset in °C.
        /// </summary>
        IObservable<decimal> GroupHeadTemperatureOffset { get; }

        /// <summary>
        /// Gets an observable value reporting the heat exchanger temperature in °C.
        /// </summary>
        IObservable<decimal> HeatExchangerTemperature { get; }

        /// <summary>
        /// Gets an observable value reporting the heat exchanger temperature offset in °C.
        /// </summary>
        IObservable<decimal> HeatExchangerTemperatureOffset { get; }

        /// <summary>
        /// Gets an observable value reporting the preinfusion time in s.
        /// </summary>
        IObservable<decimal> PreInfusionTime { get; }

        /// <summary>
        /// Gets an observable value reporting the desired shot weight in grams.
        /// </summary>
        IObservable<decimal> ShotWeight { get; }

        /// <summary>
        /// Gets an observable value reporting the desired shot temperature in °C.
        /// </summary>
        IObservable<decimal> ShotTemperature { get; }

        /// <summary>
        /// Gets an obserable value reporting the current brewstate.
        /// </summary>
        IObservable<BrewState> BrewState { get; }

        /// <summary>
        /// Gets an observable value indication whether confirmation from the user is neccessary
        /// to proceed to the next state of the brewing process.
        /// </summary>
        IObservable<bool> WaitingForConfirmation { get; }

        /// <summary>
        /// Sets the group head temperature offset.
        /// </summary>
        /// <remarks>This is used to calibrate the machine.</remarks>
        /// <param name="targetOffset">The temperature offset in °C. The range is between -5 and +5 °C.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="EspressoMachineException">Thrown when the machine is not able to process the command.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the argument is out of range.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the machine does not respond in time.</exception>
        Task SetGroupHeadTemperatureOffsetOffset(decimal targetOffset, CancellationToken cancellationToken);

        /// <summary>
        /// Sets the heat exchanger temperature offset.
        /// </summary>
        /// <remarks>This is used to calibrate the machine.</remarks>
        /// <param name="targetOffset">The temperature offset in °C. The range is between -5 and +5 °C.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="EspressoMachineException">Thrown when the machine is not able to process the command.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the argument is out of range.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the machine does not respond in time.</exception>
        Task SetHeatExchangerTemperatureOffset(decimal targetOffset, CancellationToken cancellationToken);

        /// <summary>
        /// Sets the preinfusion time.
        /// </summary>
        /// <param name="preInfusionTime">The preinfusion time. The range is between 0 and 60s.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="EspressoMachineException">Thrown when the machine is not able to process the command.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the argument is out of range.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the machine does not respond in time.</exception>
        Task SetPreinfusionTime(decimal preInfusionTime, CancellationToken cancellationToken);

        /// <summary>
        /// Sets the shot weight.
        /// </summary>
        /// <param name="desiredShotWeight">The desired shot weight in grams.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="EspressoMachineException">Thrown when the machine is not able to process the command.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the argument is out of range.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the machine does not respond in time.</exception>
        Task SetShotWeight(decimal desiredShotWeight, CancellationToken cancellationToken);

        /// <summary>
        /// Starts the machine to brew manually.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="EspressoMachineException">Thrown when the machine cannot be started.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the machine does not respond in time.</exception>
        Task Start(CancellationToken cancellationToken);

        /// <summary>
        /// Stops the brewing process.
        /// </summary>
        /// <remarks>This is intended to stop a manual brew. For an automated brew, the cancellation token can
        /// be invoked.</remarks>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="EspressoMachineException">Thrown when the machine cannot be stopped.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the machine does not respond in time.</exception>
        Task Stop(CancellationToken cancellationToken);

        /// <summary>
        /// Starts the brew process with a cooling flush and a subsequent start and stop until the desired weight
        /// is reached.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="EspressoMachineException">Thrown when the machine cannot be turned on.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the machine does not respond in time.</exception>
        Task AutoBrew(CancellationToken cancellationToken);

        /// <summary>
        /// Confirms that the auto brew process can continue.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="EspressoMachineException">Thrown when the machine is unable to process the command.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the machine does not respond in time.</exception>
        Task Continue(CancellationToken cancellationToken);
    }
}
