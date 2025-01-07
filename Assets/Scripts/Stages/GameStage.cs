using Commands;
using TMPro;
using UI;
using UnityEngine;

namespace Stages
{
    public class GameStage : Stage
    {
        [SerializeField] private GameObject _gameWindow;
        
        [SerializeField] private Transform _hints;
        [SerializeField] private HpBar _playerHP;
        [SerializeField] private Transform _cross;
        [SerializeField] private TextMeshProUGUI _magazineLabel;
        
        private Vector2 _crossPos;
        
        protected override void OnOpen()
        {
            _gameWindow.SetActive(true);
            
            CommandBus.singleton.RegisterHandler<UpdateAim>(UpdateAim);
        }

        protected override void OnClose()
        {
            _gameWindow.SetActive(false);
            
            CommandBus.singleton.RemoveHandler<UpdateAim>(UpdateAim);
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
    }
}
