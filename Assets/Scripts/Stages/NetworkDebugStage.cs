using UnityEngine;

namespace Stages
{
    public class NetworkDebugStage : Stage
    {
        [SerializeField] private GameObject _window;

        public override StageType GetStageType()
        {
            return StageType.NetworkDebug;
        }

        protected override void OnOpen()
        {
            _window.SetActive(true);
        }

        protected override void OnClose()
        {
            _window.SetActive(false);
        }
    }
}
