using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class Enemy
    {
        public readonly EnemyUnitBase EnemyUnit;
        public readonly NavMeshAgent Agent;
        public readonly Transform Transform;

        public Enemy(NavMeshAgent agent, Transform transform, EnemyUnitBase enemyUnit)
        {
            Agent = agent;
            Transform = transform;
            EnemyUnit = enemyUnit;
        }
        
        public void DoAction(IAction action)
        {
            action?.PerformAction(this);
        }
    }
}