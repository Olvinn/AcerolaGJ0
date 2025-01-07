using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Controllers
{
    public class UIController : Singleton<UIController>
    {
        [SerializeField] private Transform _hints;
        [SerializeField] private HpBar _playerHP;
        [SerializeField] private TextMeshProUGUI _magazineLabel;


        public void UpdatePlayerHP(float value, float max)
        {
            _playerHP.SetValue(value, max);
        }

        public void UpdateMagazineMagazine(int current, int max)
        {
            _magazineLabel.text = $"{current}/{max}";
        }
    }
}
