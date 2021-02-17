// <copyright file="MachineOffCheck.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.WebUI.Shared
{
    using System.Windows.Input;
    using Mac.Digital.ViewModels;
    using Microsoft.AspNetCore.Components;
    using ReactiveUI;
    using ReactiveUI.Blazor;

    /// <summary>
    /// MachineOffCheck component. Renders either content, or a message that the machine is off.
    /// </summary>
    public partial class MachineOffCheck : ReactiveComponentBase<MachineOffCheckViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MachineOffCheck"/> class.
        /// </summary>
        public MachineOffCheck()
        {
            this.WhenActivated(disposable =>
            {
            });
        }

        /// <summary>
        /// Gets or sets the <see cref="MachineOffCheckViewModel"/> instance that will be injected into this view.
        /// </summary>
        [Inject]
        public MachineOffCheckViewModel MachineOffCheckViewModel
        {
            get => this.ViewModel;
            set => this.ViewModel = value;
        }

        /// <summary>
        /// Gets or sets the child content for this View.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Turns the machine on.
        /// </summary>
        private void TurnOn()
        {
            if (!this.ViewModel.CanExecuteTurnOn)
            {
                return;
            }

            ((ICommand)this.ViewModel.TurnOn).Execute(null);
        }
    }
}
