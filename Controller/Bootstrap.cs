using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using FluxWork.Services;

namespace FluxWork.Controller
{
    public class Bootstrap
    {
        private static Bootstrap _instance;

        public IDependencyService DependencyService { get; }

        private Bootstrap()
        {
            this.DependencyService = (IDependencyService) new DependencyService();
            this.App = new Application();
        }

        public static Bootstrap Singleton => Bootstrap._instance ?? (Bootstrap._instance = new Bootstrap());

        public Application App { get; }

        public void Boot(string title)
        {
            this.DependencyService.Init();
            this.DependencyService.Find<IWindowService>().TitleSuffix = title;
        }

        public void Run()
        {
            var windowService = this.DependencyService.Find<IWindowService>();
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CurrentCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CurrentCulture;
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof (FrameworkElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            this.App.Run((Window) windowService.BaseWindow);
        }
    }
}