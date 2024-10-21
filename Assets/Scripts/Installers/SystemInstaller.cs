using UnityEngine;
using Zenject;

public class SystemInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        this.Container.Bind<SavesController>()
            .FromNew()
            .AsSingle();
    }
}