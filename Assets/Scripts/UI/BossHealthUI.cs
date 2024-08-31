using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BossHealthUI : MonoBehaviour
    {
        [SerializeField] private Image hpBar;

        public static BossHealthUI Instance;

        private void Awake() => Instance = this;

        public void ChangeHpBarValue(float value) => hpBar.fillAmount = value;
    }
}