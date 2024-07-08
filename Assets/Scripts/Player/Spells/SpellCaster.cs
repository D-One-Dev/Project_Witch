using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    [SerializeField] private Transform _cam;
    [SerializeField] private Transform _projectilesParentObject;
    [SerializeField, SerializeReference]public ISpell spell;

    private Controls _controls;

    private void Awake()
    {
        _controls = new Controls();

        _controls.Gameplay.LMB.performed += ctx => LMB();
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
        if(spell.Type == SpellType.CastObject)
        {
            Debug.Log("Ura Pobeda");
        }
    }

    private void LMB()
    {
        if(spell.Type == SpellType.CastObject)
        {
            CastObjectSpell script = (CastObjectSpell)spell;
            GameObject obj = Instantiate(script.objectPrefab, _cam.position, Quaternion.identity, _projectilesParentObject);
            obj.transform.forward = _cam.transform.forward;
            obj.GetComponent<Rigidbody>().velocity = obj.transform.forward * script.objectSpeed;
        }
    }
}
