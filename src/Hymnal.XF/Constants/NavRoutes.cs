namespace Hymnal.XF.Constants
{
    public static class NavRoutes
    {
        public const string NavPage = "NavigationPage";
        public const string FormSheetNavPage = "FormSheetNavigationPage";

        public const string RootPage = "RootPage";
        public const string NumberPage = "NumberPage";
        public const string IndexPage = "IndexPage";
        public const string FavoritesPage = "FavoritesPage";
        public const string SettingsPage = "SettingsPage";
        public const string HymnPage = "HymnPage";
        public const string MusicSheetPage = "MusicSheetPage";
        public const string SearchPage = "SearchPage";
        public const string RecordsPage = "RecordsPage";
        public const string AlphabeticalIndexPage = "AlphabeticalIndexPage";
        public const string NumericalIndexPage = "NumericalIndexPage";
        public const string ThematicIndexPage = "ThematicIndexPage";
        public const string ThematicHymnsListPage = "ThematicHymnsListPage";
        public const string ThematicSubGroupPage = "ThematicSubGroupPage";

        public static readonly string HymnViewerAsModal = $"{NavPage}/{HymnPage}";
        public static readonly string HymnViewerAsFormSheetModal = $"{FormSheetNavPage}/{HymnPage}";
        public static readonly string RecordsPageAsFormSheetModal = $"{FormSheetNavPage}/{RecordsPage}";
        public static readonly string SearchPageAsFormSheetModal = $"{FormSheetNavPage}/{SearchPage}";
    }
}
