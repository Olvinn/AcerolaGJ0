using System;
using System.Collections.Generic;
using System.Text;
using Stages;
using UnityEngine;

namespace Controllers
{
    public class StageController : Singleton<StageController>
    {
        [SerializeField] private Stage _defaultStage;
        [SerializeField] private NetworkDebugStage _networkDebug;
        [SerializeField] private GameStage _gameStage;

        private Dictionary<StageType, Stage> _stages;

        private Stack<Stage> _stack;
        
        private Stage _currentStage;

        private void Start()
        {
            _stages = new();
            _stages.Add(_networkDebug.GetStageType(), _networkDebug);
            _stages.Add(_gameStage.GetStageType(), _gameStage);

            _stack = new();
            
            _currentStage = _defaultStage;
            _currentStage.Open();
        }

        public void OpenStage(StageType type)
        {
            var newStage = _stages[type];
            if (_currentStage == newStage) return;
            _stack.Push(newStage);
            _currentStage.Close();
            _currentStage = newStage;
            _currentStage.Open();
        }

        public void Back()
        {
            if (!_stack.TryPop(out var oldStage)) return;
            oldStage.Close();
            if (!_stack.TryPeek(out var newStage)) newStage = _defaultStage;
            _currentStage = newStage;
            _currentStage.Open();
        }

        public string GetStack()
        {
            if (_stack == null) return String.Empty;
            
            StringBuilder result = new ();
            foreach (var stage in _stack.ToArray())
                result.AppendLine(stage.ToString());
            return result.ToString();
        }

        public string GetCurrent()
        {
            if (_currentStage == null) return String.Empty;
            
            return _currentStage.ToString();
        }
    }
}