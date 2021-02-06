// <copyright file="AppDelegate.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.iOS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Foundation;
    using UIKit;
    using Xamarin.Forms;

    /// <summary>
    /// Application delegate class.
    /// </summary>
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        /// <inheritdoc />
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            // For iOS, wrap inside a navigation page, otherwise the header looks wrong
            var formsApp = new App();
            formsApp.MainPage = new NavigationPage(formsApp.MainPage);

            this.LoadApplication(formsApp);

            return base.FinishedLaunching(app, options);
        }
    }
}
