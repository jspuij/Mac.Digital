// <copyright file="BrewState.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// An enum communicating the brew state.
    /// </summary>
    public enum BrewState
    {
        /// <summary>
        /// The machine is idle.
        /// </summary>
        Idle,

        /// <summary>
        /// The machine is performing a cooling flush.
        /// </summary>
        CoolingFlush,

        /// <summary>
        /// The machine is brewing.
        /// </summary>
        Brewing,
    }
}
