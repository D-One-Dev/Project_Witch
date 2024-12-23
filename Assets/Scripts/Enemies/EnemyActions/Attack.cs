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

    public class AttackWithCallback : Attack
    {
        public delegate void AttackCall();

        private readonly AttackCall _beforeAttackCall;
        private readonly AttackCall _attackCall;

        private readonly float _timeForAttack;
        
        public AttackWithCallback(Transform player, float timeBetweenAttacks, string isAttackingTriggerKey, AttackCall attackCall) : base(player, timeBetweenAttacks, isAttackingTriggerKey)
        {
            _attackCall = attackCall;
        }
        
        public AttackWithCallback(Transform player, float timeBetweenAttacks, string isAttackingTriggerKey, AttackCall attackCall, AttackCall beforeAttackCall, string animationName = "") : base(player, timeBetweenAttacks, isAttackingTriggerKey)
        {
            _attackCall = attackCall;
            _beforeAttackCall = beforeAttackCall;
            
            if (animationName != "")
            {
                /*RuntimeAnimatorController ac = Enemy.Animator.runtimeAnimatorController;
                
                for (int i = 0; i < ac.animationClips.Length; i++)
                {
                    if (ac.animationClips[i].name == animationName)
                    {
                        _timeForAttack = ac.animationClips[i].length;
                    }
                    else
                    {
                        _timeForAttack = Enemy.Animator.GetNextAnimatorClipInfo(0).Length;
                    }
                }*/
                
                _timeForAttack = Enemy.Animator.GetNextAnimatorClipInfo(0).Length;
            }
        }
        
        public AttackWithCallback(Transform player, float timeBetweenAttacks, string isAttackingTriggerKey, AttackCall attackCall, float timeForAttack) : base(player, timeBetweenAttacks, isAttackingTriggerKey)
        {
            _attackCall = attackCall;
            _timeForAttack = timeForAttack;
        }

        protected override void DoAttack()
        {
            _beforeAttackCall?.Invoke();
            Enemy.EnemyUnit.StartCoroutine(Attacking());
        }
        
        protected IEnumerator Attacking()
        {
            if (_timeForAttack == 0)
                yield return new WaitForSeconds(Enemy.Animator.GetCurrentAnimatorClipInfo(0).Length + 0.1f);
            else yield return new WaitForSeconds(_timeForAttack);
            
            _attackCall();
        }
    }
    
    public class ShootingAttackWithCallback : AttackWithCallback
    {
        public ShootingAttackWithCallback(Transform player, float timeBetweenAttacks, string isAttackingTriggerKey, AttackCall attackCall) : base(player, timeBetweenAttacks, isAttackingTriggerKey, attackCall)
        {
        }
        
        protected override void DoAttack()
        {
            base.DoAttack();
            var shootingEnemyUnit = (ShootingEnemyUnitBase) Enemy.EnemyUnit;
            shootingEnemyUnit.shootingPoint.LookAt(Player);
        }
    }
}