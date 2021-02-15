// <copyright file="Header.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.WebUI.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Disposables;
    using System.Text;
    using System.Windows.Input;
    using Blazorise;
    using Mac.Digital.ViewModels;
    using Microsoft.AspNetCore.Components;
    using ReactiveUI;
    using ReactiveUI.Blazor;

    /// <summary>
    /// Header view. Renders the header.
    /// </summary>
    public partial class Header : ReactiveComponentBase<HeaderViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Header"/> class.
        /// </summary>
        public Header()
        {
            this.WhenActivated(disposable =>
            {
            });
        }

        /// <summary>
        /// Gets or sets the <see cref="HeaderViewModel"/> instance that will be injected into this view.
        /// </summary>
        [Inject]
        public HeaderViewModel HeaderViewModel
        {
            get => this.ViewModel;
            set => this.ViewModel = value;
        }
    }
}
