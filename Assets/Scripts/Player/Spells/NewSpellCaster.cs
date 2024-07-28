using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSpellCaster : MonoBehaviour
{
    [SerializeField] private Transform _cam;
    [SerializeField] private Transform _projectilesParentObject;


    [SerializeField] private float castDelay;
    [SerializeField] private int manaAmount;

    [Header ("Secondary Spells")]
    [SerializeReference] private Spell steamSpell;
    [SerializeField] private Spell lavaSpell;
    [SerializeField] private Spell icyRockSpell;

    [Header("Current Spells and Effects")]
    [SerializeReference] private Spell leftSpell;
    [SerializeReference] private Spell rightSpell;
    [SerializeReference] public Effect leftEffect;
    [SerializeReference] public Effect rightEffect;

    [Header("UI")]
    [SerializeField] private GameObject leftSpellIcon;
    [SerializeField] private GameObject rightSpellIcon;
    [SerializeField] private GameObject leftEffectIcon;
    [SerializeField] private GameObject rightEffectIcon;

    private Controls _controls;

    private bool leftSpellCheck, rightSpellCheck;
    private bool leftSpellCheckComplete, rightSpellCheckComplete;
    private Coroutine _castCoroutine;

    private List<Spell> spells;
    private List<Effect> effects;

    private int currentMana;

    private void Awake()
    {
        _controls = new Controls();

        _controls.Gameplay.LMB.started += ctx =>
        {
            if (Time.timeScale > 0f)
            {
                leftSpellCheck = true;
            }
        };
        _controls.Gameplay.LMB.canceled += ctx =>
        {
            leftSpellCheck = false;
            leftSpellCheckComplete = false;
        };

        _controls.Gameplay.RMB.started += ctx =>
        {
            if (Time.timeScale > 0f)
            {
                rightSpellCheck = true;
            }
        }; 
        _controls.Gameplay.RMB.canceled += ctx =>
        {
            rightSpellCheck = false;
            rightSpellCheckComplete = false;
        };

        _controls.Gameplay.Q.performed += ctx =>
        {
            if (Time.timeScale > 0f)
            {
                if (leftEffect.ManaCost <= currentMana)
                {
                    currentMana -= leftEffect.ManaCost;
                    ManaBarController.instance.UpdateFill(manaAmount, currentMana);

                    effects.Add(leftEffect);
                }
                else ManaBarController.instance.ShakeManaBar();
                AnimationsController.instance.ClickButton(leftEffectIcon);
            };
        };

        _controls.Gameplay.E.performed += ctx =>
        {
            if (Time.timeScale > 0f)
            {
                if (rightEffect.ManaCost <= currentMana)
                {
                    currentMana -= rightEffect.ManaCost;
                    ManaBarController.instance.UpdateFill(manaAmount, currentMana);

                    effects.Add(rightEffect);
                }
                else ManaBarController.instance.ShakeManaBar();
                AnimationsController.instance.ClickButton(rightEffectIcon);
            };
        };
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void Start()
    {
        spells = new List<Spell>();
        effects = new List<Effect>();

        currentMana = manaAmount;
        ManaBarController.instance.UpdateFill(manaAmount, currentMana);
    }

    private void Update()
    {
        CheckSpells();
    }

    private void CheckSpells()
    {
        if (leftSpellCheck && !leftSpellCheckComplete)
        {
            if (_castCoroutine != null) StopCoroutine(_castCoroutine);
            _castCoroutine = StartCoroutine(CastDelay());
            if (leftSpell != null && leftSpell.manaCost <= currentMana)
            {
                currentMana -= leftSpell.manaCost;
                ManaBarController.instance.UpdateFill(manaAmount, currentMana);
                spells.Add(leftSpell);
            }
            else ManaBarController.instance.ShakeManaBar();
            leftSpellCheckComplete = true;


            AnimationsController.instance.ClickButton(leftSpellIcon);
        }

        else if (rightSpellCheck && !rightSpellCheckComplete)
        {
            if (_castCoroutine != null) StopCoroutine(_castCoroutine);
            _castCoroutine = StartCoroutine(CastDelay());
            if (rightSpell != null && rightSpell.manaCost <= currentMana)
            {
                currentMana -= rightSpell.manaCost;
                ManaBarController.instance.UpdateFill(manaAmount, currentMana);
                spells.Add(rightSpell);
            }
            else ManaBarController.instance.ShakeManaBar();
            rightSpellCheckComplete = true;

            AnimationsController.instance.ClickButton(rightSpellIcon);
        }
    }

    private IEnumerator CastDelay()
    {
        yield return new WaitForSeconds(castDelay);
        if ((leftSpellCheck && leftSpellCheckComplete) ||
            (rightSpellCheck && rightSpellCheckComplete))
        {
            if (leftSpellCheck && currentMana - leftSpell.manaCost >= 0)
            {
                currentMana -= leftSpell.manaCost;
                spells.Add(leftSpell);
            }
            if (rightSpellCheck && currentMana - rightSpell.manaCost >= 0)
            {
                currentMana -= rightSpell.manaCost;
                spells.Add(rightSpell);
            }
            ManaBarController.instance.UpdateFill(manaAmount, currentMana);
            StopCoroutine(_castCoroutine);
            _castCoroutine = StartCoroutine(CastDelay());
        }
        else
        {
            Cast();
            spells.Clear();
            effects.Clear();
            currentMana = manaAmount;
            ManaBarController.instance.UpdateFill(manaAmount, currentMana);
            leftSpellCheck = false;
            rightSpellCheck = false;
            leftSpellCheckComplete = false;
            rightSpellCheckComplete = false;
            _castCoroutine = null;
        }
    }

    private void Cast()
    {
        List<Spell> outputSpells = new List<Spell>();
        List<GameObject> objects = new List<GameObject>();
        while (spells.Count > 0)
        {
            if(spells.Count == 1)
            {
                outputSpells.Add(spells[0]);
                spells.RemoveAt(0);
            }
            SpellType spell1Type;
            SpellType spell2Type;
            for (int i = 0; i < spells.Count - 1; i++)
            {
                for (int j = i + 1; j < spells.Count; j++)
                {
                    spell1Type = spells[i].Type;
                    spell2Type = spells[j].Type;

                    if ((spell1Type == SpellType.Fire && spell2Type == SpellType.Ice) || (spell1Type == SpellType.Ice && spell2Type == SpellType.Fire))
                    {
                        outputSpells.Add(steamSpell);
                        spells.RemoveAt(i);
                        spells.RemoveAt(j - 1);
                    }

                    else if ((spell1Type == SpellType.Fire && spell2Type == SpellType.Earth) || (spell1Type == SpellType.Earth && spell2Type == SpellType.Fire))
                    {
                        outputSpells.Add(lavaSpell);
                        spells.RemoveAt(i);
                        spells.RemoveAt(j - 1);
                    }

                    else if ((spell1Type == SpellType.Ice && spell2Type == SpellType.Earth) || (spell1Type == SpellType.Earth && spell2Type == SpellType.Ice))
                    {
                        outputSpells.Add(icyRockSpell);
                        spells.RemoveAt(i);
                        spells.RemoveAt(j - 1);
                    }

                    else
                    {
                        outputSpells.Add(spells[i]);
                        spells.RemoveAt(i);
                    }
                }
            }
        }

        foreach (Spell spell in outputSpells)
        {
            Debug.Log(spell);
            Vector3 randomPos;
            if(outputSpells.Count < 2) randomPos = _cam.transform.position;
            else randomPos = _cam.position + (_cam.transform.right * Random.Range(-spell.objectPrefab.transform.localScale.x,
                spell.objectPrefab.transform.localScale.x) + _cam.transform.up * Random.Range(-spell.objectPrefab.transform.localScale.y,
                spell.objectPrefab.transform.localScale.y)) / 2;
            GameObject obj = Instantiate(spell.objectPrefab, randomPos, Quaternion.identity, _projectilesParentObject);
            obj.transform.forward = _cam.transform.forward;
            objects.Add(obj);
        }

        foreach (Effect effect in effects)
        {
            effect.Activate(objects);
        }
    }
}
