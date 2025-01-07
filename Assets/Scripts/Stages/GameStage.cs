using System.Collections;
using System.Collections.Generic;
using Commands;
using TMPro;
using UI;
using Units;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Stages
{
    public class GameStage : Stage
    {
        [SerializeField] private GameObject _gameWindow;
        
        [SerializeField] private Transform _hints;
        [SerializeField] private HpBar _playerHP;
        [SerializeField] private Transform _cross;
        [SerializeField] private TextMeshProUGUI _magazineLabel;
        
        private GameObject _hintPrefab;
        private UnitModel _playerModel;
        
        private Vector2 _crossPos;
        private List<Hint> _unusedHints;
        private Dictionary<int, Hint> _hintsInUse;

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

        public void SetPlayerModel(UnitModel playerModel)
        {
            _playerModel = playerModel;
            _playerHP.SetValue(playerModel.hp, playerModel.maxHp);
            _magazineLabel.text = $"{playerModel.weapon.magazineCapacity}/{playerModel.weapon.magazineCapacity}";
        }

        protected override void OnOpen()
        {
            _gameWindow.SetActive(true);
            
            CommandBus.singleton.RegisterHandler<UpdateAim>(UpdateAim);
            CommandBus.singleton.RegisterHandler<ShowHint>(ShowHint);
            CommandBus.singleton.RegisterHandler<HideHint>(HideHint);
            CommandBus.singleton.RegisterHandler<UpdatePlayer>(UpdatePlayer);
            CommandBus.singleton.RegisterHandler<UpdatePlayerWeapon>(UpdateWeapon);
        }

        protected override void OnClose()
        {
            _gameWindow.SetActive(false);
            
            CommandBus.singleton.RemoveHandler<UpdateAim>(UpdateAim);
            CommandBus.singleton.RemoveHandler<ShowHint>(ShowHint);
            CommandBus.singleton.RemoveHandler<HideHint>(HideHint);
            CommandBus.singleton.RemoveHandler<UpdatePlayer>(UpdatePlayer);
            CommandBus.singleton.RemoveHandler<UpdatePlayerWeapon>(UpdateWeapon);
        }

        protected override void OnUpdate()
        {
        }
        
        protected override void OnLateUpdate()
        {
            _cross.transform.position = _crossPos;
        }
        
        private void UpdateAim(UpdateAim data)
        {
            _crossPos = data.Pos;
            _cross.gameObject.SetActive(data.Show);
        }
        
        private void ShowHint(ShowHint data)
        {
            if (_unusedHints.Count == 0)
                InstantiateHint();

            var hint = _unusedHints[0];
            _unusedHints.Remove(hint);
            _hintsInUse.Add(data.Id, hint);
            hint.gameObject.SetActive(true);
            Color color = _hintsInUse.Count switch
            {
                1 => GameConfigsAndSettings.instance.config.mainUseColor,
                2 => GameConfigsAndSettings.instance.config.secondaryUseColor,
                _ => Color.magenta
            };
            hint.SetHint(color, data.Text, data.Pos);
        }

        private void HideHint(HideHint data)
        {
            Hint hint;
            _hintsInUse.Remove(data.Id, out hint);
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
        
        private void UpdatePlayer(UpdatePlayer data)
        {
            _playerHP.SetValue(data.CurrentHP, data.MaxHP);
        }

        private void UpdateWeapon(UpdatePlayerWeapon data)
        {
            _magazineLabel.text = $"{data.CurrentMag}/{data.MaxMag}";
        }
    }
}
