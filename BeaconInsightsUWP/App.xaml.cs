using BeaconInsightsUWP.Services;
using Microsoft.QueryStringDotNET;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.System;
using Windows.UI.Xaml;

namespace BeaconInsightsUWP
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);

            #region App settings

            var _settings = SettingsService.Instance;
            RequestedTheme = _settings.AppTheme;
            CacheMaxDuration = _settings.CacheMaxDuration;
            ShowShellBackButton = _settings.UseShellBackButton;

            #endregion
        }

        // runs even if restored from state
        public override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            // content may already be shell when resuming
            if ((Window.Current.Content as Views.Shell) == null)
            {
                // setup hamburger shell
                var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
                Window.Current.Content = new Views.Shell(nav);
            }

            return Task.CompletedTask;
        }

        // runs only when not restored from state
        public async override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            // Handle toast activation
            if (args is ToastNotificationActivatedEventArgs)
            {
                var toastActivationArgs = args as ToastNotificationActivatedEventArgs;

                // Parse the query string
                QueryString arguments = QueryString.Parse(toastActivationArgs.Argument);

                // See what action is being requested 
                switch (arguments["action"])
                {
                    case "openurl":
                        var url = arguments["url"];
                        await Launcher.LaunchUriAsync(new Uri(url));
                        break;
                    case "showtemperature":
                        var temperature = arguments["temperature"];
                        NavigationService.Navigate(typeof(Views.NotificationsPage), temperature);
                        break;
                    case "gettingfar":
                        var distance = arguments["distance"];
                        NavigationService.Navigate(typeof(Views.NotificationsPage), distance);
                        break;
                }
            }

            if (NavigationService.CurrentPageType != typeof(Views.MainPage))
                NavigationService.Navigate(typeof(Views.MainPage));
        }

    }
}

