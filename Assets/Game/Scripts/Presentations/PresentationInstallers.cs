using UnityEngine;
using Zenject;

namespace Game.Presenters
{
    [CreateAssetMenu(
        fileName = "PresentersInstallers",
        menuName = "Zenject/New PresentersInstallers"
    )]
    public sealed class PresentationInstallers : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            this.Container.Bind<PlanetPopupPresentation>()
                .AsSingle().NonLazy();
            
            this.Container.BindInterfacesAndSelfTo<MoneyPresentation>()
                .AsSingle();
            
            this.Container.BindInterfacesAndSelfTo<PlanetPresentationCatalog>()
                .AsSingle()
                .NonLazy();
        }
    }
}