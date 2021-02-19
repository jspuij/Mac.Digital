// <copyright file="RxExtensionsTests.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Tests.Rx
{
    using System;
    using System.Reactive.Linq;
    using FluentAssertions;
    using Mac.Digital.Rx;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="RxExtensions"/> class.
    /// </summary>
    public static class RxExtensionsTests
    {
        /// <summary>
        /// Can Call With Previous and calculate Fibonacci.
        /// </summary>
        [Fact]
        public static void CanCallWithPrevious()
        {
            var expected = new int[] { 0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610, 987, 1597, 2584, 4181, 6765 };

            var source = Observable.Range(0, 20);
            var result = source.WithPrevious((p, c) => c + p);
            result.ToArray().Subscribe(a =>
            {
                a.Should().BeEquivalentTo(expected);
            });
        }

        /// <summary>
        /// Cannot call WithPrevious with an null source.
        /// </summary>
        [Fact]
        public static void CannotCallWithPreviousWithNullSource()
        {
            Assert.Throws<ArgumentNullException>(() => default(IObservable<int>).WithPrevious(default(Func<int, int, int>)));
        }

        /// <summary>
        /// Cannot call WithPrevious with an null combinator.
        /// </summary>
        [Fact]
        public static void CannotCallWithPreviousWithNullCombinator()
        {
            Assert.Throws<ArgumentNullException>(() => new Mock<IObservable<int>>().Object.WithPrevious(default(Func<int, int, int>)));
        }
    }
}