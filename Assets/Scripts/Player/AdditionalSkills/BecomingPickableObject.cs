using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PickableObject))]
    public class BecomingPickableObject : MonoBehaviour
    {
        [SerializeField] private GameObject parent;
        private Rigidbody _rigidBody;
        private PickableObject _pickableObject;

        private MeshCollider _meshCollider;

        private void Awake()
        {
            /*_rigidBody = GetComponent<Rigidbody>();
            _pickableObject = GetComponent<PickableObject>();
            _meshCollider = GetComponent<MeshCollider>();
            
            _rigidBody.isKinematic = true;
            _pickableObject.enabled = false;

            _meshCollider.convex = true;*/
        }

        public void Become()
        {
            /*Vector3 localPosition = transform.localPosition;
            Quaternion localRotation = transform.localRotation;
    
            // Вытаскиваем дочерний объект
            transform.SetParent(null, true);
    
            // Корректируем позицию и вращение
            transform.position = parent.transform.TransformPoint(localPosition);
            transform.rotation = parent.transform.rotation * localRotation;
            
            _rigidBody.isKinematic = false;
            _pickableObject.enabled = true;

            gameObject.layer = 13;
            
            _meshCollider.convex = true;

            tag = "Untagged";*/
        }
    }
}