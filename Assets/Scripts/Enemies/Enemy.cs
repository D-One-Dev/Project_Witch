using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class Enemy
    {
        public readonly EnemyUnitBase EnemyUnit;
        public readonly NavMeshAgent Agent;
        public readonly Transform Transform;
        public readonly Animator Animator;

        public Enemy(NavMeshAgent agent, Transform transform, Animator animator, EnemyUnitBase enemyUnit)
        {
            Agent = agent;
            Transform = transform;
            EnemyUnit = enemyUnit;
            Animator = animator;
        }
        
        public void DoAction(IAction action)
        {
            action?.PerformAction(this);
        }
    }
}