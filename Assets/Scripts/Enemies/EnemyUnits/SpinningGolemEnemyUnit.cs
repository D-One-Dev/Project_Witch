using System.Collections;
using Enemies.EnemyActions;
using UnityEngine;

namespace Enemies.EnemyUnits
{
    public class SpinningGolemEnemyUnit : EnemyUnitBase.EnemyUnitBase
    {
        [SerializeField] private float rushCooldown;
        
        private static readonly int IsRushing = Animator.StringToHash("isRushing");

        private float _defaultAcceleration;

        protected override void InitEnemy()
        {
            _enemy = new Enemy(_agent, transform, animator, this);
            _walkAction = new Idle();
            _chaseAction = new ChaseWithTrigger(_player, () => StartCoroutine(UpdateAttackState()));
            _attackAction = new Chase(_player);

            _defaultAcceleration = _agent.acceleration;
            
            StartCoroutine(UpdateAttackState());
        }

        private IEnumerator UpdateAttackState()
        {
            while (true)
            {
                yield return new WaitForSeconds(rushCooldown);
                StartCoroutine(RushToPlayer());
            }
        }

        private IEnumerator RushToPlayer()
        {
            _chaseAction = new Idle();
            _attackAction = new Idle();
            _agent.acceleration = 0;
            yield return new WaitForSeconds(1);
            animator.SetBool(IsRushing, true);
            yield return new WaitForSeconds(1);
            SetDefaultAttackMode();
            _agent.acceleration = _defaultAcceleration;
            _agent.acceleration *= 10;
            yield return new WaitForSeconds(2);
            _agent.acceleration /= 10;
            animator.SetBool(IsRushing, false);
        }

        private void SetDefaultAttackMode()
        {
            _chaseAction = new Chase(_player);
            _attackAction = new Chase(_player);
        }
    }
}