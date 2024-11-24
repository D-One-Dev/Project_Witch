using UI;
using Zenject;

namespace Enemies
{
    public class EnemyBossHealth : EntityHealth
    {
        private BossHealthUI _bossHealthUI;

        [Inject]
        public void Construct(BossHealthUI bossHealthUI)
        {
            _bossHealthUI = bossHealthUI;
        }

        public override void UpdateUI() => _bossHealthUI.ChangeHpBarValue((float) health / OriginHealth);

        protected override void Death()
        {
            isDead = true;
            OnDeath.Invoke();
        }
    }
}