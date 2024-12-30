using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Enemies
{
    public class GolemWaveBossfight : MonoBehaviour
    {
        [SerializeField] private GolemWave[] waves;

        [SerializeField] private UnityEvent onStartBossFight;
        [SerializeField] private UnityEvent onEndBossFight;

        [SerializeField] private GameObject explosionEffect;
        
        private int _currentWave = -1;

        private int _currentEnemiesWaveCount;
        private List<int> _currentDefeatedEnemiesID = new();

        private bool isBossfightActivated;

        private void Start()
        {
            for (int i = 0; i < waves.Length; i++)
            {
                for (int j = 0; j < waves[i].golems.Count; j++)
                {
                    GolemInWave golem = waves[i].golems[j].gameObject.AddComponent<GolemInWave>();
                    golem.bossFightSystem = this;
                    
                    golem.waveID = i;
                    golem.golemID = j;
                    
                    golem.gameObject.GetComponent<EntityHealth>().OnDeath.AddListener(golem.OnDeath);
                    
                    waves[i].golems[j].DeactivateEnemyUnit();
                }
            }
        }

        private void StartNextWave()
        {
            _currentWave++;

            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            
            Debug.Log("current wave " + _currentWave);

            if (_currentWave >= waves.Length)
            {
                //Bossfight complete
                Debug.Log("HELL YEAH");
                
                onEndBossFight?.Invoke();
                
                return;
            }

            _currentEnemiesWaveCount = waves[_currentWave].golems.Count;
                
            for (int i = 0; i < waves[_currentWave].golems.Count; i++)
            {
                waves[_currentWave].golems[i].ActivateEnemyUnit();
                print(waves[_currentWave].golems[i]);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isBossfightActivated && other.CompareTag("Player"))
            {
                print("entered");
                isBossfightActivated = true;
                onStartBossFight?.Invoke();
                StartNextWave();
            }
        }

        public void OnDestroyGolemInWave(int waveID, int golemID)
        {
            if (_currentEnemiesWaveCount == -1 || waveID != _currentWave) return;

            if (!_currentDefeatedEnemiesID.Contains(golemID))
            {
                _currentEnemiesWaveCount--;
                Debug.Log("current enemies wave count " + _currentEnemiesWaveCount);

                _currentDefeatedEnemiesID.Add(golemID);
            }

            if (_currentEnemiesWaveCount <= 0)
            {
                _currentEnemiesWaveCount = -1;
                _currentDefeatedEnemiesID = new List<int>();
                StartNextWave();
            }
        }
    }

    [System.Serializable]
    public class GolemWave
    {
        public List<EnemyUnitBase.EnemyUnitBase> golems;
    }
}