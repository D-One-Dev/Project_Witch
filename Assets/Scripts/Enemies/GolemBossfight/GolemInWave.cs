using UnityEngine;

namespace Enemies
{
    public class GolemInWave : MonoBehaviour
    {
        public GolemWaveBossfight bossFightSystem;
        
        private void OnDestroy()
        {
            bossFightSystem.OnDestroyGolemInWave();
        }
    }
}