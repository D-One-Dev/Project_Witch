using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeReference] public Spell leftSpell;
    [SerializeReference] public Spell rightSpell;
    [SerializeReference] public Effect leftEffect;
    [SerializeReference] public Effect rightEffect;

    [Header("UI")]
    [SerializeField] private GameObject leftSpellIcon;
    [SerializeField] private GameObject rightSpellIcon;
    [SerializeField] private GameObject leftEffectIcon;
    [SerializeField] private GameObject rightEffectIcon;
    [SerializeField] private Image leftSpellIconImage;
    [SerializeField] private Image rightSpellIconImage;
    [SerializeField] private Image leftEffectIconImage;
    [SerializeField] private Image rightEffectIconImage;

    private Controls _controls;

    private bool leftSpellCheck, rightSpellCheck;
    private bool leftSpellCheckComplete, rightSpellCheckComplete;
    private Coroutine _castCoroutine;

    private List<Spell> spells;
    private List<Effect> effects;

    private int currentMana;

    private Vector3 tempCamPos;
    //private Vector3 tempCamUp;
    //private Vector3 tempCamForward;
    //private Vector3 tempCamRight;

    public static NewSpellCaster instance;

    private void Awake()
    {
        instance = this;

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

                SetTempCamPos();
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

                SetTempCamPos();
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
        UpdateSpellIcons();
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

            SetTempCamPos();
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

            SetTempCamPos();
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
            SetTempCamPos();
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

            GameObject spellPrefab = spell.objectPrefabs[Random.Range(0, spell.objectPrefabs.Length)];

            Vector3 randomPos;

            if (outputSpells.Count < 2) randomPos = tempCamPos;

            //else randomPos = tempCamPos + (tempCamRight * Random.Range(-spellPrefab.transform.localScale.x,
            //    spellPrefab.transform.localScale.x) + tempCamUp * Random.Range(-spellPrefab.transform.localScale.y,
            //    spellPrefab.transform.localScale.y)) / 2;

            else randomPos = tempCamPos + (_cam.transform.right * Random.Range(-spellPrefab.transform.localScale.x,
                spellPrefab.transform.localScale.x) + _cam.transform.up * Random.Range(-spellPrefab.transform.localScale.y,
                spellPrefab.transform.localScale.y)) / 2;

            GameObject obj = Instantiate(spellPrefab, randomPos, Quaternion.identity, _projectilesParentObject);

            //obj.transform.forward = tempCamForward;

            obj.transform.forward = _cam.transform.forward;

            obj.transform.localEulerAngles = new Vector3(obj.transform.localEulerAngles.x, obj.transform.localEulerAngles.y, Random.Range(0f, 360f));
            objects.Add(obj);
        }

        foreach (Effect effect in effects)
        {
            effect.Activate(objects);
        }
    }

    public void ClearCastList()
    {
        spells.Clear();
        effects.Clear();
        if(_castCoroutine != null)
        {
            StopCoroutine(_castCoroutine);
            _castCoroutine = null;
        }
        currentMana = manaAmount;
        ManaBarController.instance.UpdateFill(manaAmount, currentMana);
    }

    public void UpdateSpellIcons()
    {
        leftSpellIconImage.sprite = leftSpell.spellIcon;
        rightSpellIconImage.sprite = rightSpell.spellIcon;
        leftEffectIconImage.sprite = leftEffect.EffectIcon;
        rightEffectIconImage.sprite = rightEffect.EffectIcon;
    }

    private void SetTempCamPos()
    {
        tempCamPos = _cam.transform.position;
        //tempCamUp = _cam.transform.up;
        //tempCamForward = _cam.transform.forward;
        //tempCamRight = _cam.transform.right;
    }
}