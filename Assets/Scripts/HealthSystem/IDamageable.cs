namespace HealthSystem
{
    public interface IDamageable
    {
        public void TakeDamage(int damage, DamageType damageType, bool isElementStrengthened);
    }
}