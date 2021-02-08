// <copyright file="PhotoTests.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Tests.Models
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using FluentAssertions;
    using Mac.Digital.Models;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="Photo"/> model.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PhotoTests
    {
        private Photo testClass;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotoTests"/> class.
        /// </summary>
        public PhotoTests()
        {
            this.testClass = new Photo();
        }

        /// <summary>
        /// Can construct a new instance of the Photo class.
        /// </summary>
        [Fact]
        public void CanConstruct()
        {
            var instance = new Photo();
            instance.Should().NotBeNull();
        }

        /// <summary>
        /// Can set and get the Id property.
        /// </summary>
        [Fact]
        public void CanSetAndGetId()
        {
            var testValue = new Guid("0ecddd8d-ef92-45ad-8681-53b4b17e42d4");
            this.testClass.Id = testValue;
            this.testClass.Id.Should().Be(testValue);
        }

        /// <summary>
        /// Can set and get the recipe Id.
        /// </summary>
        [Fact]
        public void CanSetAndGetRecipeId()
        {
            var testValue = new Guid("99662993-8ed0-43d0-870e-2edba2650dce");
            this.testClass.RecipeId = testValue;
            this.testClass.RecipeId.Should().Be(testValue);
        }

        /// <summary>
        /// Can set and get the file name.
        /// </summary>
        [Fact]
        public void CanSetAndGetFileName()
        {
            var testValue = "Photo001.jpg";
            this.testClass.FileName = testValue;
            this.testClass.FileName.Should().Be(testValue);
        }

        /// <summary>
        /// Can get and set the data.
        /// </summary>
        [Fact]
        public void CanSetAndGetData()
        {
            var testValue = new byte[] { 103, 190, 164, 121 };
            this.testClass.Data = testValue;
            this.testClass.Data.Should().BeEquivalentTo(testValue);
        }
    }
}