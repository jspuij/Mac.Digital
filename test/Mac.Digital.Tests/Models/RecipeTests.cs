// <copyright file="RecipeTests.cs" company="Jan-Willem Spuij">
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
    /// Unit tests for the <see cref="Recipe"/> model.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RecipeTests
    {
        private Recipe testClass;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecipeTests"/> class.
        /// </summary>
        public RecipeTests()
        {
            this.testClass = new Recipe();
        }

        /// <summary>
        /// Can construct an instance of the <see cref="Recipe"/> class.
        /// </summary>
        [Fact]
        public void CanConstruct()
        {
            var instance = new Recipe();
            instance.Should().NotBeNull();
        }

        /// <summary>
        /// Can get and set the Id.
        /// </summary>
        [Fact]
        public void CanSetAndGetId()
        {
            var testValue = new Guid("4e50e9d2-72f9-49f6-b670-86ada13eab7c");
            this.testClass.Id = testValue;
            this.testClass.Id.Should().Be(testValue);
        }

        /// <summary>
        /// Can get and set the name.
        /// </summary>
        [Fact]
        public void CanSetAndGetName()
        {
            var testValue = "Recipe #1";
            this.testClass.Name = testValue;
            this.testClass.Name.Should().Be(testValue);
        }

        /// <summary>
        /// Can get and set coffee.
        /// </summary>
        [Fact]
        public void CanSetAndGetCoffee()
        {
            var testValue = "Bani Ofair";
            this.testClass.Coffee = testValue;
            this.testClass.Coffee.Should().Be(testValue);
        }

        /// <summary>
        /// Can get and set the roast.
        /// </summary>
        [Fact]
        public void CanSetAndGetRoast()
        {
            var testValue = 4;
            this.testClass.Roast = testValue;
            this.testClass.Roast.Should().Be(testValue);
        }

        /// <summary>
        /// Can get and set the roaster.
        /// </summary>
        [Fact]
        public void CanSetAndGetRoaster()
        {
            var testValue = "Evermore";
            this.testClass.Roaster = testValue;
            this.testClass.Roaster.Should().Be(testValue);
        }

        /// <summary>
        /// Can get and set the dose.
        /// </summary>
        [Fact]
        public void CanSetAndGetDose()
        {
            var testValue = 19.00m;
            this.testClass.Dose = testValue;
            this.testClass.Dose.Should().Be(testValue);
        }

        /// <summary>
        /// Can get and set the target weight.
        /// </summary>
        [Fact]
        public void CanSetAndGetTargetWeight()
        {
            var testValue = 40.00m;
            this.testClass.TargetWeight = testValue;
            this.testClass.TargetWeight.Should().Be(testValue);
        }

        /// <summary>
        /// Can get and set the grid size.
        /// </summary>
        [Fact]
        public void CanSetAndGetGridSize()
        {
            var testValue = 24m;
            this.testClass.GridSize = testValue;
            this.testClass.GridSize.Should().Be(testValue);
        }

        /// <summary>
        /// Can get and set the temperature.
        /// </summary>
        [Fact]
        public void CanSetAndGetTemperature()
        {
            var testValue = 94.0m;
            this.testClass.Temperature = testValue;
            this.testClass.Temperature.Should().Be(testValue);
        }

        /// <summary>
        /// Can get and set the desired duration.
        /// </summary>
        [Fact]
        public void CanSetAndGetDesiredDuration()
        {
            var testValue = new TimeSpan(0, 0, 30);
            this.testClass.DesiredDuration = testValue;
            this.testClass.DesiredDuration.Should().Be(testValue);
        }

        /// <summary>
        /// Can get and set the prefusion time.
        /// </summary>
        [Fact]
        public void CanSetAndGetPreInfusionTime()
        {
            var testValue = new TimeSpan(0, 0, 7);
            this.testClass.PreInfusionTime = testValue;
            this.testClass.PreInfusionTime.Should().Be(testValue);
        }

        /// <summary>
        /// Can get and set the notes.
        /// </summary>
        [Fact]
        public void CanSetAndGetNotes()
        {
            var testValue = "Berries, cherries and marzipan.";
            this.testClass.Notes = testValue;
            this.testClass.Notes.Should().Be(testValue);
        }
    }
}