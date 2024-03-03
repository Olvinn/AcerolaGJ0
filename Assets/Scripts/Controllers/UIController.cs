using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Controllers
{
    public class UIController : Singleton<UIController>
    {
        [SerializeField] private Transform _hints;

        private GameObject _hintPrefab;

        private List<Hint> _unusedHints;
        private Dictionary<int, Hint> _hintsInUse;
        private int _counter;

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

        private void InstantiateHint()
        {
            var hint = Instantiate(_hintPrefab, _hints).GetComponent<Hint>();
            _unusedHints.Add(hint);
            hint.gameObject.SetActive(false);
        }
    }
}
