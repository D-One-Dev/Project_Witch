using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PickableObject))]
    public class BecomingPickableObject : MonoBehaviour
    {
        private PickableObject _pickableObject;

        private MeshCollider _meshCollider;

        [SerializeField] private bool isAble;

        private void Awake()
        {
            _pickableObject = GetComponent<PickableObject>();
            _meshCollider = GetComponent<MeshCollider>();
            
            _pickableObject.enabled = false;
        }

        public void Become()
        {
            if (!isAble) return;
            
            transform.parent = null;
            
            _meshCollider.convex = true;

            Rigidbody _rigidBody = gameObject.AddComponent<Rigidbody>();
            
            _pickableObject.enabled = true;

            gameObject.layer = 13;

            tag = "Untagged";
        }
    }
}