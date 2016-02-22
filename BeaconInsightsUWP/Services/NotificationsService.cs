using BeaconInsightsUWP.Services.Interfaces;
using Microsoft.QueryStringDotNET; // QueryString.NET
using NotificationsExtensions.Toasts; // NotificationsExtensions.Win10
using System;
using System.Text;
using Windows.UI.Notifications;

namespace BeaconInsightsUWP.Services
{
    public class NotificationsService : INotificationsService
    {
        private ToastContent _toastContent;

        public void NotifyWithUrl(string title, string message, string url)
        {
            ToastVisual visual = new ToastVisual()
            {
                TitleText = new ToastText()
                {
                    Text = title
                },
                BodyTextLine1 = new ToastText()
                {
                    Text = message
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

            _toastContent = new ToastContent()
            {
                Visual = visual,
                Actions = actions,

                // Arguments when the user taps body of toast
                Launch = new QueryString()
                {
                    { "action", "openurl" },
                    { "url", url }

                }.ToString()
            };
            SendNotification();
        }

        public void NotifyWithTemperature(string title, string message, float temperature)
        {
            StringBuilder sb = new StringBuilder();
            ToastVisual visual = new ToastVisual()
            {
                TitleText = new ToastText()
                {
                    Text = title
                },
                BodyTextLine1 = new ToastText()
                {
                    Text = sb.AppendFormat(message, temperature).ToString()
                }
            };

            _toastContent = new ToastContent()
            {
                Visual = visual,

                // Arguments when the user taps body of toast
                Launch = new QueryString()
                {
                    { "action", "showtemperature" },
                    { "temperature", temperature.ToString() }

                }.ToString()
            };
            SendNotification();
        }

        public void NotifyWithDistance(string title, string message, double distance)
        {
            ToastVisual visual = new ToastVisual()
            {
                TitleText = new ToastText()
                {
                    Text = title
                },
                BodyTextLine1 = new ToastText()
                {
                    Text = message
                }
            };

            _toastContent = new ToastContent()
            {
                Visual = visual,

                // Arguments when the user taps body of toast
                Launch = new QueryString()
                {
                    { "action", "gettingfar" },
                    { "distance", distance.ToString() }

                }.ToString()
            };
            SendNotification();
        }

        private void SendNotification() {
            // Create the toast notification
            var toast = new ToastNotification(_toastContent.GetXml());
            toast.ExpirationTime = DateTime.Now.AddDays(1);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
