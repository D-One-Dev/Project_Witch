using UnityEngine;
using Zenject;

namespace Player
{
    public class Telekinesis : MonoBehaviour
    {
        [Header("Pickup Settings")] [SerializeField]
        private Transform holdArea;

        [SerializeField] private LayerMask _layermask;

        public GameObject heldObj;
        private Rigidbody heldObjRB;

        [Header("Physics Parameters")] [SerializeField]
        private float pickupRange = 5f;

        [SerializeField] private float pickupForce = 150f;
        [SerializeField] private float throwForce = 150f;
        
        private const int MaxSmallObjectMass = 20;
        
        private AdditionalSkillsManager _additionalSkillsManager;
        private Controls _controls;
        [Inject(Id = "Camera")]
        private readonly Transform _camera;
        [Inject(Id = "TelekinesisIcon")]
        private GameObject _telekinesisIcon;

        public Transform objectForRaycast;

        private AnimationsController _animationsController;
        
        [Inject]
        public void Construct(Controls controls, AdditionalSkillsManager additionalSkillsManager, AnimationsController animationsController)
        {
            _controls = controls;
            _additionalSkillsManager = additionalSkillsManager;
            _animationsController = animationsController;
            
            _controls.Gameplay.RMB.performed += ctx => OnRMBClick();
            
            controls.Enable();
        }

        private void FixedUpdate()
        {
            if (!_controls.Gameplay.RMB.IsPressed() && heldObj != null)
            {
                MoveObject();
            }
        }

        public void OnVClick()
        {
            _animationsController.ClickButton(_telekinesisIcon);
            if (heldObj == null)
            {
                RaycastHit hit;

                if (Physics.Raycast(objectForRaycast.position, objectForRaycast.forward, out hit, pickupRange, _layermask))
                {
                    PickupObject(hit.transform.gameObject);
                }
                else _additionalSkillsManager.TelekinesisDeactivate();
            }
            else
            {
                DropObject();
            }
        }

        private void OnRMBClick()
        {
            if (heldObj != null)
            {
                ThrowObject();
            }
        }

        private void MoveObject()
        {
            if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
            {
                Vector3 moveDirection = holdArea.position - heldObj.transform.position;
                heldObjRB.AddForce(moveDirection * pickupForce);
            }
        }

        private void PickupObject(GameObject pickObj)
        {
            _additionalSkillsManager.TelekinesisActivate();
            pickObj.TryGetComponent<Rigidbody>(out var pickObjRB);
            pickObj.TryGetComponent<PickableObject>(out var pickObjCharacteristic);

            if (pickObjRB != null && pickObjCharacteristic != null)
            {
                if (pickObjCharacteristic.mass > MaxSmallObjectMass) return;
                
                heldObjRB = pickObjRB;
                heldObjRB.useGravity = false;
                heldObjRB.drag = 10;

                heldObjRB.transform.parent = holdArea;
                heldObj = pickObj;
            }
        }

        public void DropObject()
        {
            heldObjRB.useGravity = true;
            heldObjRB.drag = 1;

            heldObjRB.transform.parent = null;
            heldObj = null;
            
            _additionalSkillsManager.TelekinesisDeactivate();
        }

        private void ThrowObject()
        {
            DropObject();
            heldObjRB.AddForce(_camera.forward * throwForce);
        }

        public void EnableControls()
        {
            _controls.Enable();
        }
        
        public void DisableControls()
        {
            _controls.Disable();
        }
    }
}