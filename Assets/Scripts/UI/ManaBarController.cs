using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ManaBarController
{
    [Inject(Id = "PlayerManaBar")]
    private readonly Image _manaBar;
    [Inject(Id = "PlayerManaBarParent")]
    private readonly RectTransform manaBarParent;

    private float _barTarget;

    private AnimationsController _animationsController;

    [Inject]
    public void Construct(AnimationsController animationsController)
    {
        _animationsController = animationsController;
    }

    public void UpdateFill(int maxMana, int currentMana)
    {
        _barTarget = currentMana / (float) maxMana;
        if(_manaBar.fillAmount > _barTarget)
            _animationsController.UpdateBar(_manaBar, _barTarget, manaBarParent, true);
        else _animationsController.UpdateBar(_manaBar, _barTarget, manaBarParent, false);
    }

    public void ShakeManaBar()
    {
        _animationsController.ShakeBar(manaBarParent);
    }
}