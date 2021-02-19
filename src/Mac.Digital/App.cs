// <copyright file="App.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital
{
    using System;
    using System.Threading;
    using Blazorise;
    using Blazorise.Bootstrap;
    using Blazorise.Icons.FontAwesome;
    using Mac.Digital.Policies;
    using Mac.Digital.Services;
    using Mac.Digital.Simulation;
    using Mac.Digital.ViewModels;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Hosting;
    using Microsoft.MobileBlazorBindings;
    using Polly;
    using ReactiveUI;
    using Splat;
    using Splat.Microsoft.Extensions.DependencyInjection;
    using Splat.Microsoft.Extensions.Logging;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    /// <summary>
    /// Xamarin form application class.
    /// </summary>
    public class App : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        /// <param name="fileProvider">The file provider to use to fetch the resources for the platform.</param>
        public App(IFileProvider fileProvider = null)
        {
            var hostBuilder = MobileBlazorBindingsHost.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.UseMicrosoftDependencyResolver();
                    var resolver = Locator.CurrentMutable;
                    resolver.InitializeSplat();
                    resolver.InitializeReactiveUI();

                    // Adds web-specific services such as NavigationManager
                    services.AddBlazorHybrid();

                    // Register app-specific services
                    services.AddBlazorise(options =>
                    {
                        options.ChangeTextOnKeyPress = true;
                    })
                    .AddBootstrapProviders()
                    .AddFontAwesomeIcons();

                    // Default command timeout is 3 seconds.
                    services.AddSingleton<ICommandPolicyProvider>(s
                        => new DelegateCommandPolicyProvider(() => Policy.TimeoutAsync(3)));

#if SIMULATION
                    services.AddScoped(s => new ServiceSimulation(SynchronizationContext.Current, 10, false, 0m, 1.2m, 0m, 0m, false));
                    services.AddScoped<IPowerService>(s => s.GetRequiredService<ServiceSimulation>());
                    services.AddScoped<IBoilerService>(s => s.GetRequiredService<ServiceSimulation>());
#endif

                    services.AddSingleton<ITitleService, TitleService>();
                    services.AddScoped<MachineOffCheckViewModel>();
                    services.AddScoped<HeaderViewModel>();
                    services.AddScoped<BoilerViewModel>();
                })
                .ConfigureLogging(loggingBuilder =>
                {
                    loggingBuilder.AddSplat();
                })
                .UseWebRoot("wwwroot");

            if (fileProvider != null)
            {
                hostBuilder.UseStaticFiles(fileProvider);
            }
            else
            {
                hostBuilder.UseStaticFiles();
            }

            var host = hostBuilder.Build();

            host.Services.UseMicrosoftDependencyResolver();

            host.Services
              .UseBootstrapProviders()
              .UseFontAwesomeIcons();

            this.MainPage = new ContentPage { Title = "My Application" };
            NavigationPage.SetHasNavigationBar(this.MainPage, false);
            host.AddComponent<Main>(parent: this.MainPage);
        }

        /// <inheritdoc />
        protected override void OnStart()
        {
        }

        /// <inheritdoc />
        protected override void OnSleep()
        {
        }

        /// <inheritdoc />
        protected override void OnResume()
        {
        }
    }
}
