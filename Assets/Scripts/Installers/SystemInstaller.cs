using UnityEngine;
using Zenject;

public class SystemInstaller : MonoInstaller
{
    [SerializeField] private BoughtSpellWriter boughtSpellWriter;
    public override void InstallBindings()
    {
        this.Container.BindInterfacesAndSelfTo<SavesController>()
            .FromNew()
            .AsSingle();

        this.Container.Bind<AnimationsController>()
            .FromNew()
            .AsSingle();
        
        this.Container.Bind<BoughtSpellWriter>()
            .FromInstance(boughtSpellWriter)
            .AsSingle();
    }
}