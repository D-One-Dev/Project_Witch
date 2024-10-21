using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HPBarController
{
    [Inject(Id = "PlayerHealthBar")]
    private readonly Image _playerHealthBar;
    [Inject(Id = "PlayerHealthBarParent")]
    private readonly RectTransform _playerHealthBarParent;

    private AnimationsController _animationsController;
    private float _barTarget;

    [Inject]
    public void Construct(AnimationsController animationsController)
    {
        _animationsController = animationsController;
    }

    public void UpdateFill(int maxHp, int currentHp)
    {
        _barTarget = currentHp / (float) maxHp;
        if (_playerHealthBar.fillAmount > _barTarget)
            _animationsController.UpdateBar(_playerHealthBar, _barTarget, _playerHealthBarParent, true);
        else _animationsController.UpdateBar(_playerHealthBar, _barTarget, _playerHealthBarParent, false);
    }
}