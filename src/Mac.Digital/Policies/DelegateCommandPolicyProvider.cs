// <copyright file="DelegateCommandPolicyProvider.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Policies
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Polly;

    /// <summary>
    /// A delegate command policy provider. This will provide timeouts and
    /// handle exceptions.
    /// </summary>
    public class DelegateCommandPolicyProvider : ICommandPolicyProvider
    {
        private readonly Func<AsyncPolicy> policyDelegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommandPolicyProvider"/> class.
        /// </summary>
        /// <param name="policyDelegate">The delegate that will provide the policy.</param>
        public DelegateCommandPolicyProvider(Func<AsyncPolicy> policyDelegate)
        {
            this.policyDelegate = policyDelegate ?? throw new ArgumentNullException(nameof(policyDelegate));
        }

        /// <inheritdoc />
        public AsyncPolicy GetPolicy() => this.policyDelegate();
    }
}
