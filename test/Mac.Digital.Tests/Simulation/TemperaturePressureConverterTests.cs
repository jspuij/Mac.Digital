// <copyright file="TemperaturePressureConverterTests.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Tests.Simulation
{
    using System;
    using FluentAssertions;
    using Mac.Digital.Simulation;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="TemperaturePressureConverter"/> class.
    /// </summary>
    public static class TemperaturePressureConverterTests
    {
        /// <summary>
        /// Can call the Pressure function to get a result.
        /// </summary>
        [Fact]
        public static void CanCallPressure()
        {
            var temperature = 143.7m;
            var result = TemperaturePressureConverter.Pressure(temperature);
            result.Should().Be(4.009m);
        }

        /// <summary>
        /// Can call the Temperature function to get a result.
        /// </summary>
        [Fact]
        public static void CanCallTemperatyre()
        {
            var pressure = 4.009m;
            var result = TemperaturePressureConverter.Temperature(pressure);
            result.Should().Be(143.71m);
        }
    }
}