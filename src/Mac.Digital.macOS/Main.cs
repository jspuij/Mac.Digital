// <copyright file="Main.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.macOS
{
    using AppKit;
    using Microsoft.MobileBlazorBindings.WebView.macOS;

    /// <summary>
    /// Main class.
    /// </summary>
#pragma warning disable SA1649 // File name should match first type name
    internal static class MainClass
#pragma warning restore SA1649 // File name should match first type name
    {
        private static void Main(string[] args)
        {
            BlazorHybridMacOS.Init();
            NSApplication.Init();
            NSApplication.SharedApplication.Delegate = new AppDelegate();
            NSApplication.Main(args);
        }
    }
}
