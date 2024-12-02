using System.Collections;
using System.Collections.Generic;
using Enemies.EnemyActions;
using UnityEngine;

namespace Enemies.EnemyUnits
{
    public class GolemEnemyUnit : EnemyUnitBase.EnemyUnitBase
    {
        [Header("Golem parameters")]
        [SerializeField] private GameObject centerPointPrefab;
        [SerializeField] private BoxCollider[] attackAreas;
        
        [SerializeField] private GameObject explosion;
        [SerializeField] private GameObject explosionEffect;
        
        private List<IAction> _attackActions = new();
        
        private IAction _deathAction;
        
        private Transform _centerPoint;
        
        protected override void InitEnemy()
        {
            _enemy = new Enemy(_agent, transform, animator, this);
            _walkAction = new Idle();
            _chaseAction = new ChaseWithTrigger(_player, ActivateGolem);
            
            _attackAction = new CallAttack(_player, timeBetweenAttacks, "isAttacking2", DeactivateAttackArea, ActivateAttackArea);

            _deathAction = new Death("isDead");

            _attackActions.Add(new CallAttack(_player, timeBetweenAttacks, "isAttacking2", DeactivateAttackArea, ActivateAttackArea));
            _attackActions.Add(new CallAttack(_player, timeBetweenAttacks, "isAttacking1", Explode));

            animator.enabled = false;

            StartCoroutine(UpdateAttack());
        }

        private IEnumerator UpdateAttack()
        {
            while (!_isDead)
            {
                _attackAction = _attackActions[Random.Range(0, _attackActions.Count)];
                yield return new WaitForSeconds(5);
            }
        }

        private void ActivateGolem()
        {
            _centerPoint = Instantiate(centerPointPrefab, transform.position, Quaternion.identity).transform;
            _walkAction = new WalkInRadius(walkPointRange, _centerPoint);
        }
        
        private void Explode()
        {
            Explosion explosionScr = Instantiate(explosion, transform.position, Quaternion.identity).GetComponent<Explosion>();
            explosionScr.explosionScale = 50;

            Instantiate(explosionEffect, transform.position, Quaternion.identity).transform.localScale = new Vector3(10, 10, 10);
        }

        private void ActivateAttackArea()
        {
            for (int i = 0; i < attackAreas.Length; i++)
            {
                attackAreas[i].enabled = true;
            }
        }

        private void DeactivateAttackArea()
        {
            for (int i = 0; i < attackAreas.Length; i++)
            {
                attackAreas[i].enabled = false;
            }
        }

        public override void Death()
        {
            base.Death();
            StopAllCoroutines();
            _currentAction = _deathAction;
            print(_currentAction);
        }
    }
}