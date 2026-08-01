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

            this.Container.Bind<PlanetPopupPresenter>()
                .AsSingle().NonLazy();
            
            this.Container.BindInterfacesAndSelfTo<MoneyPresenter>()
                .AsSingle();
        }
    }
}