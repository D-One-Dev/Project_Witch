using System.Collections;
using System.Collections.Generic;
using Enemies.EnemyUnitBase;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies.EnemyUnits
{
    [RequireComponent(typeof(EnemyBossHealth))]
    public class GreatMageBossEnemyUnit : ShootingEnemyUnitBase
    {
        [SerializeField] private float[] timeBetweenBossActions;
        [SerializeField] private float currentTimeBetweenBossActions;
        [SerializeField] private GameObject centerPointPrefab;
        [SerializeField] private GameObject pillarObj;
        private Transform _centerPoint;

        private List<IAction> _actions = new();

        private void OnEnable() => GetComponent<EnemyBossHealth>().onHealthChanged += HealthUpdate;

        private void OnDisable() => GetComponent<EnemyBossHealth>().onHealthChanged -= HealthUpdate;

        protected override void InitEnemy()
        {
            _centerPoint = Instantiate(centerPointPrefab, transform.position, Quaternion.identity).transform;
            
            _enemy = new Enemy(_agent, transform, animator, this);

            currentTimeBetweenBossActions = timeBetweenBossActions[0];
            
            _actions.Add(new WalkInRadius(walkPointRange, _centerPoint));
            _actions.Add(new TeleportationInRadius(walkPointRange, _centerPoint));
            _actions.Add(new ShootingAttack(_player, timeBetweenAttacks, "isAttacking1", SpawnMultipleProjectTiles));
            _actions.Add(new ShootingAttack(_player, timeBetweenAttacks, "isAttacking2", SpawnPillarObj));

            StartCoroutine(BossActionUpdate());
        }

        protected void SpawnPillarObj() => StartCoroutine(SpawningPillarObj());

        private IEnumerator SpawningPillarObj()
        {
            Vector3 playerPosition = Player.Player.Instance.transform.position;
            yield return new WaitForSeconds(0.15f);
            Instantiate(pillarObj, playerPosition, pillarObj.transform.rotation);
        }

        protected void SpawnMultipleProjectTiles() => StartCoroutine(SpawningMultipleProjectTiles());

        private IEnumerator SpawningMultipleProjectTiles()
        {
            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(0.2f);
                Vector3 randomPosition = new Vector3(shootingPoint.position.x + Random.Range(-1f, 1f), shootingPoint.position.y + Random.Range(-1f, 1f), shootingPoint.position.z);
                Instantiate(projectTile, randomPosition, shootingPoint.rotation);
            }
        }

        protected override void CheckState() {}

        private IEnumerator BossActionUpdate()
        {
            _currentAction = _actions[Random.Range(0, _actions.Count)];
            yield return new WaitForSeconds(currentTimeBetweenBossActions);
            _currentAction = new Idle();
            yield return new WaitForSeconds(currentTimeBetweenBossActions / 40);
            StartCoroutine(BossActionUpdate());
        }

        private void HealthUpdate(int health, int originHealth)
        {
            float healthPercentage = (float)health / originHealth;

            switch (healthPercentage)
            {
                case <= 0.8f and > 0.4f:
                    currentTimeBetweenBossActions = timeBetweenBossActions[1];
                    break;
                case <= 0.4f and > 0.2f:
                    currentTimeBetweenBossActions = timeBetweenBossActions[2];
                    _actions[1] = new TeleportationInRadius(walkPointRange, 1, _centerPoint);
                    break;
                case <= 0.2f:
                    currentTimeBetweenBossActions = timeBetweenBossActions[3];
                    break;
            }
        }
    }
}