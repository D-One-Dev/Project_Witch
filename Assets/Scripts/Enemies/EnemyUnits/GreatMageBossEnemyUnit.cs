using System.Collections;
using System.Collections.Generic;
using Enemies.EnemyActions;
using Enemies.EnemyUnitBase;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies.EnemyUnits
{
    [RequireComponent(typeof(EnemyBossHealth))]
    public class GreatMageBossEnemyUnit : ShootingEnemyUnitBase
    {
        [Header("Variables")]
        [SerializeField] private float[] timeBetweenBossActions;
        [SerializeField] private float currentTimeBetweenBossActions;
        
        [Header("Additional references")]
        [SerializeField] private GameObject centerPointPrefab;
        [SerializeField] private GameObject teleportEffect;
        [SerializeField] private GameObject lightEffect;
        [SerializeField] private GameObject shieldSphere;
        [SerializeField] private GameObject shieldSphereEffect;
        [SerializeField] private GameObject pillarWaveSpawner;
        private GameObject _pillarWaveSpawnerCache;

        [SerializeField] private AudioClip bossTheme;
        
        [Header("Projectiles")]
        [SerializeField] private GameObject rockObj;
        [SerializeField] private GameObject iceObj;
        [SerializeField] private GameObject icyRockObj;
        [SerializeField] private GameObject pillarObj;

        private int _previousState = 1;
        private int _currentState;
        
        private Transform _centerPoint;

        private List<IAction> _actions = new();
        private IAction _deathAction;

        private void OnEnable() => GetComponent<EnemyBossHealth>().OnHealthChanged += HealthUpdate;

        private void OnDisable() => GetComponent<EnemyBossHealth>().OnHealthChanged -= HealthUpdate;

        protected override void InitEnemy()
        {
            _centerPoint = Instantiate(centerPointPrefab, transform.position, Quaternion.identity).transform;
            
            _enemy = new Enemy(_agent, transform, animator, this);

            currentTimeBetweenBossActions = timeBetweenBossActions[0];
            
            _actions.Add(new WalkInRadius(walkPointRange, _centerPoint));
            _actions.Add(new TeleportationInRadius(walkPointRange, _centerPoint, SpawnTeleportEffect));
            _actions.Add(new ShootingAttack(_player, timeBetweenAttacks, "isAttacking2", SpawnIceProjectTile));
            _actions.Add(new ShootingAttack(_player, timeBetweenAttacks, "isAttacking2", SpawnIcyRockProjectTile));
            _actions.Add(new ShootingAttack(_player, timeBetweenAttacks, "isAttacking1", SpawnMultipleProjectTiles));
            _actions.Add(new ShootingAttack(_player, timeBetweenAttacks, "isAttacking2", SpawnPillarObj));
            _actions.Add(new ShootingAttack(_player, timeBetweenAttacks, "isAttacking2", SpawnPillarWave));

            _deathAction = new Death("isDead");

            _currentAction = new Idle();

            StartCoroutine(StartingBoss());
        }

        private IEnumerator StartingBoss()
        {
            yield return new WaitForSeconds(7);
            StartCoroutine(BossActionUpdate());
            MusicController.Instance.ChangeMusic(bossTheme);
            shieldSphere.SetActive(true);
            shieldSphereEffect.SetActive(false);
        }

        private void SpawnTeleportEffect() => Instantiate(teleportEffect, transform.position, Quaternion.identity);

        protected void SpawnIceProjectTile()
        {
            currentProjectTile = iceObj;
            SpawnNewProjectTile();
        }
        
        protected void SpawnIcyRockProjectTile()
        {
            currentProjectTile = icyRockObj;
            SpawnNewProjectTile();
        }

        protected void SpawnMultipleProjectTiles() => StartCoroutine(SpawningMultipleProjectTiles());

        private IEnumerator SpawningMultipleProjectTiles()
        {
            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(0.1f);
                Vector3 randomPosition = new Vector3(shootingPoint.position.x + Random.Range(-1f, 1f), shootingPoint.position.y + Random.Range(-1f, 1f), shootingPoint.position.z);
                _container.InstantiatePrefab(rockObj, randomPosition, shootingPoint.rotation, null);
            }
        }
        
        protected void SpawnPillarObj()
        {
            Vector3 position = new Vector3(_player.position.x, 0.02f, _player.position.z);
            _container.InstantiatePrefab(pillarObj, position, pillarObj.transform.rotation, null);
        }
        
        protected void SpawnPillarWave()
        {
            if (_pillarWaveSpawnerCache != null) return;
            
            StopCoroutine(Levitation());
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

            if (_previousState != _currentState)
            {
                BossStateUpdate();
                _previousState = _currentState;
            }
        }

        public override void Death()
        {
            base.Death();
            StopAllCoroutines();
            _agent.baseOffset = 0.73f;
            _currentAction = _deathAction;
            lightEffect.SetActive(false);
            shieldSphere.SetActive(false);
            MusicController.Instance.DisableMusic();
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
                    lightEffect.SetActive(true);
                    _currentAction = _actions[1];
                    print("state 2");
                    break;
                case 3:
                    currentTimeBetweenBossActions = timeBetweenBossActions[3];
                    print("state 3");
                    break;
            }
        }
    }
}