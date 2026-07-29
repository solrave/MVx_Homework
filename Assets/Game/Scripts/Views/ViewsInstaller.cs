using Modules.UI;
using Modules.Utils;
using Zenject;

namespace Game.Views
{
    public sealed class ViewsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            //TODO:
            this.Container.BindInterfacesAndSelfTo<Countdown>()
                .AsSingle()
                .NonLazy();
            
            this.Container.BindInterfacesAndSelfTo<ParticleAnimator>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();
        }
    }
}