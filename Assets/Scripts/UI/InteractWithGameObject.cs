using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class InteractWithGameObject : MonoBehaviour
    {
        [SerializeField] private GameObject interactIcon;

        public UnityEvent[] onInteract;

        public void Interact()
        {
            for (int i = 0; i < onInteract.Length; i++)
            {
                onInteract[i].Invoke();
            }
        }

        public void Activate()
        {
            interactIcon.SetActive(true);
        }

        public void Deactivate()
        {
            interactIcon.SetActive(false);
        }
    }
}