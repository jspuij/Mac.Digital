// <copyright file="BoilerViewModel.cs" company="Jan-Willem Spuij">
// Copyright (c) Jan-Willem Spuij.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.
// </copyright>

namespace Mac.Digital.ViewModels
{
    using System.Reactive.Disposables;
    using Mac.Digital.Policies;
    using Mac.Digital.Rx;
    using Mac.Digital.Services;
    using ReactiveUI;

    /// <summary>
    /// ViewModel for showing Boiler Information and adjustment.
    /// </summary>
    public class BoilerViewModel : ReactiveObject, IActivatableViewModel
    {
        private ObservableAsPropertyHelper<decimal> pressure;
        private ObservableAsPropertyHelper<decimal> targetPressure;
        private ObservableAsPropertyHelper<decimal> temperature;
        private ObservableAsPropertyHelper<Trend> temperatureTrend;
        private ObservableAsPropertyHelper<bool> protection;
        private ObservableAsPropertyHelper<bool> heating;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoilerViewModel"/> class.
        /// </summary>
        /// <param name="boilerService">The boiler Service to use.</param>
        /// <param name="policyProvider">The policy provider.</param>
        public BoilerViewModel(IBoilerService boilerService, ICommandPolicyProvider policyProvider)
        {
            this.WhenActivated(disposable =>
            {
                this.pressure = boilerService.Pressure.ToProperty(this, x => x.Pressure).DisposeWith(disposable);
                this.targetPressure = boilerService.TargetPressure.ToProperty(this, x => x.Pressure).DisposeWith(disposable);
                this.temperature = boilerService.Temperature.ToProperty(this, x => x.Temperature).DisposeWith(disposable);
                this.temperatureTrend = boilerService.Temperature
                    .WithPrevious((p, c) => c < p ? Trend.Falling : c > p ? Trend.Rising : Trend.Stable)
                    .ToProperty(this, x => x.TemperatureTrend)
                    .DisposeWith(disposable);
                this.protection = boilerService.Protection.ToProperty(this, x => x.Protection).DisposeWith(disposable);
                this.heating = boilerService.Heating.ToProperty(this, x => x.Heating).DisposeWith(disposable);
            });
        }

        /// <summary>
        /// Gets an observable value indicating the pressure.
        /// </summary>
        public decimal Pressure => this.pressure?.Value ?? 0m;

        /// <summary>
        /// Gets an observable value indicating the target pressure.
        /// </summary>
        public decimal TargetPressure => this.targetPressure?.Value ?? 0m;

        /// <summary>
        /// Gets an observable value indicating the temperature.
        /// </summary>
        public decimal Temperature => this.temperature?.Value ?? 0m;

        /// <summary>
        /// Gets an observable value indicating the pressure.
        /// </summary>
        public Trend TemperatureTrend => this.temperatureTrend?.Value ?? Trend.Stable;

        /// <summary>
        /// Gets a value indicating whether the proction was activated.
        /// </summary>
        public bool Protection => this.protection?.Value ?? false;

        /// <summary>
        /// Gets a value indicating whether the heating was activated.
        /// </summary>
        public bool Heating => this.heating?.Value ?? false;

        /// <summary>
        /// Gets the ViewModel Activator.
        /// </summary>
        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}
