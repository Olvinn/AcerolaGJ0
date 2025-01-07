using UnityEngine;

namespace Stages
{
    public class GameStage : Stage
    {
        [SerializeField] private GameObject _gameWindow;
        
        protected override void OnOpen()
        {
            _gameWindow.SetActive(true);
        }

        protected override void OnClose()
        {
            _gameWindow.SetActive(false);
        }

        protected override void OnUpdate()
        {
        }
    }
}
