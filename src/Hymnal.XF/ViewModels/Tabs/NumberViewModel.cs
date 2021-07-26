using System.Collections.Generic;
using System.Linq;
using Hymnal.XF.Constants;
using Hymnal.XF.Extensions;
using Hymnal.XF.Models;
using Hymnal.XF.Models.Parameters;
using Hymnal.XF.Services;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Essentials.Interfaces;

namespace Hymnal.XF.ViewModels
{
    public class NumberViewModel : BaseViewModel
    {
        private readonly IHymnsService hymnsService;
        private readonly IPreferencesService preferencesService;
        private readonly IDeviceInfo deviceInfo;

        private string hymnNumber;
        public string HymnNumber
        {
            get => hymnNumber;
            set => SetProperty(ref hymnNumber, value);
        }

        #region Commands
        public DelegateCommand<string> OpenHymnCommand { get; internal set; }
        public DelegateCommand OpenRecordsCommand { get; internal set; }
        public INavigationService Navigationservice { get; }
        #endregion

        public NumberViewModel(
            INavigationService navigationService,
            IHymnsService hymnsService,
            IPreferencesService preferencesService,
            IDeviceInfo deviceInfo
            ) : base(navigationService)
        {
            Navigationservice = navigationService;
            this.hymnsService = hymnsService;
            this.preferencesService = preferencesService;
            this.deviceInfo = deviceInfo;
            OpenHymnCommand = new DelegateCommand<string>(OpenHymnAsync).ObservesCanExecute(() => NotBusy);
            OpenRecordsCommand = new DelegateCommand(OpenRecordsAsync).ObservesCanExecute(() => NotBusy);
#if DEBUG
            // A long hymn
            HymnNumber = $"{1}";
#endif
        }

        //public override void OnAppearing()
        //{
        //    base.OnAppearing();

        //    Analytics.TrackEvent(Constants.TrackEv.Navigation, new Dictionary<string, string>
        //    {
        //        { Constants.TrackEv.NavigationReferenceScheme.PageName, nameof(NumberViewModel) },
        //        { Constants.TrackEv.NavigationReferenceScheme.CultureInfo, Constants.CurrentCultureInfo.Name },
        //        { Constants.TrackEv.NavigationReferenceScheme.HymnalVersion, preferencesService.ConfiguratedHymnalLanguage.Id }
        //    });
        //}

        #region Command Actions
        private async void OpenHymnAsync(string text)
        {
            Busy = true;
            var num = text ?? HymnNumber;

            HymnalLanguage language = preferencesService.ConfiguratedHymnalLanguage;
            IEnumerable<Hymn> hymns = await hymnsService.GetHymnListAsync(language);

            if (int.TryParse(num, out var number))
            {
                if (number < 0 || number > hymns.Count())
                {
                    Busy = false;
                    return;
                }

                await NavigationService.NavigateAsync(
                    NavRoutes.HymnViewerAsFormSheetModal,
                    new HymnIdParameter
                    {
                        Number = number,
                        HymnalLanguage = language
                    },
                    true, true);
            }
            Busy = false;
        }

        private async void OpenRecordsAsync()
        {
            if (deviceInfo.Platform == Xamarin.Essentials.DevicePlatform.iOS)
            {
                await NavigationService.NavigateAsync(NavRoutes.RecordsPageAsFormSheetModal, true, true);
            }
            else
            {
                await NavigationService.NavigateAsync(NavRoutes.RecordsPage);
            }
        }
        #endregion
    }
}
