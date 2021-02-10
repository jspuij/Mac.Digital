// <copyright file="IRecipeService.cs" company="Jan-Willem Spuij">
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
    /// A service for managing recipes.
    /// </summary>
    public interface IRecipeService
    {
        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of <see cref="Recipe"/>.
        /// </summary>
        /// <param name="cancellationToken">A token that cancels the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns the recipes.</returns>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        /// <exception cref="RecipeException">There was an error fetching the recipes.</exception>
        Task<IEnumerable<Recipe>> GetRecipes(CancellationToken cancellationToken);

        /// <summary>
        /// Deletes the recipe.
        /// </summary>
        /// <param name="recipe">The recipe to delete.</param>
        /// <param name="cancellationToken">A token that cancels the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        /// <exception cref="RecipeException">There was an error deleting the recipe.</exception>
        Task DeleteRecipe(Recipe recipe, CancellationToken cancellationToken);

        /// <summary>
        /// Saves the recipe.
        /// </summary>
        /// <param name="recipe">The recipe to save.</param>
        /// <param name="cancellationToken">A token that cancels the operation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
        /// <exception cref="RecipeException">There was an error saving the recipe.</exception>
        Task SaveRecipe(Recipe recipe, CancellationToken cancellationToken);
    }
}
