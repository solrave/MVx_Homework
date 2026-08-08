using Modules.UI;
using UnityEngine;
using Zenject;

namespace Game.Presenters
{
    [CreateAssetMenu(
        fileName = "PresentersInstallers",
        menuName = "Zenject/New PresentersInstallers"
    )]
    public sealed class PresentersInstallers : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            this.Container.Bind<PlanetPresenter>()
                .FromComponentsInHierarchy()
                .AsCached();

            this.Container.Bind<PlanetPopupPresenter>()
                .FromComponentInHierarchy()
                .AsSingle();
            
            this.Container.Bind<PlanetCollectionPresenter>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();
            
            this.Container.BindInterfacesAndSelfTo<MoneyPresenter>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();
            
            this.Container.BindInterfacesAndSelfTo<ParticleAnimator>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();
        }
    }
}