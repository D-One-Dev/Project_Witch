using UnityEngine;
using Zenject;

namespace Enemies.EnemyUnitBase
{
    public abstract class ShootingEnemyUnitBase : EnemyUnitBase
    {
        [SerializeField] protected GameObject projectTile;

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
            _container.InstantiatePrefab(projectTile, shootingPoint.position, shootingPoint.rotation, null);
        }
    }
}