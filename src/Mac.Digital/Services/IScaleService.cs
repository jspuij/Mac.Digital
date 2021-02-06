// <copyright file="IScaleService.cs" company="Jan-Willem Spuij">
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
    /// Interface for scale control and scale reporting.
    /// </summary>
    /// <remarks>Note that the scale is used internally by the arduino as well
    /// to auto stop an espresso shot. This is not done by the C# code in any
    /// way.</remarks>
    internal interface IScaleService
    {
        /// <summary>
        /// Gets an observable value indicating the weight in grams currently
        /// on the scale.
        /// </summary>
        IObservable<decimal> Weight { get; }

        /// <summary>
        /// Tares the scale so that the balance is set to zero.
        /// </summary>
        /// <remarks>Taring resets a possible offset value.</remarks>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="ScaleException">Thrown when the scale is not able to process the command.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the scale does not respond in time.</exception>
        Task Tare(CancellationToken cancellationToken);

        /// <summary>
        /// Offsets the scale so that the balance is calculated relative to the offset.
        /// </summary>
        /// <remarks>Can be used to quickly substract the (fixed) weight of the portafilter in case
        /// you forgot to tare previously. Taring resets the offset.</remarks>
        /// <param name="offset">The offset value.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="ScaleException">Thrown when the scale is not able to process the command.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the scale does not respond in time.</exception>
        Task Offset(decimal offset, CancellationToken cancellationToken);
    }
}
