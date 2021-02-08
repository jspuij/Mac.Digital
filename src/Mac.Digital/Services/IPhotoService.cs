// <copyright file="IPhotoService.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Mac.Digital.Models;

    /// <summary>
    /// A service for managing photos.
    /// </summary>
    internal interface IPhotoService
    {
        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of <see cref="Photo"/>.
        /// </summary>
        /// <param name="recipeId">The id of the recipe to get the photos for.</param>
        /// <param name="cancellationToken">A token that cancels the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns the photos.</returns>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        /// <exception cref="RecipeException">There was an error fetching the photos.</exception>
        Task<IEnumerable<Photo>> GetPhotos(Guid recipeId, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes the photo.
        /// </summary>
        /// <param name="photo">The photo to delete.</param>
        /// <param name="cancellationToken">A token that cancels the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        /// <exception cref="RecipeException">There was an error deleting the photo.</exception>
        Task DeletePhoto(Photo photo, CancellationToken cancellationToken);

        /// <summary>
        /// Saves the photo.
        /// </summary>
        /// <param name="photo">The photo to save.</param>
        /// <param name="cancellationToken">A token that cancels the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        /// <exception cref="RecipeException">There was an error saving the photo.</exception>
        Task SavePhoto(Photo photo, CancellationToken cancellationToken);
    }
}
