// <copyright file="Boiler.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.WebUI.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Mac.Digital.ViewModels;
    using Microsoft.AspNetCore.Components;
    using ReactiveUI;
    using ReactiveUI.Blazor;

    /// <summary>
    /// Boiler View Component. Renders the boiler info and adjustment.
    /// </summary>
    public partial class Boiler : ReactiveComponentBase<BoilerViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Boiler"/> class.
        /// </summary>
        public Boiler()
        {
            this.WhenActivated(disposable =>
            {
            });
        }

        /// <summary>
        /// Gets or sets the <see cref="BoilerViewModel"/> instance that will be injected into this view.
        /// </summary>
        [Inject]
        public BoilerViewModel BoilerViewModel
        {
            get => this.ViewModel;
            set => this.ViewModel = value;
        }
    }
}
