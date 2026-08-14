using Modules.UI;
using Zenject;

namespace Game.Views
{
    public sealed class ViewsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.Bind<ParticleAnimator>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();   
        }
    }
}