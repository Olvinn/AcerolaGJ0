using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HpBar : MonoBehaviour
    {
        [SerializeField] private Image _bar;
        [SerializeField] private TextMeshProUGUI _label;

        public void SetValue(float value, float max)
        {
            _bar.fillAmount = value / max;
            _label.text = value.ToString();
        }
    }
}
