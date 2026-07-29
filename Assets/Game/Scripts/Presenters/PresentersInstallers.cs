//using Game.Gameplay;

using Game.Views;
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
            //TODO:
            this.Container.Bind<PlanetPresenter>()
                .FromComponentsInHierarchy()
                .AsCached();

            this.Container.Bind<PlanetPopupPresenter>()
                .FromComponentInHierarchy()
                .AsSingle();
            
            this.Container.BindInterfacesAndSelfTo<PlanetManager>()
                .AsSingle()
                .NonLazy();
            
            this.Container.BindInterfacesAndSelfTo<MoneyView>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();
            
            this.Container.BindInterfacesAndSelfTo<MoneyPresenter>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();
        }
    }
}