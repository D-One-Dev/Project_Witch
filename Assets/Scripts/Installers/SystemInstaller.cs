using Zenject;

public class SystemInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        this.Container.BindInterfacesAndSelfTo<SavesController>()
            .FromNew()
            .AsSingle();

        this.Container.Bind<AnimationsController>()
            .FromNew()
            .AsSingle();


    }
}