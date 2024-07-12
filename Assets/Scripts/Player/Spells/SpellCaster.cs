using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    [SerializeField] private Transform _cam;
    [SerializeField] private Transform _projectilesParentObject;
    [SerializeField, SerializeReference] public ISpell leftSpell, topSpell, rightSpell, bottomSpell;
    [SerializeField] private float castDelay;
    [SerializeField] private int manaAmount;

    private Controls _controls;

    private bool leftSpellCheck, topSpellCheck, rightSpellCheck, bottomSpellCheck;
    private bool leftSpellCheckComplete, topSpellCheckComplete, rightSpellCheckComplete, bottomSpellCheckComplete;
    private Coroutine _castCoroutine;
    private List<ISpell> spells;
    private int currentMana;

    private void Awake()
    {
        _controls = new Controls();

        _controls.Gameplay.LMB.started += ctx => leftSpellCheck = true;
        _controls.Gameplay.LMB.canceled += ctx =>
        {
            leftSpellCheck = false;
            leftSpellCheckComplete = false;
        };

        _controls.Gameplay.Shift.started += ctx => topSpellCheck = true;
        _controls.Gameplay.Shift.canceled += ctx =>
        {
            topSpellCheck = false;
            topSpellCheckComplete = false;
        };

        _controls.Gameplay.RMB.started += ctx => rightSpellCheck = true;
        _controls.Gameplay.RMB.canceled += ctx =>
        {
            rightSpellCheck = false;
            rightSpellCheckComplete = false;
        };

        _controls.Gameplay.LControl.started += ctx => bottomSpellCheck = true;
        _controls.Gameplay.LControl.canceled += ctx =>
        {
            bottomSpellCheck = false;
            bottomSpellCheckComplete = false;
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
        spells = new List<ISpell>();
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
            if (leftSpell != null && leftSpell.manaCost <= currentMana)
            {
                if (_castCoroutine != null) StopCoroutine(_castCoroutine);
                _castCoroutine = StartCoroutine(CastDelay());
                currentMana -= leftSpell.manaCost;
                ManaBarController.instance.UpdateFill(manaAmount, currentMana);
                spells.Add(leftSpell);
                leftSpellCheckComplete = true;
            }
        }

        else if (topSpellCheck && !topSpellCheckComplete)
        {
            if (topSpell != null && topSpell.manaCost <= currentMana)
            {
                if (_castCoroutine != null) StopCoroutine(_castCoroutine);
                _castCoroutine = StartCoroutine(CastDelay());
                currentMana -= topSpell.manaCost;
                ManaBarController.instance.UpdateFill(manaAmount, currentMana);
                spells.Add(topSpell);
                topSpellCheckComplete = true;
            }
        }

        else if (rightSpellCheck && !rightSpellCheckComplete)
        {
            if (rightSpell != null && rightSpell.manaCost <= currentMana)
            {
                if (_castCoroutine != null) StopCoroutine(_castCoroutine);
                _castCoroutine = StartCoroutine(CastDelay());
                currentMana -= rightSpell.manaCost;
                ManaBarController.instance.UpdateFill(manaAmount, currentMana);
                spells.Add(rightSpell);
                rightSpellCheckComplete = true;
            }
        }

        else if (bottomSpellCheck && !bottomSpellCheckComplete)
        {
            if (bottomSpell != null && bottomSpell.manaCost <= currentMana)
            {
                if (_castCoroutine != null) StopCoroutine(_castCoroutine);
                _castCoroutine = StartCoroutine(CastDelay());
                currentMana -= bottomSpell.manaCost;
                ManaBarController.instance.UpdateFill(manaAmount, currentMana);
                spells.Add(bottomSpell);
                bottomSpellCheckComplete = true;
            }
        }
    }

    private IEnumerator CastDelay()
    {
        yield return new WaitForSeconds(castDelay);
        if((leftSpellCheck && leftSpellCheckComplete) ||
            (topSpellCheck && topSpellCheckComplete) ||
            (rightSpellCheck && rightSpellCheckComplete) ||
            (bottomSpellCheck && bottomSpellCheckComplete))
        {
            if (leftSpellCheck && currentMana - leftSpell.manaCost >= 0)
            {
                currentMana -= leftSpell.manaCost;
                spells.Add(leftSpell);
            }
            if (topSpellCheck && currentMana - topSpell.manaCost >= 0)
            {
                currentMana -= topSpell.manaCost;
                spells.Add(topSpell);
            }
            if (rightSpellCheck && currentMana - rightSpell.manaCost >= 0)
            {
                currentMana -= rightSpell.manaCost;
                spells.Add(rightSpell);
            }
            if (bottomSpellCheck && currentMana - bottomSpell.manaCost >= 0)
            {
                currentMana -= bottomSpell.manaCost;
                spells.Add(bottomSpell);
            }
            ManaBarController.instance.UpdateFill(manaAmount, currentMana);
            StopCoroutine(_castCoroutine);
            _castCoroutine = StartCoroutine(CastDelay());
        }
        else
        {
            Cast();
            spells.Clear();
            currentMana = manaAmount;
            ManaBarController.instance.UpdateFill(manaAmount, currentMana);
            leftSpellCheck = false;
            topSpellCheck = false;
            rightSpellCheck = false;
            bottomSpellCheck = false;
            leftSpellCheckComplete = false;
            topSpellCheckComplete = false;
            rightSpellCheckComplete = false;
            bottomSpellCheckComplete = false;
            _castCoroutine = null;
        }
    }

    private void Cast()
    {
        SortSpells();
        List<GameObject> objects = new List<GameObject>();
        foreach (ISpell spell in spells)
        {
            Debug.Log(spell);

            if(spell.Type == SpellType.CastObject)
            {
                CastObjectSpell script = (CastObjectSpell)spell;
                GameObject obj = Instantiate(script.objectPrefab, _cam.position, Quaternion.identity, _projectilesParentObject);
                obj.transform.forward = _cam.transform.forward;
                obj.GetComponent<Rigidbody>().velocity = obj.transform.forward * script.objectSpeed;
                objects.Add(obj);
            }
            else if (spell.Type == SpellType.EffectObject)
            {
                EffectObjectSpell script = (EffectObjectSpell)spell;
                if(script.effect == EffectObjectSpell.Effect.SizeIncrease)
                {
                    foreach (GameObject obj in objects)
                    {
                        obj.transform.localScale *= 1.5f;
                    }
                }
            }
        }
    }

    private void SortSpells()
    {
        int length = spells.Count;
        for(int i = 1; i < length; i++)
        {
            for(int j = 0; j < length - i; j++)
            {
                if (spells[j].Type > spells[j + 1].Type)
                {
                    (spells[j], spells[j + 1]) = (spells[j + 1], spells[j]);
                }
            }
        }
    }
}
