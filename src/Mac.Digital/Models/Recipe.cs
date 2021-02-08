// <copyright file="Recipe.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A coffee recipe.
    /// </summary>
    internal class Recipe
    {
        /// <summary>
        /// Gets or sets the unique Id for the recipe.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Coffee.
        /// </summary>
        public string Coffee { get; set; }

        /// <summary>
        /// Gets or sets the lightness / darkness on a scale from 1 to 10. 1 being the lightest.
        /// </summary>
        public int Roast { get; set; }

        /// <summary>
        /// Gets or sets the roaster.
        /// </summary>
        public string Roaster { get; set; }

        /// <summary>
        /// Gets or sets the dose in g.
        /// </summary>
        public decimal Dose { get; set; }

        /// <summary>
        /// Gets or sets the target weight of the cofee.
        /// </summary>
        public decimal TargetWeight { get; set; }

        /// <summary>
        /// Gets or sets the grind size (if the grinder has a scale).
        /// </summary>
        public decimal GridSize { get; set; }

        /// <summary>
        /// Gets or sets the brewing temperature.
        /// </summary>
        public decimal Temperature { get; set; }

        /// <summary>
        /// Gets or sets the desired duration.
        /// </summary>
        public TimeSpan DesiredDuration { get; set; }

        /// <summary>
        /// Gets or sets the preinfusion time.
        /// </summary>
        public TimeSpan PreInfusionTime { get; set; }

        /// <summary>
        /// Gets or sets the notes about this recipe.
        /// </summary>
        public string Notes { get; set; }
    }
}
