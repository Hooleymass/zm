using System.Collections.Generic;
using System.Threading.Tasks;
using Hymnal.XF.Constants;
using Hymnal.XF.Extensions;
using Hymnal.XF.Models;
using Hymnal.XF.Models.Parameters;
using Hymnal.XF.Services;
using Microsoft.AppCenter.Analytics;
using MvvmHelpers;
using Prism.Navigation;

namespace Hymnal.XF.ViewModels
{
    /// <summary>
    /// Navigable from <see cref="ThematicSubGroupViewModel"/>
    /// </summary>
    public sealed class ThematicHymnsListViewModel : BaseViewModelParameter<GenericNavigationParameter<Ambit>>
    {
        private readonly IHymnsService hymnsService;
        private readonly IPreferencesService preferencesService;

        #region Properties
        private Ambit ambit;
        public Ambit Ambit
        {
            get => ambit;
            set => SetProperty(ref ambit, value);
        }

        public ObservableRangeCollection<Hymn> Hymns { get; } = new();

        public Hymn SelectedHymn
        {
            get => null;
            set
            {
                if (value == null)
                    return;

                SelectedHymnExecuteAsync(value).ConfigureAwait(true);
                RaisePropertyChanged(nameof(SelectedHymn));
            }
        }

        private readonly HymnalLanguage language;
        #endregion

        public ThematicHymnsListViewModel(
            INavigationService navigationService,
            IHymnsService hymnsService,
            IPreferencesService preferencesService
            ) : base(navigationService)
        {
            this.hymnsService = hymnsService;
            this.preferencesService = preferencesService;

            language = this.preferencesService.ConfiguredHymnalLanguage;
        }

        public override async Task InitializeAsync(INavigationParameters parameters, GenericNavigationParameter<Ambit> parameter)
        {
            await base.InitializeAsync(parameters, parameter);
            Ambit = parameter.Value;
            Hymns.AddRange((await hymnsService.GetHymnListAsync(language)).GetRange(Ambit.Star, Ambit.End));
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            Analytics.TrackEvent(TrackingConstants.TrackEv.Navigation, new Dictionary<string, string>
            {
                { TrackingConstants.TrackEv.NavigationReferenceScheme.PageName, nameof(ThematicHymnsListViewModel) },
                { "Ambit", Ambit.AmbitName },
                { TrackingConstants.TrackEv.NavigationReferenceScheme.CultureInfo, InfoConstants.CurrentCultureInfo.Name },
                { TrackingConstants.TrackEv.NavigationReferenceScheme.HymnalVersion, preferencesService.ConfiguredHymnalLanguage.Id }
            });
        }

        private async Task SelectedHymnExecuteAsync(Hymn hymn)
        {
            await NavigationService.NavigateAsync(
                NavRoutes.HymnViewerAsModal,
                new HymnIdParameter
                {
                    Number = hymn.Number,
                    HymnalLanguage = language
                }, true, true);
        }
    }
}
