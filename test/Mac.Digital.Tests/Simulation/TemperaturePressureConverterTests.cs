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
        /// <param name="temperature">The temperature.</param>
        /// <param name="expectedPressure">The expected pressure.</param>
        [Theory]
        [InlineData(100.0, 1.014)]
        [InlineData(120.4, 2.012)]
        [InlineData(133.7, 3.015)]
        [InlineData(143.7, 4.009)]
        public static void CanCallPressure(decimal temperature, decimal expectedPressure)
        {
            var result = TemperaturePressureConverter.Pressure(temperature);
            result.Should().Be(expectedPressure);
        }

        /// <summary>
        /// A call to Pressure with an invalid argument throws an ArgumentOutOfRangeException.
        /// </summary>
        /// <param name="temperature">The temperature.</param>
        [Theory]
        [InlineData(-1)]
        [InlineData(151)]
        public static void CallPressureThrowsArgumentOutOfRangeException(decimal temperature)
        {
            Action act = () => TemperaturePressureConverter.Pressure(temperature);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        /// <summary>
        /// Can call the Temperature function to get a result.
        /// </summary>
        /// <param name="pressure">The pressure.</param>
        /// <param name="expectedTemperature">The expected temperature.</param>
        [Theory]
        [InlineData(1.000, 99.6)]
        [InlineData(2.000, 120.21)]
        [InlineData(3.000, 133.53)]
        [InlineData(4.000, 143.63)]
        public static void CanCallTemperature(decimal pressure, decimal expectedTemperature)
        {
            var result = TemperaturePressureConverter.Temperature(pressure);
            result.Should().Be(expectedTemperature);
        }

        /// <summary>
        /// A call to Temperature with an invalid argument throws an ArgumentOutOfRangeException.
        /// </summary>
        /// <param name="pressure">The pressure.</param>
        [Theory]
        [InlineData(-0.001)]
        [InlineData(5.000)]
        public static void CallTemperatureThrowsArgumentOutOfRangeException(decimal pressure)
        {
            Action act = () => TemperaturePressureConverter.Temperature(pressure);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}