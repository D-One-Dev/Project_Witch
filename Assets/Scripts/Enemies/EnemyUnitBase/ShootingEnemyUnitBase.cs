using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Enemies.EnemyUnitBase
{
    public abstract class ShootingEnemyUnitBase : EnemyUnitBase
    {
        [FormerlySerializedAs("projectTile")] [SerializeField] protected GameObject currentProjectTile;

        protected DiContainer _container;
        public delegate void SpawnProjectTile();
        
        public Transform shootingPoint;
        
        [Inject]
        private void Construct(DiContainer container)
        {
            _container = container;
        }

        protected void SpawnNewProjectTile()
        {
            _container.InstantiatePrefab(currentProjectTile, shootingPoint.position, shootingPoint.rotation, null);
        }
    }
}