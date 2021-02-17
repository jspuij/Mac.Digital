// <copyright file="HeaderViewModel.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.ViewModels
{
    using System;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Threading;
    using System.Windows.Input;
    using Mac.Digital.Policies;
    using Mac.Digital.Services;
    using Microsoft.AspNetCore.Components;
    using ReactiveUI;

    /// <summary>
    /// ViewModel for the header component.
    /// </summary>
    public sealed class HeaderViewModel : ReactiveObject, IActivatableViewModel
    {
        /// <summary>
        /// Property helper for power.
        /// </summary>
        private ObservableAsPropertyHelper<bool> poweredOn;

        /// <summary>
        /// Property helper for the title.
        /// </summary>
        private ObservableAsPropertyHelper<string> title;

        /// <summary>
        /// Property helper for can execute toggle power.
        /// </summary>
        private ObservableAsPropertyHelper<bool> canExecuteTogglePower;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderViewModel"/> class.
        /// </summary>
        /// <param name="powerService">The power service implementation.</param>
        /// <param name="titleService">The title service.</param>
        /// <param name="policyProvider">The policy provider to execute ordinary commands.</param>
        /// <param name="navigationManager">The navigation manager to navigate to a different page.</param>
        public HeaderViewModel(
            IPowerService powerService,
            ITitleService titleService,
            ICommandPolicyProvider policyProvider,
            NavigationManager navigationManager)
        {
            if (powerService is null)
            {
                throw new ArgumentNullException(nameof(powerService));
            }

            if (titleService is null)
            {
                throw new ArgumentNullException(nameof(titleService));
            }

            if (policyProvider is null)
            {
                throw new ArgumentNullException(nameof(policyProvider));
            }

            if (navigationManager is null)
            {
                throw new ArgumentNullException(nameof(navigationManager));
            }

            this.WhenActivated(disposable =>
            {
                this.poweredOn = powerService.PoweredOn.ToProperty(this, x => x.PoweredOn).DisposeWith(disposable);
                this.title = titleService.Title.ToProperty(this, x => x.Title).DisposeWith(disposable);
                var policy = policyProvider.GetPolicy();

                this.TogglePower = ReactiveCommand.CreateFromTask(async (ct) =>
                {
                    switch (this.PoweredOn)
                    {
                        case true:
                            await policy.ExecuteAsync(
                                async (token) => await powerService.PowerOff(token),
                                ct);
                            break;
                        case false:
                            await policy.ExecuteAsync(
                                async (token) => await powerService.PowerOn(token),
                                ct);
                            break;
                    }
                }).DisposeWith(disposable);

                this.Settings = ReactiveCommand.Create(() =>
                {
                    navigationManager.NavigateTo("/Settings");
                }).DisposeWith(disposable);

                this.canExecuteTogglePower = this.TogglePower.CanExecute.ToProperty(this, x => x.CanExecuteTogglePower).DisposeWith(disposable);
            });
        }

        /// <summary>
        /// Gets a value indicating whether the machine is powered on.
        /// </summary>
        public bool PoweredOn => this.poweredOn?.Value ?? false;

        /// <summary>
        /// Gets the title.
        /// </summary>
        public string Title => this.title?.Value ?? string.Empty;

        /// <summary>
        /// Gets a command that toggles the power.
        /// </summary>
        public ReactiveCommand<Unit, Unit> TogglePower { get; private set; }

        /// <summary>
        /// Gets a value indicating whether you can execute TogglePower.
        /// </summary>
        public bool CanExecuteTogglePower => this.canExecuteTogglePower?.Value ?? false;

        /// <summary>
        /// Gets a command that toggles the power.
        /// </summary>
        public ReactiveCommand<Unit, Unit> Settings { get; private set; }

        /// <summary>
        /// Gets the ViewModel Activator.
        /// </summary>
        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}
