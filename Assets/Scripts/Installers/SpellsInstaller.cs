using UnityEngine;
using Zenject;

public class SpellsInstaller : MonoInstaller
{
    [SerializeField] private Transform projectilesParentTransform;

    [SerializeField] private float castDelay;
    [SerializeField] private int manaAmount;
    [SerializeField] private float manaRefillSpeed;

    [Header("Secondary Spells")]
    [SerializeField] private Spell steamSpell;
    [SerializeField] private Spell lavaSpell;
    [SerializeField] private Spell icyRockSpell;
    [SerializeField] private Spell poisonedFireballSpell;
    [SerializeField] private Spell poisonedRockSpell;

    [Header("Current Spells and Effects")]
    [SerializeReference] public Spell leftSpell;
    [SerializeReference] public Spell rightSpell;
    [SerializeReference] public Effect leftEffect;
    [SerializeReference] public Effect rightEffect;
    public override void InstallBindings()
    {
        this.Container.Bind<MonoInstaller>()
            .WithId("SpellsInstaller")
            .FromInstance(this)
            .AsTransient();

        this.Container.Bind<Transform>()
            .WithId("ProjectilesParentTransform")
            .FromInstance(projectilesParentTransform)
            .AsTransient();

        this.Container.Bind<float>()
            .WithId("CastDelay")
            .FromInstance(castDelay)
            .AsTransient();
        this.Container.Bind<int>()
            .WithId("ManaAmount")
            .FromInstance(manaAmount)
            .AsTransient();
        this.Container.Bind<float>()
            .WithId("ManaRefillSpeed")
            .FromInstance(manaRefillSpeed)
            .AsTransient();

        this.Container.Bind<Spell>()
            .WithId("SteamSpell")
            .FromInstance(steamSpell)
            .AsTransient();
        this.Container.Bind<Spell>()
            .WithId("LavaSpell")
            .FromInstance(lavaSpell)
            .AsTransient();
        this.Container.Bind<Spell>()
            .WithId("IcyRockSpell")
            .FromInstance(icyRockSpell)
            .AsTransient();
        this.Container.Bind<Spell>()
            .WithId("PoisonedFireballSpell")
            .FromInstance(poisonedFireballSpell)
            .AsTransient();
        this.Container.Bind<Spell>()
            .WithId("PoisonedRockSpell")
            .FromInstance(poisonedRockSpell)
            .AsTransient();

        this.Container.Bind<Spell>()
            .WithId("LeftSpell")
            .FromInstance(leftSpell)
            .AsTransient();
        this.Container.Bind<Spell>()
            .WithId("RightSpell")
            .FromInstance(rightSpell)
            .AsTransient();
        this.Container.Bind<Effect>()
            .WithId("LeftEffect")
            .FromInstance(leftEffect)
            .AsTransient();
        this.Container.Bind<Effect>()
            .WithId("RightEffect")
            .FromInstance(rightEffect)
            .AsTransient();

        this.Container.BindInterfacesAndSelfTo<NewSpellCaster>()
            .FromNew()
            .AsSingle()
            .NonLazy();
    }
}