namespace Enemies.EnemyActions
{
    public class Idle : IAction
    {
        public void PerformAction(Enemy enemy) {}
    }

    public class Death : IAction
    {
        private readonly string _isDeadTriggerKey;
        
        public Death(string isDeadTriggerKey)
        {
            _isDeadTriggerKey = isDeadTriggerKey;
        }
        
        public void PerformAction(Enemy enemy)
        {
            enemy.Animator.SetTrigger(_isDeadTriggerKey);
        }
    }
}