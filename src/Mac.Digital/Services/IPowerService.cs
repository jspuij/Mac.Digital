// <copyright file="IPowerService.cs" company="Jan-Willem Spuij">
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
    /// Interface for a Service to power on and power off the machine.
    /// </summary>
    public interface IPowerService
    {
        /// <summary>
        /// Gets an observable value indicating whether the machine is powered on.
        /// </summary>
        IObservable<bool> PoweredOn { get; }

        /// <summary>
        /// Gets an observable value indicating the power usage in Watts.
        /// </summary>
        IObservable<decimal> PowerInWatts { get; }

        /// <summary>
        /// Turns the machine on.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="PowerException">Thrown when the machine cannot be turned on.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the machine does not respond in time.</exception>
        Task PowerOn(CancellationToken cancellationToken);

        /// <summary>
        /// Turns the machine on.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="PowerException">Thrown when the machine cannot be turned on.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the machine does not respond in time.</exception>
        Task PowerOff(CancellationToken cancellationToken);
    }
}
