// <copyright file="AppDelegate.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.macOS
{
    using AppKit;
    using Foundation;

    /// <summary>
    /// Application delegate class.
    /// </summary>
    [Register("AppDelegate")]
    public class AppDelegate : Xamarin.Forms.Platform.MacOS.FormsApplicationDelegate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDelegate"/> class.
        /// </summary>
        public AppDelegate()
        {
            var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;
            var rect = new CoreGraphics.CGRect(200, 1000, 1024, 768);
            this.MainWindow = new NSWindow(rect, style, NSBackingStore.Buffered, false)
            {
                Title = "My Application",
                TitleVisibility = NSWindowTitleVisibility.Visible,
            };
        }

        /// <inheritdoc />
        public override NSWindow MainWindow { get; }

        /// <inheritdoc />
        public override void DidFinishLaunching(NSNotification notification)
        {
            // Menu options to make it easy to press cmd+q to quit the app
            NSApplication.SharedApplication.MainMenu = this.MakeMainMenu();

            Xamarin.Forms.Forms.Init();
            this.LoadApplication(new App());
            base.DidFinishLaunching(notification);
        }

        /// <inheritdoc />
        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }

        private NSMenu MakeMainMenu()
        {
            // top bar app menu
            var menubar = new NSMenu();
            var appMenuItem = new NSMenuItem();
            menubar.AddItem(appMenuItem);

            var appMenu = new NSMenu();
            appMenuItem.Submenu = appMenu;

            // add separator
            var separator = NSMenuItem.SeparatorItem;
            appMenu.AddItem(separator);

            // add quit menu item
            var quitTitle = string.Format("Quit {0}", "Mac.Digital.macOS");
            var quitMenuItem = new NSMenuItem(quitTitle, "q", delegate
            {
                NSApplication.SharedApplication.Terminate(menubar);
            });
            appMenu.AddItem(quitMenuItem);

            return menubar;
        }
    }
}
