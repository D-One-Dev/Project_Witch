using Enemies.EnemyActions;

namespace Enemies.EnemyUnits
{
    public class SpinningGolemEnemyUnit : EnemyUnitBase.EnemyUnitBase
    {
        protected override void InitEnemy()
        {
            _enemy = new Enemy(_agent, transform, animator, this);
            _walkAction = new Idle();
            _chaseAction = new Chase(_player);
        }
    }
}