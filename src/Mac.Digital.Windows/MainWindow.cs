// <copyright file="MainWindow.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.Windows
{
    using System;
    using Microsoft.MobileBlazorBindings.WebView.Windows;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.WPF;

    /// <summary>
    /// Main Window for the Windows app.
    /// </summary>
    public class MainWindow : FormsApplicationPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            Forms.Init();
            BlazorHybridWindows.Init();
            this.LoadApplication(new App());
        }

        /// <summary>
        /// Main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            var app = new System.Windows.Application();
            app.Run(new MainWindow());
        }
    }
}
