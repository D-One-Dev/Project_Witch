using UI;

namespace Enemies
{
    public class EnemyBossHealth : EntityHealth
    {
        public override void UpdateUI() => BossHealthUI.Instance.ChangeHpBarValue((float) health / OriginHealth);
    }
}