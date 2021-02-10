// <copyright file="IBoilerService.cs" company="Jan-Willem Spuij">
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
    /// Interface for interacting with the boiler monitor and control circuit.
    /// </summary>
    /// <remarks>There is a protection circuit in the arduino that will protect
    /// the boiler from overheating. This should normally never activate. It is
    /// resettable through software though. There is also a hardware Clixon that
    /// will trigger at 150 °C but then something has gone really wrong, it should
    /// never trigger.</remarks>
    public interface IBoilerService
    {
        /// <summary>
        /// Gets an observable value reporting the boiler pressure in bar.
        /// </summary>
        IObservable<decimal> Pressure { get; }

        /// <summary>
        /// Gets an observable value reporting the boiler pressure offset in bar.
        /// </summary>
        IObservable<decimal> PressureOffset { get; }

        /// <summary>
        /// Gets an observable value reporting the boiler target pressure in bar.
        /// </summary>
        IObservable<decimal> TargetPressure { get; }

        /// <summary>
        /// Gets an observable value reporting the boiler temperature in °C.
        /// </summary>
        IObservable<decimal> Temperature { get; }

        /// <summary>
        /// Gets an observable value reporting the boiler temperature offset in °C.
        /// </summary>
        IObservable<decimal> TemperatureOffset { get; }

        /// <summary>
        /// Gets an observable value reporting whether the protection circuit
        /// was activated.
        /// </summary>
        IObservable<bool> Protection { get; }

        /// <summary>
        /// Sets the target pressure on the boiler.
        /// </summary>
        /// <param name="targetPressure">The target pressure value in bar. The range is between 0 and 2bar.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="BoilerException">Thrown when the boiler is not able to process the command.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the argument is out of range.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the boiler does not respond in time.</exception>
        Task SetTargetPressure(decimal targetPressure, CancellationToken cancellationToken);

        /// <summary>
        /// Function to reset the boiler protection.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="BoilerException">Thrown when the boiler is not able to process the command.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the boiler does not respond in time.</exception>
        Task ResetProtection(CancellationToken cancellationToken);

        /// <summary>
        /// Sets the pressure offset in bar.
        /// </summary>
        /// <remarks>This is used to calibrate the machine.</remarks>
        /// <param name="targetOffset">The pressure offset value in bar. The range is between -1 and 1bar.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="BoilerException">Thrown when the boiler is not able to process the command.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the argument is out of range.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the boiler does not respond in time.</exception>
        Task SetPressureOffset(decimal targetOffset, CancellationToken cancellationToken);

        /// <summary>
        /// Sets the temperature offset on the boiler.
        /// </summary>
        /// <remarks>This is used to calibrate the machine.</remarks>
        /// <param name="targetOffset">The temperature offset in °C. The range is between -5 and +5 °C.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="BoilerException">Thrown when the boiler is not able to process the command.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the argument is out of range.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the boiler does not respond in time.</exception>
        Task SetTemperatureOffset(decimal targetOffset, CancellationToken cancellationToken);
    }
}
