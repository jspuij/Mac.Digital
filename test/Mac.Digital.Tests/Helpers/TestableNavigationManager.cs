// <copyright file="TestableNavigationManager.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Tests.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using Microsoft.AspNetCore.Components;

    /// <summary>
    /// A testableNavigationManager.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class TestableNavigationManager : NavigationManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestableNavigationManager"/> class.
        /// </summary>
        public TestableNavigationManager()
        {
            this.Initialize("https://localhost:5000/", "https://localhost:5000/");
        }

        /// <summary>
        /// Gets the last Uri.
        /// </summary>
        public string LastUri { get; private set; }

        /// <summary>
        /// Gets a value indicating whether load was forced.
        /// </summary>
        public bool LastForceLoad { get; private set; }

        /// <inheritdoc/>
        protected override void NavigateToCore(string uri, bool forceLoad)
        {
            this.LastUri = uri;
            this.LastForceLoad = forceLoad;
        }
    }
}
