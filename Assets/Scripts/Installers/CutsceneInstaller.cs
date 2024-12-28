using UnityEngine;
using Zenject;
using TMPro;
using UnityEngine.UI;

public class CutsceneInstaller : MonoInstaller
{
    [SerializeField] private GameObject dialogueScreen;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Gradient rainbow;

    public override void InstallBindings()
    {
        this.Container.Bind<GameObject>()
            .WithId("DialogueScreen")
            .FromInstance(dialogueScreen)
            .AsTransient();
        this.Container.Bind<TMP_Text>()
            .WithId("DialogueText")
            .FromInstance(dialogueText)
            .AsTransient();

        this.Container.BindInterfacesAndSelfTo<SavesController>()
            .FromNew()
            .AsSingle();

        this.Container.BindInterfacesAndSelfTo<AnimationsController>()
            .FromNew()
            .AsSingle()
            .NonLazy();

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
        this.Container.Bind<ShopUIController>()
            .FromInstance(null)
            .AsSingle();
        this.Container.Bind<SettingsLoader>()
            .FromInstance(null)
            .AsSingle();

        this.Container.BindInterfacesAndSelfTo<TextAnimator>()
            .FromNew()
            .AsSingle()
            .NonLazy();
        this.Container.Bind<Gradient>()
            .WithId("Rainbow")
            .FromInstance(rainbow)
            .AsTransient();
    }
}
