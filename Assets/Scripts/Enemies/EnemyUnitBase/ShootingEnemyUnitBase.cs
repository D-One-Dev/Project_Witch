using UnityEngine;

namespace Enemies.EnemyUnitBase
{
    public abstract class ShootingEnemyUnitBase : EnemyUnitBase
    {
        [SerializeField] private GameObject projectTile;
        public delegate void SpawnProjectTile();
        
        public Transform shootingPoint;

        protected void SpawnNewProjectTile()
        {
            Instantiate(projectTile, shootingPoint.position, shootingPoint.rotation);
        }
    }
}