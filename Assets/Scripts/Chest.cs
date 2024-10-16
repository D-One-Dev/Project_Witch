using UnityEngine;
using Zenject;

public class Chest : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem[] particles;
    [SerializeField] private GameObject icon;
    [SerializeField] private int moneyAmount;
    private bool isActive;
    private Controls controls;
    private PlayerMoney _playerMoney;

    [Inject]
    public void Construct(PlayerMoney playerMoney)
    {
        _playerMoney = playerMoney;
    }

    private void Awake()
    {
        controls = new Controls();
        controls.Gameplay.Interact.performed += ctx => Open();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    private void Open()
    {
        if (isActive)
        {
            _playerMoney.ChangeBalance(moneyAmount);
            icon.SetActive(false);
            animator.SetTrigger("Open");
        }
    }

    public void PlayParticles()
    {
        foreach (ParticleSystem particle in particles)
        {
            particle.Play();
            icon.SetActive(false);
            Destroy(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActive = true;
            icon.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActive = false;
            icon.SetActive(false);
        }
    }
}
