using System.ComponentModel;
using Hymnal.XF.Resources.Languages;
using Hymnal.XF.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Hymnal.XF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : BaseContentPage<SearchViewModel>, ISearchPage, IModalPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Focuse HymnSearchBar
            if (string.IsNullOrWhiteSpace(HymnSearchBar.Text))
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        @delegate.BecomeFirstResponder();
                        break;

                    default:
                        HymnSearchBar.Focus();
                        break;
                }
            }
        }

        #region IModalPage
        string IModalPage.CloseButtonText => Languages.Generic_Close;

        void IModalPage.PopModal() => ViewModel.NavigationService.GoBackAsync(null, true, true);
        #endregion

        #region ISearchPage
        private ISearchDelegate @delegate;
        ISearchDelegate ISearchPage.Delegate
        {
            get => @delegate;
            set
            {
                @delegate = value;

                ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(ViewModel.TextSearchBar)) && @delegate is not null && !ViewModel.TextSearchBar.Equals(@delegate.SearchText))
                @delegate.SearchText = ViewModel.TextSearchBar;
        }

        ISearchPageSettings ISearchPage.Settings { get; } = new SearchPageSettings
        {
            InitiallyFocus = true,
        };

        string ISearchPage.PlaceholderText => HymnSearchBar.Placeholder;
        Color ISearchPage.PlaceHolderColor => (Color)App.Current.ThemeHelper.CurrentResourceDictionaryTheme["PrimaryLightColor"];
        Color ISearchPage.TextColor => (Color)App.Current.ThemeHelper.CurrentResourceDictionaryTheme["NavBarTextColor"];

        void ISearchPage.OnSearchBarTextChanged(in string text) => ViewModel.TextSearchBar = text;
        void ISearchPage.SearchTapped(in string text) { }
        void ISearchPage.Focused() { }
        void ISearchPage.Unfocused() { }
        void ISearchPage.Canceled() { }
        #endregion
    }
}
