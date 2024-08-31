using UI;

namespace Enemies
{
    public class EnemyBossHealth : EnemyHealth
    {
        public override void UpdateUI() => BossHealthUI.Instance.ChangeHpBarValue((float) health / OriginHealth);
    }
}