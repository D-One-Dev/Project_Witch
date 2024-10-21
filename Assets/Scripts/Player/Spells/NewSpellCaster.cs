using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class NewSpellCaster : IInitializable, ITickable
{
    [Inject(Id = "Camera")]
    private readonly Transform _cam;
    [Inject(Id = "ProjectilesParentTransform")]
    private readonly Transform _projectilesParentTransform;

    [Inject(Id = "CastDelay")]
    private readonly float _castDelay;
    [Inject(Id = "ManaAmount")]
    private readonly int _manaAmount;
    [Inject(Id = "ManaRefillSpeed")]
    private readonly float _manaRefillSpeed;

    [Inject(Id = "SteamSpell")]
    private readonly Spell _steamSpell;
    [Inject(Id = "LavaSpell")]
    private readonly Spell _lavaSpell;
    [Inject(Id = "IcyRockSpell")]
    private readonly Spell _icyRockSpell;
    [Inject(Id = "PoisonedFireballSpell")]
    private readonly Spell _poisonedFireballSpell;
    [Inject(Id = "PoisonedRockSpell")]
    private readonly Spell _poisonedRockSpell;

    [Inject(Id = "LeftSpell")]
    public Spell LeftSpell;
    [Inject(Id = "RightSpell")]
    public Spell RightSpell;
    [Inject(Id = "LeftEffect")]
    public Effect LeftEffect;
    [Inject(Id = "RightEffect")]
    public Effect RightEffect;

    [Inject(Id = "LeftSpellIcon")]
    private readonly GameObject _leftSpellIcon;
    [Inject(Id = "RightSpellIcon")]
    private readonly GameObject _rightSpellIcon;
    [Inject(Id = "LeftEffectIcon")]
    private readonly GameObject _leftEffectIcon;
    [Inject(Id = "RightEffectIcon")]
    private readonly GameObject _rightEffectIcon;

    [Inject(Id = "LeftSpellIconImage")]
    private readonly Image _leftSpellIconImage;
    [Inject(Id = "RightSpellIconImage")]
    private readonly Image _rightSpellIconImage;
    [Inject(Id = "LeftEffectIconImage")]
    private readonly Image _leftEffectIconImage;
    [Inject(Id = "RightEffectIconImage")]
    private readonly Image _rightEffectIconImage;
    [Inject(Id = "SpellsInstaller")]
    private readonly MonoInstaller _spellsInstaller;

    private Controls _controls;

    private bool _leftSpellCheck, _rightSpellCheck;
    private bool _leftSpellCheckComplete, _rightSpellCheckComplete;
    private Coroutine _castCoroutine;
    private Coroutine _manaRefillCoroutine;

    private List<Spell> _spells;
    private List<Effect> _effects;

    private int _currentMana;

    private Vector3 _tempCamPos;

    private AnimationsController _animationsController;
    private ManaBarController _manaBarController;

    private DiContainer _container;

    [Inject]
    public void Construct(Controls controls, DiContainer container, AnimationsController animationsController, ManaBarController manaBarController)
    {
        _container = container;
        _controls = controls;
        _animationsController = animationsController;
        _manaBarController = manaBarController;

        _controls.Gameplay.LMB.started += ctx =>
        {
            if (Time.timeScale > 0f)
            {
                _leftSpellCheck = true;
            }
        };
        _controls.Gameplay.LMB.canceled += ctx =>
        {
            _leftSpellCheck = false;
            _leftSpellCheckComplete = false;
        };

        _controls.Gameplay.RMB.started += ctx =>
        {
            if (Time.timeScale > 0f)
            {
                _rightSpellCheck = true;
            }
        };
        _controls.Gameplay.RMB.canceled += ctx =>
        {
            _rightSpellCheck = false;
            _rightSpellCheckComplete = false;
        };

        _controls.Gameplay.Q.performed += ctx =>
        {
            if (Time.timeScale > 0f)
            {
                if (LeftEffect.ManaCost <= _currentMana)
                {
                    _currentMana -= LeftEffect.ManaCost;
                    _manaBarController.UpdateFill(_manaAmount, _currentMana);

                    _effects.Add(LeftEffect);
                }
                else _manaBarController.ShakeManaBar();
                _animationsController.ClickButton(_leftEffectIcon);

                SetTempCamPos();
            };
        };

        _controls.Gameplay.E.performed += ctx =>
        {
            if (Time.timeScale > 0f)
            {
                if (RightEffect.ManaCost <= _currentMana)
                {
                    _currentMana -= RightEffect.ManaCost;
                    _manaBarController.UpdateFill(_manaAmount, _currentMana);

                    _effects.Add(RightEffect);
                }
                else _manaBarController.ShakeManaBar();
                _animationsController.ClickButton(_rightEffectIcon);

                SetTempCamPos();
            };
        };

        PlayerHealth.OnPlayerDeath += DisableControls;
        _controls.Enable();
    }

    public void Initialize()
    {
        _spells = new List<Spell>();
        _effects = new List<Effect>();

        _currentMana = _manaAmount;
        _manaBarController.UpdateFill(_manaAmount, _currentMana);
        UpdateSpellIcons();

        _manaRefillCoroutine = _spellsInstaller.StartCoroutine(RefillMana());
    }

    public void Tick()
    {
        CheckSpells();
    }

    private void CheckSpells()
    {
        if (_leftSpellCheck && !_leftSpellCheckComplete)
        {
            if (_castCoroutine != null) _spellsInstaller.StopCoroutine(_castCoroutine);
            _castCoroutine = _spellsInstaller.StartCoroutine(CastDelay());
            if (LeftSpell != null && LeftSpell.manaCost <= _currentMana)
            {
                _currentMana -= LeftSpell.manaCost;
                _manaBarController.UpdateFill(_manaAmount, _currentMana);
                _spells.Add(LeftSpell);
            }
            else _manaBarController.ShakeManaBar();
            _leftSpellCheckComplete = true;


            _animationsController.ClickButton(_leftSpellIcon);

            SetTempCamPos();
        }

        else if (_rightSpellCheck && !_rightSpellCheckComplete)
        {
            if (_castCoroutine != null) _spellsInstaller.StopCoroutine(_castCoroutine);
            _castCoroutine = _spellsInstaller.StartCoroutine(CastDelay());
            if (RightSpell != null && RightSpell.manaCost <= _currentMana)
            {
                _currentMana -= RightSpell.manaCost;
                _manaBarController.UpdateFill(_manaAmount, _currentMana);
                _spells.Add(RightSpell);
            }
            else _manaBarController.ShakeManaBar();
            _rightSpellCheckComplete = true;

            _animationsController.ClickButton(_rightSpellIcon);

            SetTempCamPos();
        }
    }

    private IEnumerator CastDelay()
    {
        yield return new WaitForSeconds(_castDelay);
        if ((_leftSpellCheck && _leftSpellCheckComplete) ||
            (_rightSpellCheck && _rightSpellCheckComplete))
        {
            if (_leftSpellCheck && _currentMana - LeftSpell.manaCost >= 0)
            {
                _currentMana -= LeftSpell.manaCost;
                _spells.Add(LeftSpell);
            }
            if (_rightSpellCheck && _currentMana - RightSpell.manaCost >= 0)
            {
                _currentMana -= RightSpell.manaCost;
                _spells.Add(RightSpell);
            }
            SetTempCamPos();
            _manaBarController.UpdateFill(_manaAmount, _currentMana);
            _spellsInstaller.StopCoroutine(_castCoroutine);
            _castCoroutine = _spellsInstaller.StartCoroutine(CastDelay());
        }
        else
        {
            Cast();
            _spells.Clear();
            _effects.Clear();
            _manaBarController.UpdateFill(_manaAmount, _currentMana);
            _leftSpellCheck = false;
            _rightSpellCheck = false;
            _leftSpellCheckComplete = false;
            _rightSpellCheckComplete = false;
            _castCoroutine = null;

            if (_manaRefillCoroutine == null) _manaRefillCoroutine = _spellsInstaller.StartCoroutine(RefillMana());
        }
    }

    private void Cast()
    {
        List<Spell> outputSpells = new List<Spell>();
        List<GameObject> objects = new List<GameObject>();
        while (_spells.Count > 0)
        {
            if(_spells.Count == 1)
            {
                outputSpells.Add(_spells[0]);
                _spells.RemoveAt(0);
            }
            SpellType spell1Type;
            SpellType spell2Type;
            for (int i = 0; i < _spells.Count - 1; i++)
            {
                for (int j = i + 1; j < _spells.Count; j++)
                {
                    spell1Type = _spells[i].Type;
                    spell2Type = _spells[j].Type;

                    if ((spell1Type == SpellType.Fire && spell2Type == SpellType.Ice) || (spell1Type == SpellType.Ice && spell2Type == SpellType.Fire))
                    {
                        outputSpells.Add(_steamSpell);
                        _spells.RemoveAt(i);
                        _spells.RemoveAt(j - 1);
                    }

                    else if ((spell1Type == SpellType.Fire && spell2Type == SpellType.Earth) || (spell1Type == SpellType.Earth && spell2Type == SpellType.Fire))
                    {
                        outputSpells.Add(_lavaSpell);
                        _spells.RemoveAt(i);
                        _spells.RemoveAt(j - 1);
                    }

                    else if ((spell1Type == SpellType.Ice && spell2Type == SpellType.Earth) || (spell1Type == SpellType.Earth && spell2Type == SpellType.Ice))
                    {
                        outputSpells.Add(_icyRockSpell);
                        _spells.RemoveAt(i);
                        _spells.RemoveAt(j - 1);
                    }

                    else if ((spell1Type == SpellType.Fire && spell2Type == SpellType.Poison) || (spell1Type == SpellType.Poison && spell2Type == SpellType.Fire))
                    {
                        outputSpells.Add(_poisonedFireballSpell);
                        _spells.RemoveAt(i);
                        _spells.RemoveAt(j - 1);
                    }

                    else if ((spell1Type == SpellType.Earth && spell2Type == SpellType.Poison) || (spell1Type == SpellType.Poison && spell2Type == SpellType.Earth))
                    {
                        outputSpells.Add(_poisonedRockSpell);
                        _spells.RemoveAt(i);
                        _spells.RemoveAt(j - 1);
                    }

                    else
                    {
                        outputSpells.Add(_spells[i]);
                        _spells.RemoveAt(i);
                    }
                }
            }
        }

        foreach (Spell spell in outputSpells)
        {
            Debug.Log(spell);

            GameObject spellPrefab = spell.objectPrefabs[Random.Range(0, spell.objectPrefabs.Length)];

            Vector3 randomPos;

            if (outputSpells.Count < 2) randomPos = _tempCamPos;

            else randomPos = _tempCamPos + (_cam.transform.right * Random.Range(-spellPrefab.transform.localScale.x,
                spellPrefab.transform.localScale.x) + _cam.transform.up * Random.Range(-spellPrefab.transform.localScale.y,
                spellPrefab.transform.localScale.y)) / 2;

            GameObject obj = _container.InstantiatePrefab(spellPrefab, randomPos, Quaternion.identity, _projectilesParentTransform);

            obj.transform.forward = _cam.transform.forward;

            obj.transform.localEulerAngles = new Vector3(obj.transform.localEulerAngles.x, obj.transform.localEulerAngles.y, Random.Range(0f, 360f));
            objects.Add(obj);
        }

        foreach (Effect effect in _effects)
        {
            effect.Activate(objects);
        }
    }

    public void ClearCastList()
    {
        _spells.Clear();
        _effects.Clear();
        if(_castCoroutine != null)
        {
            _spellsInstaller.StopCoroutine(_castCoroutine);
            _castCoroutine = null;
        }
        _currentMana = _manaAmount;
        _manaBarController.UpdateFill(_manaAmount, _currentMana);
    }

    public void UpdateSpellIcons()
    {
        _leftSpellIconImage.sprite = LeftSpell.spellIcon;
        _rightSpellIconImage.sprite = RightSpell.spellIcon;
        _leftEffectIconImage.sprite = LeftEffect.EffectIcon;
        _rightEffectIconImage.sprite = RightEffect.EffectIcon;
    }

    private void SetTempCamPos()
    {
        _tempCamPos = _cam.transform.position;
    }

    private IEnumerator RefillMana()
    {
        yield return new WaitForSeconds(_manaRefillSpeed);
        if (_currentMana < _manaAmount)
        {
            _currentMana++;
            _manaBarController.UpdateFill(_manaAmount, _currentMana);
        }
        if(_spells.Count > 0 || _effects.Count > 0)
        {
            _manaRefillCoroutine = null;
        }
        else
        {
            _manaRefillCoroutine = _spellsInstaller.StartCoroutine(RefillMana());
        }
    }

    private void DisableControls()
    {
        _controls.Disable();
    }
}