using UnityEngine;

public class InGameHint : MonoBehaviour
{
    [TextArea]
    public string hintText;
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
