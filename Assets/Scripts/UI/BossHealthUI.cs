using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class BossHealthUI
    {
        [Inject(Id = "BossHealthBar")]
        private readonly Image _bossHealthBar;
        public void ChangeHpBarValue(float value)
        {
            _bossHealthBar.fillAmount = value;
        }
    }
}