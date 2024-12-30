using UnityEngine;

namespace Enemies
{
    public class GolemInWave : MonoBehaviour
    {
        public GolemWaveBossfight bossFightSystem;

        public int waveID;
        public int golemID;
        
        public void OnDeath()
        {
            bossFightSystem.OnDestroyGolemInWave(waveID, golemID);
        }
    }
}