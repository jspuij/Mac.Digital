// <copyright file="TestContextFixture.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Tests.Helpers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using Blazorise;
    using Blazorise.Bootstrap;
    using Blazorise.Icons.FontAwesome;
    using Bunit;
    using Mac.Digital.Policies;
    using Mac.Digital.Services;
    using Mac.Digital.Simulation;
    using Mac.Digital.ViewModels;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.DependencyInjection;
    using Polly;

    /// <summary>
    /// Fixture that creates a <see cref="TestContext"/> for bUnit.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TestContextFixture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestContextFixture"/> class.
        /// </summary>
        public TestContextFixture()
        {
            this.TestContext = new TestContext();

            this.TestContext.JSInterop.Mode = JSRuntimeMode.Loose;

            SetupDependencyInjection(this.TestContext.Services);
        }

        /// <summary>
        /// Gets the bUnit test context.
        /// </summary>
        public TestContext TestContext { get; }

        /// <summary>
        /// Sets up dependency injection for the bUnit tests.
        /// </summary>
        /// <param name="services">The services to set up.</param>
        public static void SetupDependencyInjection(IServiceCollection services)
        {
            if (services is null)
            {
                throw new System.ArgumentNullException(nameof(services));
            }

            services.AddSingleton<ICommandPolicyProvider>(s
                => new DelegateCommandPolicyProvider(() => Policy.TimeoutAsync(3)));

            // Register app-specific services
            services.AddBlazorise(options =>
            {
                options.ChangeTextOnKeyPress = true;
            })
            .AddBootstrapProviders()
            .AddFontAwesomeIcons();

            services.AddSingleton<ITitleService, TitleService>();
            services.AddSingleton<NavigationManager, TestableNavigationManager>();
            services.AddSingleton<HeaderViewModel>();
            services.AddSingleton<MachineOffCheckViewModel>();
            services.AddSingleton<BoilerViewModel>();
            services.AddSingleton(s => new ServiceSimulation(SynchronizationContext.Current, 10, false, 0m, 1.2m, 0m, 0m, false));
            services.AddSingleton<IPowerService>(s => s.GetRequiredService<ServiceSimulation>());
            services.AddSingleton<IBoilerService>(s => s.GetRequiredService<ServiceSimulation>());
        }
    }
}
