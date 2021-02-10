// <copyright file="TemperaturePressureConverter.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Simulation
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Converts saturated steam temperature to pressure and vice versa. Note that this is absolute
    /// pressure. To get a reading relative to atmospheric pressure, substract the atmospheric pressure.
    /// </summary>
    /// <remarks>See https://www.researchgate.net/publication/257830931_A_new_formula_for_saturated_water_steam_pressure_within_the_temperature_range_-25_to_220C for formula.</remarks>
    public static class TemperaturePressureConverter
    {
        private const double A = 19.846d;
        private const double B = 0.00897d;
        private const double C = 0.00001248d;
        private const double E0 = 6.1121d;
        private const double K = 273.15d;

        /// <summary>
        /// Calculates the pressure in bar based on temperature in °C.
        /// </summary>
        /// <remarks>
        /// ln(E(t)/E(0)) = [(A − Bt + Ct^2)t/tK] with
        /// A = 19.846, B = 8.97 × 10−3, C = 1.248 × 10−5 and E(0) = 6.1121 GPa.
        /// </remarks>
        /// <param name="temperature">The temperature in °C.</param>
        /// <returns>The pressure in millibar.</returns>
        public static decimal Pressure(decimal temperature)
        {
            if (temperature > 150 || temperature < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(temperature));
            }

            var t = Convert.ToDouble(temperature);
            var tK = K + t;

            var lnEtDivE0 = (A - (B * t) + (C * Math.Pow(t, 2))) * t / tK;

            var etDivE0 = Math.Exp(lnEtDivE0);

            var et = etDivE0 * E0;
            return Math.Round(Convert.ToDecimal(et) / 1000m, 3);
        }

        /// <summary>
        /// Calculates the temperature in °C based on pressure in millibar.
        /// </summary>
        /// <remarks>Appriximation as inverse function is terrible.</remarks>
        /// <param name="pressure">The pressure in millibar.</param>
        /// <returns>The temperature in °C.</returns>
        public static decimal Temperature(decimal pressure)
        {
            var minPressure = Pressure(0m);
            var maxPressure = Pressure(150m);

            if (pressure < minPressure || pressure > maxPressure)
            {
                throw new ArgumentOutOfRangeException(nameof(pressure));
            }

            var currentTemp = 75m;
            decimal delta;

            do
            {
                var approxPressure = currentTemp > 150 ?
                    maxPressure :
                    currentTemp < 0 ?
                    minPressure :
                    Pressure(currentTemp);

                delta = pressure - approxPressure;
                currentTemp += delta * (75m / (maxPressure - minPressure));
            }
            while (Math.Abs(delta) > 0.001m);

            return Math.Round(currentTemp, 2);
        }
    }
}
