using Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform playerTransform;
    
    [SerializeField] private Telekinesis telekinesis;
    
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private Transform cam;
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioSource dialogueAudioSource;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashCooldownTime;
    [SerializeField] private Image dashIcon;
    [SerializeField] private float jumpHelpTime;
    [SerializeField] private float maxJumpAngle;
    [SerializeField] private Sprite effectDisabledSprite;
    [SerializeField] private Sprite effectEnabledSprite;
    [SerializeField] private float defaultMouseSens;
    [SerializeField] private AudioClip[] playerFootstepsDefault, playerFootstepsStone, playerFootstepsGrass;
    [SerializeField] private LayerMask collisionLayerMask;
    [SerializeField] private float timeToStartHeal;
    [SerializeField] private float healSpeed;
    
    [SerializeField] private SettingsLoader settingsLoader;
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private SoundBase playerSoundBase;
    [SerializeField] private AudioClip alexHurtSound;
    
    public override void InstallBindings()
    {
        this.Container.Bind<SettingsLoader>()
            .FromInstance(settingsLoader)
            .AsTransient()
            .NonLazy();
        
        this.Container.Bind<Controls>()
            .FromNew()
            .AsTransient();

        this.Container.Bind<MonoInstaller>()
            .WithId("PlayerInstaller")
            .FromInstance(this)
            .AsSingle();

        this.Container.Bind<CharacterController>()
            .FromInstance(characterController)
            .AsSingle();
        this.Container.Bind<Transform>()
            .WithId("PlayerTransform")
            .FromInstance(playerTransform)
            .AsTransient();
        this.Container.Bind<float>()
            .WithId("MovementSpeed")
            .FromInstance(movementSpeed)
            .AsTransient();
        this.Container.Bind<float>()
            .WithId("JumpSpeed")
            .FromInstance(jumpSpeed)
            .AsTransient();
        this.Container.Bind<float>()
            .WithId("Gravity")
            .FromInstance(gravity)
            .AsTransient();
        this.Container.Bind<Transform>()
            .WithId("Camera")
            .FromInstance(cam)
            .AsTransient();
        this.Container.Bind<AudioSource>()
            .WithId("PlayerAudioSource")
            .FromInstance(playerAudioSource)
            .AsTransient();
        this.Container.Bind<AudioSource>()
            .WithId("DialogueAudioSource")
            .FromInstance(dialogueAudioSource)
            .AsTransient();
        this.Container.Bind<AudioClip>()
            .WithId("JumpSound")
            .FromInstance(jumpSound)
            .AsTransient();
        this.Container.Bind<float>()
            .WithId("DashDistance")
            .FromInstance(dashDistance)
            .AsTransient();
        this.Container.Bind<float>()
            .WithId("DashCooldownTime")
            .FromInstance(dashCooldownTime)
            .AsTransient();
        this.Container.Bind<Image>()
            .WithId("DashIcon")
            .FromInstance(dashIcon)
            .AsTransient();
        this.Container.Bind<float>()
            .WithId("JumpHelpTime")
            .FromInstance(jumpHelpTime)
            .AsTransient();
        this.Container.Bind<float>()
            .WithId("MaxJumpAngle")
            .FromInstance(maxJumpAngle)
            .AsTransient();
        this.Container.Bind<Sprite>()
            .WithId("EffectEnabledSprite")
            .FromInstance(effectEnabledSprite)
            .AsTransient();
        this.Container.Bind<Sprite>()
            .WithId("EffectDisabledSprite")
            .FromInstance(effectDisabledSprite)
            .AsTransient();

        this.Container.BindInterfacesAndSelfTo<PlayerMovement>()
            .FromNew()
            .AsSingle()
            .NonLazy();

        this.Container.Bind<PlayerMoney>()
            .FromNew()
            .AsSingle();

        this.Container.Bind<float>()
            .WithId("DefaultMouseSens")
            .FromInstance(defaultMouseSens)
            .AsTransient();

        this.Container.BindInterfacesAndSelfTo<CameraLook>()
            .FromNew()
            .AsSingle()
            .NonLazy();

        this.Container.Bind<AudioClip[]>()
            .WithId("PlayerFootstepsDefault")
            .FromInstance(playerFootstepsDefault)
            .AsTransient();
        this.Container.Bind<AudioClip[]>()
            .WithId("PlayerFootstepsStone")
            .FromInstance(playerFootstepsStone)
            .AsTransient();
        this.Container.Bind<AudioClip[]>()
            .WithId("PlayerFootstepsGrass")
            .FromInstance(playerFootstepsGrass)
            .AsTransient();
        this.Container.Bind<LayerMask>()
            .WithId("PlayerLayerMask")
            .FromInstance(collisionLayerMask)
            .AsTransient();

        this.Container.BindInterfacesAndSelfTo<PlayerFootsteps>()
            .FromNew()
            .AsSingle()
            .NonLazy();
        
        this.Container.Bind<Telekinesis>()
            .WithId("Telekinesis")
            .FromInstance(telekinesis)
            .AsSingle();
        
        this.Container.Bind<AdditionalSkillsManager>()
            .FromNew()
            .AsSingle()
            .NonLazy();

        this.Container.Bind<float>()
            .WithId("TimeToStartHeal")
            .FromInstance(timeToStartHeal)
            .AsTransient();

        this.Container.Bind<float>()
            .WithId("HealSpeed")
            .FromInstance(healSpeed)
            .AsTransient();
            
        this.Container.Bind<AudioClip>()
            .WithId("DashSound")
            .FromInstance(dashSound)
            .AsTransient();

        this.Container.Bind<SoundBase>()
            .WithId("PlayerSoundBase")
            .FromInstance(playerSoundBase)
            .AsTransient();
        
        this.Container.Bind<AudioClip>()
            .WithId("AlexHurtSound")
            .FromInstance(alexHurtSound)
            .AsTransient();
    }
}