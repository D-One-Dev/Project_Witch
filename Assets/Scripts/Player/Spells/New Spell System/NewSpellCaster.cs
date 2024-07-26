using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSpellCaster : MonoBehaviour
{
    [SerializeField] private Transform _cam;
    [SerializeField] private Transform _projectilesParentObject;
    //[SerializeReference] public ISpell leftSpell, rightSpell, leftEffect, rightEffect;

    [SerializeReference] private Spell leftSpell, rightSpell;
    [SerializeField, SerializeReference] public Effect leftEffect, rightEffect;

    [SerializeField] private float castDelay;
    [SerializeField] private int manaAmount;

    private Controls _controls;

    private bool leftSpellCheck, rightSpellCheck;
    private bool leftSpellCheckComplete, rightSpellCheckComplete;
    private Coroutine _castCoroutine;
    //private List<ISpell> spells;

    private List<Spell> spells;
    private List<Effect> effects;

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

        _controls.Gameplay.RMB.started += ctx => rightSpellCheck = true;
        _controls.Gameplay.RMB.canceled += ctx =>
        {
            rightSpellCheck = false;
            rightSpellCheckComplete = false;
        };

        _controls.Gameplay.Q.performed += ctx =>
        {
            if (leftEffect.ManaCost <= currentMana)
            {
                currentMana -= leftEffect.ManaCost;
                ManaBarController.instance.UpdateFill(manaAmount, currentMana);
                //spells.Add(leftEffect);

                effects.Add(leftEffect);
            }
        };

        _controls.Gameplay.E.performed += ctx =>
        {
            if (rightEffect.ManaCost <= currentMana)
            {
                currentMana -= rightEffect.ManaCost;
                ManaBarController.instance.UpdateFill(manaAmount, currentMana);
                //spells.Add(rightEffect);

                effects.Add(rightEffect);
            }
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
        //spells = new List<ISpell>();

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
            leftSpellCheckComplete = true;
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
            rightSpellCheckComplete = true;
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
        //SortSpells();
        List<GameObject> objects = new List<GameObject>();
        //foreach (ISpell spell in spells)
        foreach (Spell spell in spells)
        {
            Debug.Log(spell);

            //if (spell.Type == SpellType.CastObject)
            //{
            //CastObjectSpell script = (CastObjectSpell)spell;

            //GameObject obj = Instantiate(script.objectPrefab, _cam.position, Quaternion.identity, _projectilesParentObject);
            GameObject obj = Instantiate(spell.objectPrefab, _cam.position, Quaternion.identity, _projectilesParentObject);
            obj.transform.forward = _cam.transform.forward;
                //obj.GetComponent<Rigidbody>().velocity = obj.transform.forward * script.objectSpeed;
                objects.Add(obj);
            //}
            //else if (spell.Type == SpellType.EffectObject)
            //{
            //    EffectObjectSpell script = (EffectObjectSpell)spell;
            //    if (script.effect == EffectObjectSpell.Effect.SizeIncrease)
            //    {
            //        foreach (GameObject obj in objects)
            //        {
            //            obj.transform.localScale *= 1.5f;
            //        }
            //    }
            //}
        }

        foreach (Effect effect in effects)
        {
            effect.Activate(objects);
        }


    }

    //private void SortSpells()
    //{
    //    int length = spells.Count;
    //    for (int i = 1; i < length; i++)
    //    {
    //        for (int j = 0; j < length - i; j++)
    //        {
    //            if (spells[j].Type > spells[j + 1].Type)
    //            {
    //                (spells[j], spells[j + 1]) = (spells[j + 1], spells[j]);
    //            }
    //        }
    //    }
    //}
}
