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

        [SerializeField] private GameObject laserBeam;

        private GameObject _currentBeam;

        private List<IAction> _attackActions = new();
        
        private IAction _deathAction;
        
        private Transform _centerPoint;
        
        protected override void InitEnemy()
        {
            _enemy = new Enemy(_agent, transform, animator, this);
            _walkAction = new Idle();
            _chaseAction = new ChaseWithTrigger(_player, ActivateGolem);
            
            _attackAction = new AttackWithCallback(_player, timeBetweenAttacks, "isAttacking3", DespawnLaser, SpawnLaser, "ATTACK3");

            _deathAction = new Death("isDead");

            //_attackActions.Add(new AttackWithCallback(_player, timeBetweenAttacks, "isAttacking1", Explode));
            //_attackActions.Add(new AttackWithCallback(_player, timeBetweenAttacks, "isAttacking2", DeactivateAttackArea, ActivateAttackArea));
            _attackActions.Add(new AttackWithCallback(_player, timeBetweenAttacks, "isAttacking3", DespawnLaser, SpawnLaser, "ATTACK3"));

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
            animator.SetTrigger("isAwake");
            _centerPoint = Instantiate(centerPointPrefab, transform.position, Quaternion.identity).transform;
            _walkAction = new WalkInRadius(walkPointRange, _centerPoint);
        }

        private void SpawnLaser() => StartCoroutine(SpawningLaser());

        private IEnumerator SpawningLaser()
        {
            yield return new WaitForSeconds(2);
            _currentBeam = Instantiate(laserBeam, explosionPoint.position, Quaternion.identity);
            _currentBeam.transform.parent = explosionPoint.transform; 
        }

        private void DespawnLaser()
        {
            Destroy(_currentBeam);
            print("despawn");
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