using Controllers;
using UnityEngine;

public class VFX : MonoBehaviour
{
    public EffectType type;
    [SerializeField] private Timer _timer;

    public void StartEffect()
    {
        if (_timer)
            _timer.StartTimer(1);
    }

    public void Return()
    {
        VFXController.instance.Return(this);
    }
}
