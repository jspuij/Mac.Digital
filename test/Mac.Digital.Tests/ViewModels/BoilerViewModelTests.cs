// <copyright file="BoilerViewModelTests.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Tests.ViewModels
{
    using System;
    using Mac.Digital;
    using Mac.Digital.Policies;
    using Mac.Digital.Services;
    using Mac.Digital.ViewModels;
    using Moq;
    using ReactiveUI;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="BoilerViewModel"/> class.
    /// </summary>
    public class BoilerViewModelTests
    {
        private Mock<IBoilerService> boilerService;
        private Mock<ICommandPolicyProvider> policyProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoilerViewModelTests"/> class.
        /// </summary>
        public BoilerViewModelTests()
        {
            this.boilerService = new Mock<IBoilerService>();
            this.policyProvider = new Mock<ICommandPolicyProvider>();
        }

        /// <summary>
        /// Can construct an instance of the ViewModel.
        /// </summary>
        [Fact]
        public void CanConstruct()
        {
            var instance = new BoilerViewModel(this.boilerService.Object, this.policyProvider.Object);
            Assert.NotNull(instance);
        }

        /// <summary>
        /// Cannot construct with null boiler service.
        /// </summary>
        [Fact]
        public void CannotConstructWithNullBoilerService()
        {
            Assert.Throws<ArgumentNullException>(() => new BoilerViewModel(default(IBoilerService), new Mock<ICommandPolicyProvider>().Object));
        }

        /// <summary>
        /// Cannot construct with null policy provider.
        /// </summary>
        [Fact]
        public void CannotConstructWithNullPolicyProvider()
        {
            Assert.Throws<ArgumentNullException>(() => new BoilerViewModel(new Mock<IBoilerService>().Object, default(ICommandPolicyProvider)));
        }

        /// <summary>
        /// Can get the pressure observable.
        /// </summary>
        [Fact]
        public void CanGetPressure()
        {
            Assert.IsType<decimal>(_testClass.Pressure);
            Assert.True(false, "Create or modify test");
        }

        [Fact]
        public void CanSetAndGetTargetPressure()
        {
            _testClass.CheckProperty(x => x.TargetPressure);
        }

        [Fact]
        public void CanGetTemperature()
        {
            Assert.IsType<decimal>(_testClass.Temperature);
            Assert.True(false, "Create or modify test");
        }

        [Fact]
        public void CanGetTemperatureTrend()
        {
            Assert.IsType<Trend>(_testClass.TemperatureTrend);
            Assert.True(false, "Create or modify test");
        }

        [Fact]
        public void CanGetProtection()
        {
            Assert.IsType<bool>(_testClass.Protection);
            Assert.True(false, "Create or modify test");
        }

        [Fact]
        public void CanGetHeating()
        {
            Assert.IsType<bool>(_testClass.Heating);
            Assert.True(false, "Create or modify test");
        }

        [Fact]
        public void CanGetActivator()
        {
            Assert.IsType<ViewModelActivator>(_testClass.Activator);
            Assert.True(false, "Create or modify test");
        }
    }
}