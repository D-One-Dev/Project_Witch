using UnityEngine;

public class InGameHint : MonoBehaviour
{
    public string hintTag;
    [SerializeField] private GameObject hintIcon;

    public void Activate()
    {
        hintIcon.SetActive(true);
    }

    public void Deactivate()
    {
        hintIcon.SetActive(false);
    }
}
