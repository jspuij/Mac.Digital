// <copyright file="BlindFlushState.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Enumeration that indicates the state
    /// during the Blindflush routine.
    /// </summary>
    public enum BlindFlushState
    {
        /// <summary>
        /// Routine is not running.
        /// </summary>
        NotRunning,

        /// <summary>
        /// Cleaning 5 times with detergent.
        /// </summary>
        RinsingWithDetergent,

        /// <summary>
        /// Cleaning the blind filter.
        /// </summary>
        CleaningBlindFilter,

        /// <summary>
        /// Rinsing 5 times without detergent.
        /// </summary>
        RinsingWithoutDetergent,

        /// <summary>
        /// Cleaning the group head with the brewfilter inside.
        /// </summary>
        CleaningGroupheadAndBrewFilter,
    }
}
