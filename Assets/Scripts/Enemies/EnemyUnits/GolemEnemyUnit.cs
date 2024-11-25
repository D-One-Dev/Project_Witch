using System.Collections;
using System.Collections.Generic;
using Enemies.EnemyActions;
using Enemies.EnemyUnitBase;
using UnityEngine;

namespace Enemies.EnemyUnits
{
    public class GolemEnemyUnit : ShootingEnemyUnitBase
    {
        [Header("Golem parameters")]
        private List<IAction> _attackActions = new();

        private IAction _deathAction;
        
        protected override void InitEnemy()
        {
            _enemy = new Enemy(_agent, transform, animator, this);
            _walkAction = new Walk(walkPointRange, groundLayer);
            _chaseAction = new Chase(_player);
            
            _attackAction = new ShootingAttack(_player, timeBetweenAttacks, "isAttacking1", SpawnNewProjectTile);

            _deathAction = new Death("isDead");

            _attackActions.Add(new ShootingAttack(_player, timeBetweenAttacks, "isAttacking1", SpawnNewProjectTile));
            _attackActions.Add(new ShootingAttack(_player, timeBetweenAttacks, "isAttacking2", SpawnNewProjectTile));
            StartCoroutine(UpdateAttack());
            
        }

        private IEnumerator UpdateAttack()
        {
            _attackAction = _attackActions[Random.Range(0, _attackActions.Count)];
            yield return new WaitForSeconds(2);
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