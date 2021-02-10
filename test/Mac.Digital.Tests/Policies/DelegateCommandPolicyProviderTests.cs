// <copyright file="DelegateCommandPolicyProviderTests.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Tests.Policies
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using FluentAssertions;
    using Mac.Digital.Policies;
    using Polly;
    using Polly.Timeout;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="DelegateCommandPolicyProvider"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DelegateCommandPolicyProviderTests
    {
        private readonly DelegateCommandPolicyProvider testClass;
        private readonly Func<AsyncPolicy> policyDelegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommandPolicyProviderTests"/> class.
        /// </summary>
        public DelegateCommandPolicyProviderTests()
        {
            this.policyDelegate = () => Policy.TimeoutAsync(3);
            this.testClass = new DelegateCommandPolicyProvider(this.policyDelegate);
        }

        /// <summary>
        /// Can construct a new instance of the <see cref="DelegateCommandPolicyProvider"/> class.
        /// </summary>
        [Fact]
        public void CanConstruct()
        {
            var instance = new DelegateCommandPolicyProvider(this.policyDelegate);
            Assert.NotNull(instance);
        }

        /// <summary>
        /// Cannot construct an instance with a null policy delegate.
        /// </summary>
        [Fact]
        public void CannotConstructWithNullPolicyDelegate()
        {
            Assert.Throws<ArgumentNullException>(() => new DelegateCommandPolicyProvider(default));
        }

        /// <summary>
        /// Can get a policy from the DelegateCommandPolicyProvider class.
        /// </summary>
        [Fact]
        public void CanGetPolicy()
        {
            this.testClass.GetPolicy().Should().BeOfType<AsyncTimeoutPolicy>();
        }
    }
}