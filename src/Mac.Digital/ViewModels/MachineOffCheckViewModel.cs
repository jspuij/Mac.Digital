// <copyright file="MachineOffCheckViewModel.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.ViewModels
{
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using Mac.Digital.Policies;
    using Mac.Digital.Services;
    using ReactiveUI;

    /// <summary>
    /// ViewModel for the Machine OffCheck component.
    /// </summary>
    public class MachineOffCheckViewModel : ReactiveObject, IActivatableViewModel
    {
        /// <summary>
        /// Property helper for power.
        /// </summary>
        private ObservableAsPropertyHelper<bool> poweredOn;

        /// <summary>
        /// Property helper for can execute turnOn.
        /// </summary>
        private ObservableAsPropertyHelper<bool> canExecuteTurnOn;

        /// <summary>
        /// Initializes a new instance of the <see cref="MachineOffCheckViewModel"/> class.
        /// </summary>
        /// <param name="powerService">The power service to use.</param>
        /// <param name="policyProvider">The policy provider to use.</param>
        public MachineOffCheckViewModel(IPowerService powerService, ICommandPolicyProvider policyProvider)
        {
            if (powerService is null)
            {
                throw new System.ArgumentNullException(nameof(powerService));
            }

            if (policyProvider is null)
            {
                throw new System.ArgumentNullException(nameof(policyProvider));
            }

            this.WhenActivated(disposable =>
            {
                this.poweredOn = powerService.PoweredOn.ToProperty(this, x => x.PoweredOn).DisposeWith(disposable);
                var policy = policyProvider.GetPolicy();

                this.TurnOn = ReactiveCommand.CreateFromTask(
                    async (ct) =>
                        await policy.ExecuteAsync(
                            async (token) => await powerService.PowerOn(token),
                            ct),
                    powerService.PoweredOn.Select(x => !x)).DisposeWith(disposable);

                this.canExecuteTurnOn = this.TurnOn.CanExecute.ToProperty(this, x => x.CanExecuteTurnOn).DisposeWith(disposable);
            });
        }

        /// <summary>
        /// Gets a value indicating whether the machine is powered on.
        /// </summary>
        public bool PoweredOn => this.poweredOn?.Value ?? false;

        /// <summary>
        /// Gets a command that turns on the power.
        /// </summary>
        public ReactiveCommand<Unit, Unit> TurnOn { get; private set; }

        /// <summary>
        /// Gets a value indicating whether you can execute TurnOn.
        /// </summary>
        public bool CanExecuteTurnOn => this.canExecuteTurnOn?.Value ?? false;

        /// <summary>
        /// Gets the ViewModel Activator.
        /// </summary>
        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}
