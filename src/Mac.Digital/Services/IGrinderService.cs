// <copyright file="IGrinderService.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for controlling the grinder.
    /// </summary>
    internal interface IGrinderService
    {
        /// <summary>
        /// Gets an observable value indicating whether the grinder is powered on.
        /// </summary>
        IObservable<bool> PoweredOn { get; }

        /// <summary>
        /// Gets an observable value indicating the power usage in Watts.
        /// </summary>
        IObservable<decimal> PowerInWatts { get; }

        /// <summary>
        /// Gets an observable value indicating the grind time as received from the arduino.
        /// </summary>
        IObservable<decimal> GrindTime { get; }

        /// <summary>
        /// Gets an observable value that indicates progress during a grind operation.
        /// </summary>
        IObservable<decimal> Progress { get; }

        /// <summary>
        /// Sets the grind time.
        /// </summary>
        /// <param name="grindTime">The grind time in hundreds of seconds.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="GrindException">Occurs when the grind time cannot be set.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the grinder does not respond in time.</exception>
        Task SetGrindTime(decimal grindTime, CancellationToken cancellationToken);

        /// <summary>
        /// Turns the grinder on to grind manually.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="GrindException">Thrown when the grinder cannot be turned on.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the grinder does not respond in time.</exception>
        Task TurnOn(CancellationToken cancellationToken);

        /// <summary>
        /// Turns the grinder off during manual grind.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="GrindException">Thrown when the machine cannot be turned off.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the grinder does not respond in time.</exception>
        Task PowerOff(CancellationToken cancellationToken);

        /// <summary>
        /// Turns the grinder on to grind automatically and off at the right time.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="GrindException">Thrown when the grinder cannot be turned on.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the grinder does not respond in time.</exception>
        Task AutoGrind(CancellationToken cancellationToken);

        /// <summary>
        /// Pulses the grinder for a short period of time so that roughly a gram comes out.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="GrindException">Thrown when the grinder cannot be turned on.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the grinder does not respond in time.</exception>
        Task Pulse(CancellationToken cancellationToken);
    }
}
