using Modules.UI;
using Zenject;

namespace Game.Views
{
    public sealed class ViewsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<PlanetView>()
                .FromComponentsInHierarchy(includeInactive: true)
                .AsCached();
            
            this.Container.BindInterfacesAndSelfTo<PlanetPopupView>()
                .FromComponentInHierarchy()
                .AsSingle();
            
            this.Container.BindInterfacesAndSelfTo<ParticleAnimator>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();
            
            this.Container.BindInterfacesAndSelfTo<MoneyView>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();
            
            this.Container.BindInterfacesAndSelfTo<PlanetViewCatalog>()
                .AsSingle()
                .NonLazy();
        }
    }
}