// <copyright file="TitleService.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Services
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Text;

    /// <summary>
    /// Implementation of the Title service.
    /// </summary>
    public class TitleService : ITitleService
    {
        /// <summary>
        /// The title subject.
        /// </summary>
        private readonly Subject<string> title = new Subject<string>();

        /// <summary>
        /// Gets an obserable value indicating the title.
        /// </summary>
        public IObservable<string> Title => this.title.AsObservable();

        /// <summary>
        /// Sets the title.
        /// </summary>
        /// <param name="title">The title to set.</param>
        public void SetTitle(string title)
        {
            if (title is null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            this.title.OnNext(title);
        }
    }
}
