using UnityEngine;
using Zenject;

namespace Player
{
    public class AdditionalSkillsManager
    {
        [Inject(Id = "Telekinesis")]
        private readonly Telekinesis _telekinesis;
        
        private NewSpellCaster _newSpellCaster;
        private PlayerMovement _playerMovement;

        private Controls _controls;
        
        [Inject]
        public void Construct(Controls controls, PlayerMovement playerMovement, NewSpellCaster newSpellCaster)
        {
            _controls = controls;

            _newSpellCaster = newSpellCaster;
            _playerMovement = playerMovement;
            
            _controls.Gameplay.V.performed += ctx => TelekinesisActivate();

            controls.Enable();
        }

        private void TelekinesisActivate()
        {
            Debug.Log("bruh");
            _telekinesis.EnableControls();
            _playerMovement.DisableDash();
            _newSpellCaster.DisableControls();
        }

        public void TelekinesisDeactivate()
        {
            _telekinesis.DisableControls();
            _playerMovement.EnableDash();
            _newSpellCaster.EnableControls();
        }
    }
}