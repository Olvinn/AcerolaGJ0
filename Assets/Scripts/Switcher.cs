using UnityEngine;
using UnityEngine.Events;

public class Switcher : MonoBehaviour
{
    [SerializeField] private UnityEvent _onOn, _onOff;
    [SerializeField] private bool _isOn;

    private void Start()
    {
        if (_isOn) 
            On();
        else 
            Off();
    }

    public void On()
    {
        _onOn?.Invoke();
        _isOn = true;
    }

    public void Off()
    {
        _onOff?.Invoke();
        _isOn = false;
    }

    public void Switch()
    {
        _isOn = !_isOn;
        if (_isOn) 
            On();
        else 
            Off();
    }
}
