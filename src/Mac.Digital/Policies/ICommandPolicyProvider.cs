// <copyright file="ICommandPolicyProvider.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Policies
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Polly;
    using Polly.Timeout;

    /// <summary>
    /// Providers a default policy for handling command timeouts and exceptions.
    /// </summary>
    public interface ICommandPolicyProvider
    {
        /// <summary>
        /// Gets the policy.
        /// </summary>
        /// <returns>The async policy.</returns>
        AsyncPolicy GetPolicy();
    }
}
