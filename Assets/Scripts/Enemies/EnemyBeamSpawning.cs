using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EnemyBeamSpawning : MonoBehaviour
    {
        [SerializeField] private GameObject laserBeam;
        
        public Transform[] point;

        private List<GameObject> _currentBeams;
        
        public void SpawnLaser()
        {
            _currentBeams = new List<GameObject>();
            
            for (int i = 0; i < point.Length; i++)
            {
                _currentBeams.Add(Instantiate(laserBeam, point[i].position, Quaternion.FromToRotation(point[i].forward, point[i].position)));
                _currentBeams[i].transform.parent = point[i].transform;
                _currentBeams[i].SetActive(false);
            }
        }

        public void ActivateLaser()
        {
            for (int i = 0; i < _currentBeams.Count; i++)
            {
                _currentBeams[i].SetActive(true);
            }
        }

        public void DespawnLaser()
        {
            for (int i = 0; i < _currentBeams.Count; i++)
            {
                Destroy(_currentBeams[i]);
            }
        }
    }
}