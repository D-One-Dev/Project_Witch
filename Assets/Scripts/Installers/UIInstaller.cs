using TMPro;
using UnityEngine;
using Zenject;

public class UIInstaller : MonoInstaller
{
    [SerializeField] private GameObject inGameHintScreen;
    [SerializeField] private TMP_Text inGameHintText;

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
    }
}