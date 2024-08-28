using System.Collections;
using System.Collections.Generic;
using Enemies.EnemyUnitBase;
using UnityEngine;

namespace Enemies.EnemyUnits
{
    [RequireComponent(typeof(EnemyBossHealth))]
    public class GreatMageBossEnemyUnit : ShootingEnemyUnitBase
    {
        [SerializeField] private float timeBetweenBossActions;
        [SerializeField] private GameObject centerPointPrefab;
        private Transform _centerPoint;

        private List<IAction> _actions = new();

        protected override void InitEnemy()
        {
            _centerPoint = Instantiate(centerPointPrefab, transform.position, Quaternion.identity).transform;
            
            _enemy = new Enemy(_agent, transform, animator, this);
            
            _actions.Add(new WalkInRadius(walkPointRange, _centerPoint));
            _actions.Add(new ShootingAttack(_player, timeBetweenAttacks, "isAttacking1", SpawnMultiplePJ));
            _actions.Add(new ShootingAttack(_player, timeBetweenAttacks, "isAttacking2", SpawnNewProjectTile));

            StartCoroutine(BossActionUpdate());
        }

        protected void SpawnMultiplePJ()
        {
            StartCoroutine(SpawningMultiplePJ());
        }

        private IEnumerator SpawningMultiplePJ()
        {
            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(0.1f);
                SpawnNewProjectTile();
            }
        }

        protected override void CheckState() {}

        private IEnumerator BossActionUpdate()
        {
            _currentAction = _actions[Random.Range(0, _actions.Count)];
            yield return new WaitForSeconds(timeBetweenBossActions);
            _currentAction = new Idle();
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(BossActionUpdate());
        }
    }
}