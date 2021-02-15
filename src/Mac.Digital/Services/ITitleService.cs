// <copyright file="ITitleService.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Provides a title for the Application.
    /// </summary>
    public interface ITitleService
    {
        /// <summary>
        /// Gets an obserable value indicating the title.
        /// </summary>
        IObservable<string> Title { get; }

        /// <summary>
        /// Sets the title.
        /// </summary>
        /// <param name="title">The title to set.</param>
        void SetTitle(string title);
    }
}
