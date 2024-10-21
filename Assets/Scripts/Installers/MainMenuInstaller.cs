using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuInstaller : MonoInstaller
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Button loadGameButton;
    public override void InstallBindings()
    {
        this.Container.Bind<GameObject>()
            .WithId("LoadingScreen")
            .FromInstance(loadingScreen)
            .AsTransient();
        this.Container.Bind<Button>()
            .WithId("LoadGameButton")
            .FromInstance(loadGameButton)
            .AsTransient();

        this.Container.BindInterfacesAndSelfTo<SavesController>()
            .FromNew()
            .AsSingle();

        this.Container.Bind<Transform>()
            .WithId("PlayerTransform")
            .FromInstance(null)
            .AsTransient();
        this.Container.Bind<Image>()
            .WithId("SaveIconImage")
            .FromInstance(null)
            .AsTransient();
        this.Container.Bind<PlayerMoney>()
            .FromInstance(null)
            .AsSingle();
        this.Container.Bind<NewSpellCaster>()
            .FromInstance(null)
            .AsSingle();
        this.Container.Bind<AnimationsController>()
            .FromNew()
            .AsSingle();
    }
}