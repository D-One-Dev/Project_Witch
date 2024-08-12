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
                Debug.Log("disabled");
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

        public Chase(Transform player) => _player = player;
        
        public void PerformAction(Enemy enemy)
        {
            enemy.Agent.SetDestination(_player.position);
        }
    }
    
    public abstract class Attack : IAction
    {
        private readonly float _timeBetweenAttacks;
        private readonly Transform _player;

        protected Enemy EnemyCache;
        
        private bool _isAbleToAttack = true;

        protected Attack(Transform player, float timeBetweenAttacks)
        {
            _player = player;
            _timeBetweenAttacks = timeBetweenAttacks;
        }

        public void PerformAction(Enemy enemy)
        {
            enemy.Agent.SetDestination(enemy.Transform.position);
            
            enemy.Transform.LookAt(_player);

            EnemyCache = enemy;

            if (_isAbleToAttack)
            {
                _isAbleToAttack = false;
                DoAttack();
                enemy.EnemyUnit.StartCoroutine(Cooldown());
            }
        }

        protected abstract void DoAttack();

        private IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(_timeBetweenAttacks);
            _isAbleToAttack = true;
        }
    }

    public class LongDistanceAttack : Attack
    {
        private readonly LongDistanceEnemyUnit.SpawnProjectTile _spawnProjectTile;
        
        public LongDistanceAttack(Transform player, float timeBetweenAttacks, LongDistanceEnemyUnit.SpawnProjectTile spawnProjectTile) : base(player, timeBetweenAttacks)
        {
            _spawnProjectTile = spawnProjectTile;
        }

        protected override void DoAttack()
        {
            _spawnProjectTile();
        }
    }
}