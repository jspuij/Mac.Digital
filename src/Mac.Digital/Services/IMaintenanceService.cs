// <copyright file="IMaintenanceService.cs" company="Jan-Willem Spuij">
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
    /// Interface to perform some maintenance on the machine.
    /// </summary>
    public interface IMaintenanceService
    {
        /// <summary>
        /// Gets an observable value indicating the progress of the maintenance process.
        /// </summary>
        IObservable<decimal> Progress { get; }

        /// <summary>
        /// Gets an observable value indicating the current state of the blind flush process.
        /// </summary>
        IObservable<BlindFlushState> BlindFlushState { get; }

        /// <summary>
        /// Gets an observable value indication whether confirmation from the user is neccessary
        /// to proceed to the next state of the blind flush process.
        /// </summary>
        IObservable<bool> WaitingForConfirmation { get; }

        /// <summary>
        /// Performs a screen flush that quickly cleans the group head screen after a coffee was made.
        /// </summary>
        /// <remarks>This asynchronous operation only completes after the complete routine is finished.</remarks>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="MaintenanceException">Thrown when the machine is unable to process the command.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the machine does not respond in time.</exception>
        Task ScreenFlush(CancellationToken cancellationToken);

        /// <summary>
        /// Performs a series of flushes that can be done with a blind filter.
        /// </summary>
        /// <remarks>This asynchronous operation only completes after the complete routine is finished.</remarks>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="MaintenanceException">Thrown when the machine is unable to process the command.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the machine does not respond in time.</exception>
        Task BlindFlush(CancellationToken cancellationToken);

        /// <summary>
        /// Confirms that the blind flush process can continue.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="MaintenanceException">Thrown when the machine is unable to process the command.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the machine does not respond in time.</exception>
        Task Continue(CancellationToken cancellationToken);
    }
}
