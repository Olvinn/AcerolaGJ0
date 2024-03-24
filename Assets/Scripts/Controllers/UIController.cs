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
        [SerializeField] private Transform _cross;
        [SerializeField] private TextMeshProUGUI _magazineLabel;

        private GameObject _hintPrefab;

        private List<Hint> _unusedHints;
        private Dictionary<int, Hint> _hintsInUse;
        private int _counter;

        private Vector2 _crossPos;

        private IEnumerator Start()
        {
            _unusedHints = new List<Hint>();
            _hintsInUse = new Dictionary<int, Hint>();

            var handler = Addressables.LoadAssetAsync<GameObject>(GameConfigsAndSettings.instance.config.hint);
            while (!handler.IsDone)
                yield return null;
            _hintPrefab = handler.Result;
        
            InstantiateHint();
        }

        private void LateUpdate()
        {
            _cross.transform.position = _crossPos;
        }

        public int ShowHint(Color color, string text, Vector3 pos)
        {
            if (_unusedHints.Count == 0)
                InstantiateHint();

            var hint = _unusedHints[0];
            _unusedHints.Remove(hint);
            _counter++;
            _hintsInUse.Add(_counter, hint);
            hint.gameObject.SetActive(true);
            hint.SetHint(color, text, pos);
            return _counter;
        }

        public void HideHint(int key)
        {
            Hint hint;
            _hintsInUse.Remove(key, out hint);
            if (hint)
            {
                _unusedHints.Add(hint);
                hint.gameObject.SetActive(false);
            }
        }

        public void UpdatePlayerHP(float value, float max)
        {
            _playerHP.SetValue(value, max);
        }

        public void UpdateAim(bool show, Vector2 pos)
        {
            _crossPos = pos;
            _cross.gameObject.SetActive(show);
        }

        public void UpdateMagazineMagazine(int current, int max)
        {
            _magazineLabel.text = $"{current}/{max}";
        }

        private void InstantiateHint()
        {
            var hint = Instantiate(_hintPrefab, _hints).GetComponent<Hint>();
            _unusedHints.Add(hint);
            hint.gameObject.SetActive(false);
        }
    }
}
