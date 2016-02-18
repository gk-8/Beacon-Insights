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

            //Services
            container.RegisterType<INotificationsService, NotificationsService>();
        }
        public MainPageViewModel MainPageViewModel
        {
            get { return container.Resolve<MainPageViewModel>(); }
        }
        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }
    }

}
