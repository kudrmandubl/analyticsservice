using Modules.AnalyticsService.Interfaces;
using Zenject;

namespace Modules.AnalyticsService.DI
{
    public class AnalyticsServiceInstaller : Installer<AnalyticsServiceInstaller>
    {
        /// <summary>
        /// Установить привязки
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AnalyticsService.Implementation.AnalyticsService>()
                .FromNew()
                .AsSingle();
        }
    }

}