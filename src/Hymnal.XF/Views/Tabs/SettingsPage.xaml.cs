using System;
using Hymnal.XF.Resources.Languages;
using Hymnal.XF.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Hymnal.XF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public sealed partial class SettingsPage : BaseContentPage<SettingsViewModel>, ITabbedPage
    {
        public string TabbedPageName => Languages.Settings;

        public SettingsPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.Tizen)
            {
                PreferencesSection.Remove(FontSizeCell);

                developerSection.Remove(developerCell);
                developerSection.Remove(appVersionCell);
                developerSection.Remove(appBuildCell);
            }
        }

        // TODO: Se puede crear un converter o algo similar
        private void LetterSize_ValueChanged(object sender, ValueChangedEventArgs e) => LetterSize.Value = Math.Round(e.NewValue, 0);
    }
}
