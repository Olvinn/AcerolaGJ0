using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Controllers
{
    public class VFXController : Singleton<VFXController>
    {
        [SerializeField] private Transform _vfx;
        private Dictionary<EffectType, VFX> _prefabs;
        private Dictionary<EffectType, List<VFX>> _unusedVFX;
        
        private IEnumerator Start()
        {
            _unusedVFX = new Dictionary<EffectType, List<VFX>>();
            _prefabs = new Dictionary<EffectType, VFX>();

            var handler = Addressables.LoadAssetAsync<GameObject>(GameConfigsAndSettings.instance.config.bulletImpactVFX);
            while (!handler.IsDone)
                yield return null;
            _prefabs.Add(EffectType.BulletImpact, handler.Result.GetComponent<VFX>());
        
            InstantiateVFX(EffectType.BulletImpact);
        }

        public VFX GetEffect(EffectType type)
        {
            if (_unusedVFX[type].Count == 0)
                InstantiateVFX(type);
            var vfx = _unusedVFX[type][0];
            _unusedVFX[type].Remove(vfx);
            vfx.gameObject.SetActive(true);
            vfx.StartEffect();
            return vfx;
        }

        public void Return(VFX vfx)
        {
            vfx.gameObject.SetActive(false);
            _unusedVFX[vfx.type].Add(vfx);
        }
        
        private void InstantiateVFX(EffectType type)
        {
            var hint = Instantiate(_prefabs[type], _vfx).GetComponent<VFX>();
            if (!_unusedVFX.ContainsKey(type))
                _unusedVFX.Add(type, new List<VFX>());
            _unusedVFX[type].Add(hint);
            hint.gameObject.SetActive(false);
        }
    }

    public enum EffectType
    {
        BulletImpact,
    }
}
