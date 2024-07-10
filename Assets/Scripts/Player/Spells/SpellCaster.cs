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
    private Coroutine _castCoroutine;
    private List<ISpell> spells;
    private int currentMana;

    private void Awake()
    {
        _controls = new Controls();

        _controls.Gameplay.LMB.started += ctx => leftSpellCheck = true;
        _controls.Gameplay.LMB.canceled += ctx => leftSpellCheck = false;

        _controls.Gameplay.Shift.started += ctx => topSpellCheck = true;
        _controls.Gameplay.Shift.canceled += ctx => topSpellCheck = false;

        _controls.Gameplay.RMB.started += ctx => rightSpellCheck = true;
        _controls.Gameplay.RMB.canceled += ctx => rightSpellCheck = false;

        _controls.Gameplay.LControl.started += ctx => bottomSpellCheck = true;
        _controls.Gameplay.LControl.canceled += ctx => bottomSpellCheck = false;
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
        if (leftSpellCheck)
        {
            if (leftSpell.manaCost <= currentMana)
            {
                if (_castCoroutine != null) StopCoroutine(_castCoroutine);
                _castCoroutine = StartCoroutine(CastDelay());
                currentMana -= leftSpell.manaCost;
                ManaBarController.instance.UpdateFill(manaAmount, currentMana);
                spells.Add(leftSpell);
                leftSpellCheck = false;
            }
        }

        else if (topSpellCheck)
        {
            if (topSpell.manaCost <= currentMana)
            {
                if (_castCoroutine != null) StopCoroutine(_castCoroutine);
                _castCoroutine = StartCoroutine(CastDelay());
                currentMana -= topSpell.manaCost;
                ManaBarController.instance.UpdateFill(manaAmount, currentMana);
                spells.Add(topSpell);
                topSpellCheck = false;
            }
        }

        else if (rightSpellCheck)
        {
            if (rightSpell.manaCost <= currentMana)
            {
                if (_castCoroutine != null) StopCoroutine(_castCoroutine);
                _castCoroutine = StartCoroutine(CastDelay());
                currentMana -= rightSpell.manaCost;
                ManaBarController.instance.UpdateFill(manaAmount, currentMana);
                spells.Add(rightSpell);
                rightSpellCheck = false;
            }
        }

        else if (bottomSpellCheck)
        {
            if (bottomSpell.manaCost <= currentMana)
            {
                if (_castCoroutine != null) StopCoroutine(_castCoroutine);
                _castCoroutine = StartCoroutine(CastDelay());
                currentMana -= bottomSpell.manaCost;
                ManaBarController.instance.UpdateFill(manaAmount, currentMana);
                spells.Add(bottomSpell);
                bottomSpellCheck = false;
            }
        }
    }

    private IEnumerator CastDelay()
    {
        yield return new WaitForSeconds(castDelay);
        Cast();
        spells.Clear();
        currentMana = manaAmount;
        ManaBarController.instance.UpdateFill(manaAmount, currentMana);
        _castCoroutine = null;
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
