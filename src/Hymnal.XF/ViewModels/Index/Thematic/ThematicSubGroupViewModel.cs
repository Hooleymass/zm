using System.Collections.Generic;
using System.Threading.Tasks;
using Hymnal.XF.Constants;
using Hymnal.XF.Extensions;
using Hymnal.XF.Models;
using Hymnal.XF.Models.Parameters;
using Hymnal.XF.Services;
using Hymnal.XF.Views;
using Microsoft.AppCenter.Analytics;
using Prism.Navigation;

namespace Hymnal.XF.ViewModels
{
    /// <summary>
    /// Navigable from <see cref="ThematicIndexViewModel"/>
    /// </summary>
    public sealed class ThematicSubGroupViewModel : BaseViewModelParameter<GenericNavigationParameter<Thematic>>
    {
        private readonly IPreferencesService preferencesService;

        #region Properties
        private Thematic thematic;
        public Thematic Thematic
        {
            get => thematic;
            set => SetProperty(ref thematic, value);
        }

        public Ambit SelectedAmbit
        {
            get => null;
            set
            {
                if (value == null)
                    return;

                SelectedAmbitExecuteAsync(value).ConfigureAwait(true);
                RaisePropertyChanged(nameof(SelectedAmbit));
            }
        }
        #endregion

        public ThematicSubGroupViewModel(
            INavigationService navigationService,
            IPreferencesService preferencesService
            ) : base(navigationService)
        {
            this.preferencesService = preferencesService;
        }

        public override void Initialize(INavigationParameters parameters, GenericNavigationParameter<Thematic> parameter)
        {
            base.Initialize(parameters, parameter);
            Thematic = parameter.Value;
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            Analytics.TrackEvent(TrackingConstants.TrackEv.Navigation, new Dictionary<string, string>
            {
                { TrackingConstants.TrackEv.NavigationReferenceScheme.PageName, nameof(ThematicSubGroupViewModel) },
                { "Thematic Name", Thematic.ThematicName },
                { TrackingConstants.TrackEv.NavigationReferenceScheme.CultureInfo, InfoConstants.CurrentCultureInfo.Name },
                { TrackingConstants.TrackEv.NavigationReferenceScheme.HymnalVersion, preferencesService.ConfiguredHymnalLanguage.Id }
            });
        }

        private async Task SelectedAmbitExecuteAsync(Ambit ambit)
        {
            await NavigationService.NavigateAsync(nameof(ThematicHymnsListPage), new GenericNavigationParameter<Ambit>(ambit));
        }
    }
}
