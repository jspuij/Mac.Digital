// <copyright file="RxExtensions.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Rx
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using System.Text;

    /// <summary>
    /// Some handy reactive extension methods.
    /// </summary>
    public static class RxExtensions
    {
        /// <summary>
        /// Returns a new observable which combines the current with the previous value to create a result.
        /// </summary>
        /// <remarks>The first item predictable is swallowed.</remarks>
        /// <typeparam name="TSource">The type of the source observable.</typeparam>
        /// <typeparam name="TResult">The type of the resulting observable.</typeparam>
        /// <param name="source">The source observable.</param>
        /// <param name="combinator">The combinator function.</param>
        /// <returns>A new observable with the specified result type.</returns>
        public static IObservable<TResult> WithPrevious<TSource, TResult>(this IObservable<TSource> source, Func<TSource, TSource, TResult> combinator)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (combinator is null)
            {
                throw new ArgumentNullException(nameof(combinator));
            }

            return source.Zip(source.Skip(1)).Select((t) => combinator(t.First, t.Second));
        }
    }
}
