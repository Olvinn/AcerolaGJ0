using System;
using Controllers;
using UnityEngine;

public class VFX : MonoBehaviour
{
    public EffectType type;
    public bool returnOnDone;
    [SerializeField] private Timer _timer;
    [SerializeField] private ParticleSystem _ps;

    public void StartEffect()
    {
        if (_timer)
            _timer.StartTimer(1);
        if (_ps)
            _ps.Play();
    }

    public void Return()
    {
        if (returnOnDone)
            VFXController.instance.Return(this);
    }

    private void OnParticleSystemStopped()
    {
        Return();
    }
}
