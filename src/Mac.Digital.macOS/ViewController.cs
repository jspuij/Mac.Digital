// <copyright file="ViewController.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.macOS
{
    using System;
    using AppKit;
    using Foundation;

    /// <summary>
    /// View controller for the main view.
    /// </summary>
    public partial class ViewController : NSViewController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewController"/> class.
        /// </summary>
        /// <param name="handle">The handle.</param>
        public ViewController(IntPtr handle)
            : base(handle)
        {
        }

        /// <inheritdoc />
        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }

            set
            {
                base.RepresentedObject = value;

                // Update the view, if already loaded.
            }
        }

        /// <inheritdoc />
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
        }
    }
}
