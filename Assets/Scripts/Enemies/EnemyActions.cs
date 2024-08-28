using System.Collections;
using Enemies.EnemyUnitBase;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public interface IAction
    {
        public void PerformAction(Enemy enemy);
    }

    public class Idle : IAction
    {
        public void PerformAction(Enemy enemy) {}
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
            if (distanceToWalkPoint.magnitude < 1.5f)
            {
                _isWalkPointSet = false;
                Debug.Log("disabled");
            }
        }
        
        private void FindNewWalkPoint(Enemy enemy)
        {
            float randomZ = Random.Range(-_walkPointRange, _walkPointRange);
            float randomX = Random.Range(-_walkPointRange, _walkPointRange);

            _walkPoint = new Vector3(enemy.Transform.position.x + randomX, enemy.Transform.position.y, enemy.Transform.position.z + randomZ);
            
            if (Physics.Raycast(_walkPoint, -enemy.Transform.up, 2f, _ground) && IsPointOnNavMesh(_walkPoint)) _isWalkPointSet = true;
        }
        
        private bool IsPointOnNavMesh(Vector3 position) => NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas);
    }

    public class WalkInRadius : IAction
    {
        private readonly Transform _centerPoint;
        private readonly float _radius;

        public WalkInRadius(float radius, Transform centerPoint)
        {
            _radius = radius;
            _centerPoint = centerPoint;
        }
        
        public void PerformAction(Enemy enemy)
        {
            float angle = Random.Range(0, 360);

            Vector3 targetPosition = new Vector3
            {
                x = _centerPoint.position.x + Mathf.Cos(angle) * _radius,
                z = _centerPoint.position.z + Mathf.Sin(angle) * _radius
            };
            
            enemy.Agent.SetDestination(targetPosition);
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

        private IEnumerator Attacking()
        {
            yield return new WaitForSeconds(Enemy.Animator.GetCurrentAnimatorClipInfo(0).Length + 0.1f);
            _spawnProjectTile();
        }
    }
}