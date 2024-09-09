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
        [SerializeField] private GameObject rockObj;
        [SerializeField] private GameObject pillarObj;
        [SerializeField] private GameObject teleportEffect;
        
        [SerializeField] private GameObject pillarWaveSpawner;
        private GameObject _pillarWaveSpawnerCache;

        private int _currentState;
        
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
            _actions.Add(new TeleportationInRadius(walkPointRange, _centerPoint, SpawnTeleportEffect));
            _actions.Add(new ShootingAttack(_player, timeBetweenAttacks, "isAttacking1", SpawnMultipleProjectTiles));
            _actions.Add(new ShootingAttack(_player, timeBetweenAttacks, "isAttacking2", SpawnNewProjectTile));
            _actions.Add(new ShootingAttack(_player, timeBetweenAttacks, "isAttacking2", SpawnPillarObj));
            _actions.Add(new ShootingAttack(_player, timeBetweenAttacks, "isAttacking2", SpawnPillarWave));

            StartCoroutine(BossActionUpdate());
        }

        private void SpawnTeleportEffect() => Instantiate(teleportEffect, transform.position, Quaternion.identity);

        protected void SpawnPillarWave()
        {
            if (_pillarWaveSpawnerCache != null) return;
            
            StartCoroutine(Levitation());
            float randomAngleDegrees = 90;
            float randomAngleRadians = Mathf.Deg2Rad * randomAngleDegrees;

            float x = _centerPoint.position.x + Mathf.Cos(randomAngleRadians) * walkPointRange;
            float z = _centerPoint.position.z + Mathf.Sin(randomAngleRadians) * walkPointRange;

            _pillarWaveSpawnerCache = Instantiate(pillarWaveSpawner, new Vector3(x, transform.position.y, z), Quaternion.identity);
            _pillarWaveSpawnerCache.transform.LookAt(_player);
        }

        private IEnumerator Levitation()
        {
            while (_agent.baseOffset < 1.8f)
            {
                _agent.baseOffset += 0.01f;
                yield return null;
            }
            
            yield return new WaitForSeconds(5);
            
            while (_agent.baseOffset > 0.73f)
            {
                _agent.baseOffset -= 0.01f;
                yield return null;
            }
        }

        protected void SpawnPillarObj()
        {
            Vector3 position = new Vector3(Player.Player.Instance.transform.position.x, 0.02f, Player.Player.Instance.transform.position.z);
            Instantiate(pillarObj, position, pillarObj.transform.rotation);
        }

        /*private IEnumerator SpawningPillarObj()
        {
            Vector3 position = new Vector3(Player.Player.Instance.transform.position.x, -10f, Player.Player.Instance.transform.position.z);
            yield return new WaitForSeconds(0.4f);
            Instantiate(pillarObj, position, pillarObj.transform.rotation);
        }*/

        protected void SpawnMultipleProjectTiles() => StartCoroutine(SpawningMultipleProjectTiles());

        private IEnumerator SpawningMultipleProjectTiles()
        {
            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(0.1f);
                Vector3 randomPosition = new Vector3(shootingPoint.position.x + Random.Range(-1f, 1f), shootingPoint.position.y + Random.Range(-1f, 1f), shootingPoint.position.z);
                Instantiate(rockObj, randomPosition, shootingPoint.rotation);
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
                    _currentState = 1;
                    break;
                case <= 0.4f and > 0.2f:
                    _currentState = 2;
                    break;
                case <= 0.2f:
                    _currentState = 3;
                    break;
            }
            
            BossStateUpdate();
        }

        private void BossStateUpdate()
        {
            switch (_currentState)
            {
                case 1:
                    currentTimeBetweenBossActions = timeBetweenBossActions[1];
                    break;
                case 2:
                    currentTimeBetweenBossActions = timeBetweenBossActions[2];
                    _actions[1] = new TeleportationInRadius(walkPointRange, 1, _centerPoint, SpawnTeleportEffect);
                    break;
                case 3:
                    currentTimeBetweenBossActions = timeBetweenBossActions[3];
                    break;
            }
        }
    }
}