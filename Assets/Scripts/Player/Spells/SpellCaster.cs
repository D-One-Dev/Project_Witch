using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    [SerializeField, SerializeReference]public ISpell spell;

    private void Start()
    {
        if(spell.Type == SpellType.CastObject)
        {
            Debug.Log("Ura Pobeda");
        }
    }
}
