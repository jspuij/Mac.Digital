// <copyright file="Main.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.iOS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Foundation;
    using Microsoft.MobileBlazorBindings.WebView.iOS;
    using UIKit;

    /// <summary>
    /// Application.
    /// </summary>
#pragma warning disable SA1649 // File name should match first type name
    public class Application
#pragma warning restore SA1649 // File name should match first type name
    {
        /// <summary>
        /// Main entry point of the applcation.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            BlazorHybridIOS.Init();

            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
