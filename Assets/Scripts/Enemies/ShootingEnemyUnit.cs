using Projectiles;
using UnityEngine;

namespace Enemies
{
    public class ShootingEnemyUnit : EnemyUnitBase
    {
        [SerializeField] private GameObject projectTile;
        public Transform shootingPoint;
        
        public delegate void SpawnProjectTile();
        
        protected override void InitEnemy()
        {
            _enemy = new Enemy(_agent, transform, animator, this);
            _walkAction = new Walk(walkPointRange, groundLayer);
            _chaseAction = new Chase(_player);
            
            SpawnProjectTile spawnProjectTile = delegate
            {
                Instantiate(projectTile, shootingPoint.position, shootingPoint.rotation).GetComponent<Projectile>();
            };
            
            _attackAction = new ShootingAttack(_player, timeBetweenAttacks, spawnProjectTile);
        }
    }
}