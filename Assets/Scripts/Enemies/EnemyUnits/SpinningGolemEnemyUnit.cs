using System.Collections;
using Enemies.EnemyActions;
using UnityEngine;

namespace Enemies.EnemyUnits
{
    public class SpinningGolemEnemyUnit : EnemyUnitBase.EnemyUnitBase
    {
        [SerializeField] private float rushCooldown;
        
        private static readonly int IsRushing = Animator.StringToHash("isRushing");

        private IAction defaultChaseAction;
        private IAction _deathAction;

        private float _defaultAcceleration;

        private SphereCollider _attackArea;

        protected override void InitEnemy()
        {
            _enemy = new Enemy(_agent, transform, animator, this);
            _walkAction = new Idle();
            
            ChangeEnemyTag("Untagged");
            
            _chaseAction = new ChaseWithTrigger(_player, () =>
            {
                ChangeEnemyTag("Enemy");
                
                StartCoroutine(UpdateAttackState());
                animator.SetTrigger("isAwake");
                SetDefaultAttackMode();
                _attackArea.enabled = true;
            });

            _attackAction = new Idle();
            
            _deathAction = new Death("isDead");

            defaultChaseAction = new Chase(_player);
            _defaultAcceleration = _agent.acceleration;

            _attackArea = GetComponent<SphereCollider>();
            _attackArea.enabled = false;
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
            _chaseAction = defaultChaseAction;
            _attackAction = defaultChaseAction;
        }
        
        public override void Death()
        {
            base.Death();
            StopAllCoroutines();
            _currentAction = _deathAction;
            _agent.enabled = false;
            ChangeEnemyTag("Untagged");
        }
    }
}