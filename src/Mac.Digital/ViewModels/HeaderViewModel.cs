// <copyright file="HeaderViewModel.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.ViewModels
{
    using System;
    using System.Reactive;
    using System.Threading;
    using Mac.Digital.Policies;
    using Mac.Digital.Services;
    using Microsoft.AspNetCore.Components;
    using ReactiveUI;

    /// <summary>
    /// ViewModel for the header component.
    /// </summary>
    public sealed class HeaderViewModel : ReactiveObject, IDisposable
    {
        /// <summary>
        /// Property helper for power.
        /// </summary>
        private readonly ObservableAsPropertyHelper<bool> poweredOn;

        /// <summary>
        /// Property helper for the title.
        /// </summary>
        private readonly ObservableAsPropertyHelper<string> title;

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

            this.poweredOn = powerService.PoweredOn.ToProperty(this, x => x.PoweredOn);
            this.title = titleService.Title.ToProperty(this, x => x.Title);
            var policy = policyProvider.GetPolicy();

            this.TogglePower = ReactiveCommand.CreateFromTask(async () =>
            {
                switch (this.PoweredOn)
                {
                    case true:
                        await policy.ExecuteAsync(
                            async (token) => await powerService.PowerOff(token),
                            CancellationToken.None);
                        break;
                    case false:
                        await policy.ExecuteAsync(
                            async (token) => await powerService.PowerOn(token),
                            CancellationToken.None);
                        break;
                }
            });

            this.Settings = ReactiveCommand.Create(() => navigationManager.NavigateTo("/Settings"));
        }

        /// <summary>
        /// Gets a value indicating whether the machine is powered on.
        /// </summary>
        public bool PoweredOn => this.poweredOn.Value;

        /// <summary>
        /// Gets the title.
        /// </summary>
        public string Title => this.title.Value;

        /// <summary>
        /// Gets a command that toggles the power.
        /// </summary>
        public ReactiveCommand<Unit, Unit> TogglePower { get; }

        /// <summary>
        /// Gets a command that toggles the power.
        /// </summary>
        public ReactiveCommand<Unit, Unit> Settings { get; }

        /// <summary>
        /// Cleans up any subscriptions in this viewmodel.
        /// </summary>
        public void Dispose()
        {
            this.poweredOn.Dispose();
            this.title.Dispose();
            this.TogglePower.Dispose();
            this.Settings.Dispose();
        }
    }
}
