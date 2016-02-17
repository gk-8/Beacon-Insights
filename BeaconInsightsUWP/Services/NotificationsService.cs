using System;
using Windows.UI.Notifications;
using NotificationsExtensions.Toasts; // NotificationsExtensions.Win10
using Microsoft.QueryStringDotNET; // QueryString.NET

namespace BeaconInsightsUWP.Services
{
    public class NotificationsService
    {
        public static NotificationsService Instance { get; }
        static NotificationsService()
        {
            // implement singleton pattern
            Instance = Instance ?? new NotificationsService();
        }
        public void Notify(string url)
        {
            ToastVisual visual = new ToastVisual()
            {
                TitleText = new ToastText()
                {
                    Text = "Welcome home!"
                },
                BodyTextLine1 = new ToastText()
                {
                    Text = "What TV-shows would you wanna watch today?"
                },
                AppLogoOverride = new ToastAppLogo()
                {
                    Crop = ToastImageCrop.Circle,
                    Source = new ToastImageSource("http://i.imgur.com/qeFYZoL.png?1")
                }
            };

            ToastActionsCustom actions = new ToastActionsCustom()
            {
                Buttons = {
                    new ToastButton("Browse unwatched", new QueryString()
                    {
                        { "action", "openurl" },
                        { "url", url }

                    }.ToString())
                    {
                        ActivationType = ToastActivationType.Foreground
                    },
                    new ToastButtonDismiss("Not now...")
                }
            };

            ToastContent toastContent = new ToastContent()
            {
                Visual = visual,
                Actions = actions,

                // Arguments when the user taps body of toast
                Launch = new QueryString()
                {
                    { "action", "viewConversation" },
                    { "url", url }

                }.ToString()
            };

            // And create the toast notification
            var toast = new ToastNotification(toastContent.GetXml());
            toast.ExpirationTime = DateTime.Now.AddDays(2);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
