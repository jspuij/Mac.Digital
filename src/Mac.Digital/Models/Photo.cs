// <copyright file="Photo.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A photo that belongs to a recipe.
    /// </summary>
    public class Photo
    {
        /// <summary>
        /// Gets or sets the unique Id for the photo.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique Id for the photo.
        /// </summary>
        public Guid RecipeId { get; set; }

        /// <summary>
        /// Gets or sets the file name for the photo.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the data for the photo.
        /// </summary>
        public byte[] Data { get; set; }
    }
}
