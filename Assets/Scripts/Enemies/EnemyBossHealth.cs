using UnityEngine;
using UnityEngine.UI;

namespace Enemies
{
    public class EnemyBossHealth : EnemyHealth
    {
        [SerializeField] private Image hpBar;
        protected int OriginHealth;
        
        private void Awake() => OriginHealth = health;

        public override void UpdateUI() => hpBar.fillAmount = (health * 100) / OriginHealth;
    }
}