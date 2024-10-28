using System.Collections;
using Zenject;

public class InteractiveDialogueManager : DialogueManager
{
    protected Controls _controls;

    [Inject]
    public void Construct(Controls controls)
    {
        _controls = controls;

        PlayerHealth.OnPlayerDeath += DisableControls;
        _controls.Gameplay.Interact.performed += ctx => Trigger();
        _controls.Enable();
    }
    protected override IEnumerator OnPhraseEnd(float delayTime)
    {
        yield return null;
    }

    protected void DisableControls()
    {
        _controls.Disable();
    }
}
