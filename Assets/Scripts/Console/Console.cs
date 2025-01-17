using System;
using System.Collections.Generic;
using System.Text;
using Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Console
{
    public class Console : Singleton<Console>
    {
        [SerializeField] private GameObject _window;
        [SerializeField] private TextMeshProUGUI _logs;
        [SerializeField] private TMP_InputField _input;

        int _autofillIndex, _repeatIndex;
        string _autofillStart;

        private bool _isOpen;

        List<string> _logsMsgs;

        ConsoleCommands _commands = new ();
        List<string> _history;

        private void Start()
        {
            _logsMsgs = new List<string>();
            _history = new List<string>();
            _input.onSubmit.AddListener(Submit);
            _autofillStart = string.Empty;

            Application.logMessageReceived += CaptureLog;
            InputController.Instance.onConsole += Switch;
        }

        private void Update()
        {
            if (!_isOpen)
                return;
            
            EventSystem.current.SetSelectedGameObject(_input.gameObject);
            _input.Select();
            
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (_autofillStart == string.Empty || _input.text.Length < _autofillStart.Length)
                    _autofillStart = _input.text;
                Autofill();
                _input.Select();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
                RepeatCommand(-1);
            
            if (Input.GetKeyDown(KeyCode.DownArrow))
                RepeatCommand(1);
        }

        private void CaptureLog(string condition, string stackTrace, LogType type)
        {
            LogText(condition);
        }

        private void Submit(string txt)
        {
            _input.Select();
            _input.text = string.Empty;
            _autofillStart = string.Empty;
            _autofillIndex = 0;
            _repeatIndex = _history.Count - 1;
            _input.ActivateInputField();
            LogText(txt);
            ParseCommand(txt);
        }

        public void LogText(string txt)
        {
            _logsMsgs.Add(txt);
            if (_logsMsgs.Count > 34)
                _logsMsgs.RemoveAt(0);

            StringBuilder b = new StringBuilder();
            foreach (var t in _logsMsgs)
                b.AppendLine(t);

            if (_logs)
                _logs.text = b.ToString();
        }

        public void ParseCommand(string txt)
        {
            string[] words = txt.Split(' ');

            try
            {
                ConsoleCommand c;
                if (_commands.Commands.TryGetValue(words[0], out c))
                {
                    var arg = words.Length > 1 ? txt.Remove(0, words[0].Length + 1) : "";
                    c.Invoke(arg);
                    _history.Add(txt);
                    _repeatIndex = 0;
                    return;
                }
            }
            catch (Exception e)
            {
                LogText($"[CONSOLE] {e.Message}\n{e.StackTrace}");
                return;
            }
            LogText($"[CONSOLE] There is no command '{txt}'");

            _history.Add(txt);
            _repeatIndex = 0;
        }
        
        public void Switch()
        {
            if (_isOpen)
                Close();
            else
                Open();
        }

        public void Open()
        {
            _isOpen = true;
            _window.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_input.gameObject);
            _input.Select();
        }

        public void Close()
        {
            _isOpen = false;
            _window.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
        }

        private void Autofill()
        {
            List<string> temp = new List<string>();
            foreach (var c in _commands.Commands.Keys)
            {
                if (c.Contains(_autofillStart))
                    temp.Add(c);
            }

            if (_autofillIndex >= temp.Count)
                _autofillIndex = 0;

            if (temp.Count > 0)
            {
                _input.text = temp[_autofillIndex];
                _input.caretPosition = _input.text.Length;
            }

            _autofillIndex++;
        }

        private void RepeatCommand(int delta)
        {
            if (_history.Count == 0)
                return;

            _repeatIndex += delta;
            if (_repeatIndex < 0)
                _repeatIndex = _history.Count - 1;
            if (_repeatIndex >= _history.Count)
                _repeatIndex = 0;

            _input.text = _history[_repeatIndex];
            _input.caretPosition = _input.text.Length;
        }
    }
}
