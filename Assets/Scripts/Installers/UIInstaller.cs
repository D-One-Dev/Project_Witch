using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIInstaller : MonoInstaller
{
    [SerializeField] private GameObject inGameHintScreen;
    [SerializeField] private TMP_Text inGameHintText;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject dialogueScreen;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image saveIconImage;
    [SerializeField] private Image bossHealthBar;
    [SerializeField] private Image playerHealthBar;
    [SerializeField] private RectTransform playerHealthBarParent;
    [SerializeField] private Image playerManaBar;
    [SerializeField] private RectTransform playerManaBarParent;
    [SerializeField] private Gradient rainbow;
    [SerializeField] private ShopUIController shopUIController;

    [Header("Spells UI")]
    [SerializeField] private GameObject leftSpellIcon;
    [SerializeField] private GameObject rightSpellIcon;
    [SerializeField] private GameObject leftEffectIcon;
    [SerializeField] private GameObject rightEffectIcon;
    [SerializeField] private Image leftSpellIconImage;
    [SerializeField] private Image rightSpellIconImage;
    [SerializeField] private Image leftEffectIconImage;
    [SerializeField] private Image rightEffectIconImage;

    public override void InstallBindings()
    {
        this.Container.Bind<MonoInstaller>()
            .WithId("UIInstaller")
            .FromInstance(this)
            .AsTransient();

        this.Container.Bind<GameObject>()
            .WithId("InGameHintScreen")
            .FromInstance(inGameHintScreen)
            .AsTransient();
        this.Container.Bind<TMP_Text>()
            .WithId("InGameHintText")
            .FromInstance(inGameHintText)
            .AsTransient();
        this.Container.Bind<GameObject>()
            .WithId("DeathScreen")
            .FromInstance(deathScreen)
            .AsTransient();

        this.Container.Bind<GameObject>()
            .WithId("LeftSpellIcon")
            .FromInstance(leftSpellIcon)
            .AsTransient();
        this.Container.Bind<GameObject>()
            .WithId("RightSpellIcon")
            .FromInstance(rightSpellIcon)
            .AsTransient();
        this.Container.Bind<GameObject>()
            .WithId("LeftEffectIcon")
            .FromInstance(leftEffectIcon)
            .AsTransient();
        this.Container.Bind<GameObject>()
            .WithId("RightEffectIcon")
            .FromInstance(rightEffectIcon)
            .AsTransient();

        this.Container.Bind<Image>()
            .WithId("LeftSpellIconImage")
            .FromInstance(leftSpellIconImage)
            .AsTransient();
        this.Container.Bind<Image>()
            .WithId("RightSpellIconImage")
            .FromInstance(rightSpellIconImage)
            .AsTransient();
        this.Container.Bind<Image>()
            .WithId("LeftEffectIconImage")
            .FromInstance(leftEffectIconImage)
            .AsTransient();
        this.Container.Bind<Image>()
            .WithId("RightEffectIconImage")
            .FromInstance(rightEffectIconImage)
            .AsTransient();

        this.Container.Bind<GameObject>()
            .WithId("DialogueScreen")
            .FromInstance(dialogueScreen)
            .AsTransient();
        this.Container.Bind<TMP_Text>()
            .WithId("DialogueText")
            .FromInstance(dialogueText)
            .AsTransient();

        this.Container.Bind<DialogueManager>()
            .WithId("InteractiveDialogueManager")
            .To<InteractiveDialogueManager>()
            .FromNew()
            .AsSingle();

        this.Container.Bind<Image>()
            .WithId("SaveIconImage")
            .FromInstance(saveIconImage)
            .AsTransient();

        this.Container.Bind<BossHealthUI>()
            .FromNew()
            .AsSingle();

        this.Container.Bind<Image>()
            .WithId("BossHealthBar")
            .FromInstance(bossHealthBar)
            .AsTransient();

        this.Container.Bind<Image>()
            .WithId("PlayerHealthBar")
            .FromInstance(playerHealthBar)
            .AsTransient();
        this.Container.Bind<RectTransform>()
            .WithId("PlayerHealthBarParent")
            .FromInstance(playerHealthBarParent)
            .AsTransient();
        this.Container.Bind<HPBarController>()
            .FromNew()
            .AsSingle();

        this.Container.Bind<Image>()
            .WithId("PlayerManaBar")
            .FromInstance(playerManaBar)
            .AsTransient();
        this.Container.Bind<RectTransform>()
            .WithId("PlayerManaBarParent")
            .FromInstance(playerManaBarParent)
            .AsTransient();
        this.Container.Bind<ManaBarController>()
            .FromNew()
            .AsSingle();

        this.Container.BindInterfacesAndSelfTo<TextAnimator>()
            .FromNew()
            .AsSingle()
            .NonLazy();

        this.Container.Bind<Gradient>()
            .WithId("Rainbow")
            .FromInstance(rainbow)
            .AsTransient();

        this.Container.Bind<ShopUIController>()
            .FromInstance(shopUIController)
            .AsSingle()
            .NonLazy();
    }
}