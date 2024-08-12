using UnityEngine;

namespace Enemies
{
    public class LongDistanceEnemyUnit : EnemyUnitBase
    {
        [SerializeField] private GameObject projectTile;
        
        public delegate void SpawnProjectTile();
        
        protected override void InitEnemy()
        {
            _enemy = new Enemy(_agent, transform, this);
            _walkAction = new Walk(walkPointRange, groundLayer);
            _chaseAction = new Chase(_player);
            
            SpawnProjectTile spawnProjectTile = delegate
            {
                Projectile pj = Instantiate(projectTile, transform.position, Quaternion.identity).GetComponent<Projectile>();
                pj.SetTargetLayer(playerLayer);
            };
            
            _attackAction = new LongDistanceAttack(_player, timeBetweenAttacks, spawnProjectTile);
        }
    }
}