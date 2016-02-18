using BeaconInsightsUWP.Services;
using BeaconInsightsUWP.Services.Interfaces;
using BeaconInsightsUWP.ViewModels;
using Microsoft.Practices.Unity;

namespace BeaconInsightsUWP.Base
{
    public class Locator
    {
        private IUnityContainer container;

        public Locator()
        {
            container = new UnityContainer();

            // ViewModels
            container.RegisterType<MainPageViewModel>();
            container.RegisterType<DetailPageViewModel>();
            container.RegisterType<RadarPageViewModel>();
            container.RegisterType<AdvertisingPageViewModel>();
            container.RegisterType<NotificationsPageViewModel>();
            container.RegisterType<CloudPageViewModel>();

            //Services
            container.RegisterType<INotificationsService, NotificationsService>();
        }
        public MainPageViewModel MainPageViewModel
        {
            get { return container.Resolve<MainPageViewModel>(); }
        }
        public DetailPageViewModel DetailPageViewModel
        {
            get { return container.Resolve<DetailPageViewModel>(); }
        }
        public RadarPageViewModel RadarPageViewModel
        {
            get { return container.Resolve<RadarPageViewModel>(); }
        }
        public AdvertisingPageViewModel AdvertisingPageViewModel
        {
            get { return container.Resolve<AdvertisingPageViewModel>(); }
        }
        public NotificationsPageViewModel NotificationsPageViewModel
        {
            get { return container.Resolve<NotificationsPageViewModel>(); }
        }
        public CloudPageViewModel CloudPageViewModel
        {
            get { return container.Resolve<CloudPageViewModel>(); }
        }

        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }
    }

}
