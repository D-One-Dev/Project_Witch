using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        public static Player Instance;

        private void Awake() => Instance = this;
    }
}