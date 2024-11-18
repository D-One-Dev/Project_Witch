using System.Collections;
using Enemies.EnemyUnitBase;
using UnityEngine;

namespace Enemies.EnemyActions
{
    public abstract class Attack : IAction
    {
        protected readonly float TimeBetweenAttacks;
        protected readonly Transform Player;

        protected Enemy Enemy;
        
        private bool _isAbleToAttack = true;
        private readonly string _isAttacking;

        protected Attack(Transform player, float timeBetweenAttacks, string isAttackingTriggerKey)
        {
            Player = player;
            TimeBetweenAttacks = timeBetweenAttacks;
            _isAttacking = isAttackingTriggerKey;
        }

        public void PerformAction(Enemy enemy)
        {
            enemy.Agent.SetDestination(enemy.Transform.position);
            
            enemy.Transform.LookAt(Player);
            enemy.Transform.localEulerAngles = new Vector3(0f, enemy.Transform.localEulerAngles.y, 0f);

            Enemy = enemy;

            if (_isAbleToAttack)
            {
                _isAbleToAttack = false;
                enemy.Animator.SetTrigger(_isAttacking);
                enemy.EnemyUnit.StartCoroutine(Cooldown());
                DoAttack();
            }
        }

        protected abstract void DoAttack();

        private IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(TimeBetweenAttacks);
            _isAbleToAttack = true;
        }
    }

    public class ShootingAttack : Attack
    {
        private readonly ShootingEnemyUnitBase.SpawnProjectTile _spawnProjectTile;
        
        public ShootingAttack(Transform player, float timeBetweenAttacks, string isAttackingTriggerKey,
            ShootingEnemyUnitBase.SpawnProjectTile spawnProjectTile) : base(player, timeBetweenAttacks, isAttackingTriggerKey)
        {
            _spawnProjectTile = spawnProjectTile;
        }

        protected override void DoAttack()
        {
            var shootingEnemyUnit = (ShootingEnemyUnitBase) Enemy.EnemyUnit;
            shootingEnemyUnit.shootingPoint.LookAt(Player);
            
            Enemy.EnemyUnit.StartCoroutine(Attacking());
        }

        protected IEnumerator Attacking()
        {
            yield return new WaitForSeconds(Enemy.Animator.GetCurrentAnimatorClipInfo(0).Length + 0.1f);
            _spawnProjectTile();
        }
    }
}