using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIInstaller : MonoInstaller
{
    [SerializeField] private GameObject inGameHintScreen;
    [SerializeField] private TMP_Text inGameHintText;
    [SerializeField] private Image healthbar;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject dialogueScreen;
    [SerializeField] private TMP_Text dialogueText;

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
        this.Container.Bind<GameObject>()
            .WithId("InGameHintScreen")
            .FromInstance(inGameHintScreen)
            .AsTransient();
        this.Container.Bind<TMP_Text>()
            .WithId("InGameHintText")
            .FromInstance(inGameHintText)
            .AsTransient();
        this.Container.Bind<Image>()
            .WithId("Healthbar")
            .FromInstance(healthbar)
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
            .FromNew()
            .AsSingle();
    }
}