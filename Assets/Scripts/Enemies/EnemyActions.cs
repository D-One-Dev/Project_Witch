using System.Collections;
using UnityEngine;

namespace Enemies
{
    public interface IAction
    {
        public void PerformAction(Enemy enemy);
    }

    public class Walk : IAction
    {
        private readonly float _walkPointRange;
        
        private bool _isWalkPointSet;
        private Vector3 _walkPoint;

        private readonly LayerMask _ground;

        public Walk(float walkPointRange, LayerMask ground)
        {
            _walkPointRange = walkPointRange;
            _ground = ground;
        }
        
        public void PerformAction(Enemy enemy)
        {
            if (!_isWalkPointSet) FindNewWalkPoint(enemy);

            if (_isWalkPointSet)
            {
                enemy.Agent.SetDestination(_walkPoint);
            }

            Vector3 distanceToWalkPoint = enemy.Transform.position - _walkPoint;

            if (distanceToWalkPoint.magnitude < 1f)
            {
                _isWalkPointSet = false;
                //Debug.Log("disabled");
            }
        }
        
        private void FindNewWalkPoint(Enemy enemy)
        {
            float randomZ = Random.Range(-_walkPointRange, _walkPointRange);
            float randomX = Random.Range(-_walkPointRange, _walkPointRange);

            _walkPoint = new Vector3(enemy.Transform.position.x + randomX, enemy.Transform.position.y, enemy.Transform.position.z + randomZ);
            
            if (Physics.Raycast(_walkPoint, -enemy.Transform.up, 2f, _ground)) _isWalkPointSet = true;
        }
    }

    public class Chase : IAction
    {
        private readonly Transform _player;
        private static readonly int IsWalking = Animator.StringToHash("isWalking");

        public Chase(Transform player) => _player = player;
        
        public void PerformAction(Enemy enemy)
        {
            enemy.Agent.SetDestination(_player.position);
            enemy.Animator.SetTrigger(IsWalking);
        }
    }
    
    public abstract class Attack : IAction
    {
        protected readonly float _timeBetweenAttacks;
        protected readonly Transform _player;

        protected Enemy _enemy;
        
        private bool _isAbleToAttack = true;
        private static readonly int IsAttacking = Animator.StringToHash("isAttacking");

        protected Attack(Transform player, float timeBetweenAttacks)
        {
            _player = player;
            _timeBetweenAttacks = timeBetweenAttacks;
        }

        public void PerformAction(Enemy enemy)
        {
            enemy.Agent.SetDestination(enemy.Transform.position);
            
            enemy.Transform.LookAt(_player);
            enemy.Transform.localEulerAngles = new Vector3(0f, enemy.Transform.localEulerAngles.y, 0f);

            _enemy = enemy;

            if (_isAbleToAttack)
            {
                _isAbleToAttack = false;
                enemy.Animator.SetTrigger(IsAttacking);
                enemy.EnemyUnit.StartCoroutine(Cooldown());
                DoAttack();
            }
        }

        protected abstract void DoAttack();

        private IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(_timeBetweenAttacks);
            _isAbleToAttack = true;
        }
    }

    public class ShootingAttack : Attack
    {
        private readonly ShootingEnemyUnit.SpawnProjectTile _spawnProjectTile;
        
        public ShootingAttack(Transform player, float timeBetweenAttacks, ShootingEnemyUnit.SpawnProjectTile spawnProjectTile) : base(player, timeBetweenAttacks)
        {
            _spawnProjectTile = spawnProjectTile;
        }

        protected override void DoAttack()
        {
            var shootingEnemyUnit = (ShootingEnemyUnit)_enemy.EnemyUnit;
            shootingEnemyUnit.shootingPoint.LookAt(_player);
            
            _enemy.EnemyUnit.StartCoroutine(Attacking());
        }

        private IEnumerator Attacking()
        {
            yield return new WaitForSeconds(_enemy.Animator.GetCurrentAnimatorClipInfo(0).Length + 0.1f);
            _spawnProjectTile();
        }
    }
}