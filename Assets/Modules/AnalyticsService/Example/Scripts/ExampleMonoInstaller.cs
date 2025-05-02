using Modules.AnalyticsService.DI;
using Modules.Common.Interfaces;
using Zenject;

namespace Modules.AnalyticsService.Example
{
    public class ExampleMonoInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IMonoBehaviourCycle>()
                .To<ExampleMonoBehaviourCycle>()
                .FromNewComponentOnNewGameObject()
                .AsSingle();

            AnalyticsServiceInstaller.Install(Container);
        }
    }
}