using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies.EnemyActions
{
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
                //Debug.Log("disabled");
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
        private bool _isWalkPointSet;
        private Vector3 targetPosition;

        public WalkInRadius(float radius, Transform centerPoint)
        {
            _radius = radius;
            _centerPoint = centerPoint;
        }
        
        public void PerformAction(Enemy enemy)
        {
            if (!_isWalkPointSet)
            {
                float angle = Random.Range(0, 360);

                targetPosition = new Vector3
                {
                    x = _centerPoint.position.x + Mathf.Cos(angle) * _radius,
                    z = _centerPoint.position.z + Mathf.Sin(angle) * _radius
                };
                
                enemy.Agent.SetDestination(targetPosition);
                
                enemy.Transform.LookAt(targetPosition);
                enemy.Transform.localEulerAngles = new Vector3(0f, enemy.Transform.localEulerAngles.y, 0f);

                _isWalkPointSet = true;
            }
            
            if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
            {
                _isWalkPointSet = false;
            }
        }
    }
    
    public class TeleportationInRadius : IAction
    {
        private readonly Transform _centerPoint;
        private readonly float _radius;
        private bool _isAbleToTeleport = true;

        private readonly float _teleportCooldown;
        public delegate void TeleportEffect();

        private readonly TeleportEffect _teleportEffect;

        public TeleportationInRadius(float radius, Transform centerPoint, TeleportEffect teleportEffect)
        {
            _radius = radius;
            _centerPoint = centerPoint;

            _teleportCooldown = 2;

            _teleportEffect = teleportEffect;
        }
        
        public TeleportationInRadius(float radius, float teleportCooldown, Transform centerPoint, TeleportEffect teleportEffect)
        {
            _radius = radius;
            _centerPoint = centerPoint;

            _teleportCooldown = teleportCooldown;
            
            _teleportEffect = teleportEffect;
        }
        
        public void PerformAction(Enemy enemy)
        {
            if (_isAbleToTeleport)
            {
                _isAbleToTeleport = false;
                
                _teleportEffect();
                
                float angle = Random.Range(0, 360);

                Vector3 targetPosition = new Vector3
                {
                    x = _centerPoint.position.x + Mathf.Cos(angle) * _radius,
                    z = _centerPoint.position.z + Mathf.Sin(angle) * _radius
                };

                enemy.Transform.position = targetPosition;

                enemy.EnemyUnit.StartCoroutine(TeleportCooldown());
            }
        }

        private IEnumerator TeleportCooldown()
        {
            yield return new WaitForSeconds(_teleportCooldown);
            _isAbleToTeleport = true;
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
            
            if (enemy.Animator.enabled == false) enemy.Animator.enabled = true;
            enemy.Animator.SetTrigger(IsWalking);
        }
    }
    
    public class ChaseWithTrigger : IAction
    {
        private readonly Transform _player;
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        public delegate void ChaseTrigger();

        private readonly ChaseTrigger _chaseTriggerFunc;

        private bool _isFirstTrigger = true;

        public ChaseWithTrigger(Transform player, ChaseTrigger chaseTriggerFunc)
        {
            _player = player;
            _chaseTriggerFunc = chaseTriggerFunc;
        }

        public void PerformAction(Enemy enemy)
        {
            enemy.Agent.SetDestination(_player.position);

            if (enemy.Animator.enabled == false)
            {
                enemy.Animator.enabled = true;
            }
            
            enemy.Animator.SetTrigger(IsWalking);

            if (_isFirstTrigger)
            {
                _chaseTriggerFunc();
                _isFirstTrigger = false;
            }
        }
    }
}