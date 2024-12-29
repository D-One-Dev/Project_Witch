using UnityEngine;
using UnityEngine.Events;

namespace Enemies
{
    public class GolemWaveBossfight : MonoBehaviour
    {
        [SerializeField] private GolemWave[] waves;

        [SerializeField] private UnityEvent onStartBossFight;
        
        private int _currentWave = -1;
        private int _currentWaveEnemiesCount;

        private void Start()
        {
            for (int i = 0; i < waves.Length; i++)
            {
                for (int j = 0; j < waves[i].golems.Length; j++)
                {
                    GolemInWave golem = waves[i].golems[j].gameObject.AddComponent<GolemInWave>();
                    golem.bossFightSystem = this;
                    
                    waves[i].golems[j].DeactivateEnemyUnit();
                }
            }
        }

        private void StartNextWave()
        {
            _currentWave++;

            if (_currentWave >= waves.Length)
            {
                //Bossfight complete
                print("HELL YEAH");
                
                return;
            }

            _currentWaveEnemiesCount = waves[_currentWave].golems.Length;
                
            for (int i = 0; i < waves[_currentWave].golems.Length; i++)
            {
                waves[_currentWave].golems[i].ActivateEnemyUnit();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                onStartBossFight?.Invoke();
                StartNextWave();
            }
        }

        public void OnDestroyGolemInWave()
        {
            _currentWaveEnemiesCount--;

            if (_currentWaveEnemiesCount <= 0)
            {
                StartNextWave();
            }
        }
    }

    [System.Serializable]
    public class GolemWave
    {
        public EnemyUnitBase.EnemyUnitBase[] golems;
    }
}