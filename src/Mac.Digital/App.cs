﻿using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.MobileBlazorBindings;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Mac.Digital
{
    public class App : Application
    {
        public App(IFileProvider fileProvider = null)
        {
            var hostBuilder = MobileBlazorBindingsHost.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    // Adds web-specific services such as NavigationManager
                    services.AddBlazorHybrid();

                    // Register app-specific services
                    services.AddBlazorise(options =>
                    {
                        options.ChangeTextOnKeyPress = true;
                    })
                    .AddBootstrapProviders()
                    .AddFontAwesomeIcons();

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

            host.Services
              .UseBootstrapProviders()
              .UseFontAwesomeIcons();

            MainPage = new ContentPage { Title = "My Application" };
            NavigationPage.SetHasNavigationBar(MainPage, false);
            host.AddComponent<Main>(parent: MainPage);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
