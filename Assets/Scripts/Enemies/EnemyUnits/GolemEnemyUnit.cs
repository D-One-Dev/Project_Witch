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

        [SerializeField] private BoxCollider explosionAttackArea;
        
        [SerializeField] private GameObject explosion;
        [SerializeField] private GameObject[] explosionEffects;
        [SerializeField] private GameObject explosionSphereEffect;
        [SerializeField] private Transform explosionPoint;

        private List<IAction> _attackActions = new();
        
        private IAction _deathAction;
        
        private Transform _centerPoint;
        
        protected override void InitEnemy()
        {
            _enemy = new Enemy(_agent, transform, animator, this);
            _walkAction = new Idle();
            _chaseAction = new ChaseWithTrigger(_player, ActivateGolem);

            _attackAction = new AttackWithCallback(_player, timeBetweenAttacks, "isAttacking1", Explode);

            _deathAction = new Death("isDead");

            _attackActions.Add(new AttackWithCallback(_player, timeBetweenAttacks, "isAttacking1", Explode));
            _attackActions.Add(new AttackWithCallback(_player, timeBetweenAttacks, "isAttacking2", ActivateAttackArea, DeactivateAttackArea));
            _attackActions.Add(new Attack(_player, timeBetweenAttacks, "isAttacking3"));
        }

        private IEnumerator UpdateAttack()
        {
            while (!_isDead)
            {
                _attackAction = _attackActions[Random.Range(0, _attackActions.Count)];
                
                yield return new WaitForSeconds(5);
                
                if (Random.Range(0, 3) == 0) _chaseAction = new Attack(_player, timeBetweenAttacks, "isAttacking3");
                else _chaseAction = new Chase(_player);
            }
        }

        private void ActivateGolem()
        {
            animator.SetTrigger("isAwake");
            _centerPoint = Instantiate(centerPointPrefab, transform.position, Quaternion.identity).transform;
            _walkAction = new WalkInRadius(walkPointRange, _centerPoint);
            
            StartCoroutine(UpdateAttack());
        }
        
        private void Explode()
        {
            Explosion explosionScr = Instantiate(explosion, transform.position, Quaternion.identity).GetComponent<Explosion>();
            explosionScr.explosionScale = 70;

            for (int i = 0; i < explosionEffects.Length; i++)
            {
                Instantiate(explosionEffects[i], transform.position, Quaternion.identity).transform.localScale = new Vector3(10, 10, 10);
            }
            
            Instantiate(explosionSphereEffect, explosionPoint.position, Quaternion.identity);

            StopCoroutine(ActivateExplosionAttackArea());
            StartCoroutine(ActivateExplosionAttackArea());
        }

        private IEnumerator ActivateExplosionAttackArea()
        {
            explosionAttackArea.enabled = true;
            yield return new WaitForSeconds(0.1f);
            explosionAttackArea.enabled = false;
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