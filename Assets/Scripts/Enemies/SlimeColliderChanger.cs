using UnityEngine;

namespace Enemies
{
    public class SlimeColliderChanger : MonoBehaviour
    {
        [SerializeField] private CapsuleCollider capsuleCollider;

        public void OnIncrease()
        {
            capsuleCollider.height = 0.8147423f;
            capsuleCollider.center = new Vector3(capsuleCollider.center.x, 0.5673711f, capsuleCollider.center.z);
        }

        public void OnDecrease()
        {
            capsuleCollider.height = 0.3134208f;
            capsuleCollider.center = new Vector3(capsuleCollider.center.x, 0.2755657f, capsuleCollider.center.z);
        }
    }
}