using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EnemyBossHealth))]
    public class GreatMageBossEnemyUnit : EnemyUnitBase
    {
        [SerializeField] private GameObject centerPointPrefab;
        private Transform _centerPoint;

        protected override void InitEnemy()
        {
            _centerPoint = Instantiate(centerPointPrefab, transform.position, Quaternion.identity).transform;
            
            _enemy = new Enemy(_agent, transform, animator, this);
            _walkAction = new TeleportationInRadiusWalk(walkPointRange, _centerPoint);
            _chaseAction = new Chase(_player);
        }

        protected override void CheckState()
        {
        }
    }
}