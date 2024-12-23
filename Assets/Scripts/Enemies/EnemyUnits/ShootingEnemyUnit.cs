using Enemies.EnemyActions;
using Enemies.EnemyUnitBase;

namespace Enemies.EnemyUnits
{
    public class ShootingEnemyUnit : ShootingEnemyUnitBase
    {
        protected override void InitEnemy()
        {
            _enemy = new Enemy(_agent, transform, animator, this);
            _walkAction = new Walk(walkPointRange, groundLayer);
            _chaseAction = new Chase(_player);
            
            _attackAction = new ShootingAttackBaseWithCallback(_player, timeBetweenAttacks, "isAttacking", SpawnNewProjectTile);
        }
    }
}