using System;
using Enemies.EnemyActions;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Enemies.EnemyUnitBase
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class EnemyUnitBase : MonoBehaviour
    {
        [Header("Enemy Unit Base Parameters")]
        [SerializeField] protected Animator animator;
        [SerializeField] protected LayerMask playerLayer, groundLayer;
        
        [SerializeField] protected float walkPointRange;
        [SerializeField] protected float timeBetweenAttacks;
        [SerializeField] protected float sightRange, attackRange;
        
        protected bool _playerInSightRange, _playerInAttackRange;
        protected bool _isDead;

        [Inject(Id = "PlayerTransform")]
        protected readonly Transform _player;

        protected NavMeshAgent _agent;

        protected Enemy _enemy;
        
        protected IAction _currentAction;
        protected IAction _walkAction, _chaseAction, _attackAction;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();

            InitEnemy();
        }

        protected abstract void InitEnemy();

        private void Update()
        {
            _playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
            _playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
            
            CheckState();
            
            _enemy.DoAction(_currentAction);
        }

        protected virtual void CheckState()
        {
            if (_isDead) return;
            
            if (!_playerInSightRange && !_playerInAttackRange) SetAction(_walkAction);
            else if (_playerInSightRange && !_playerInAttackRange) SetAction(_chaseAction);
            else if (_playerInSightRange && _playerInAttackRange) SetAction(_attackAction);
        }

        private void SetAction(IAction newAction)
        {
            _currentAction = newAction ?? throw new NullReferenceException(nameof(newAction), null);
        }

        public virtual void Death()
        {
            _isDead = true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, walkPointRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sightRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}