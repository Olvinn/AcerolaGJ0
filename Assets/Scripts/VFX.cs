using System;
using Controllers;
using UnityEngine;
using Random = UnityEngine.Random;

public class VFX : MonoBehaviour
{
    public EffectType type;
    public bool returnOnDone;
    [SerializeField] private Timer _timer;
    [SerializeField] private ParticleSystem _ps;
    [SerializeField] private LineRenderer _lr;

    private void Update()
    {
        if (_lr)
        {
            for (int i = 0; i < _lr.positionCount; i++)
            {
                _lr.SetPosition(i, _lr.GetPosition(i) + Random.insideUnitSphere * Time.deltaTime);
            }
        }
    }

    public void StartEffect()
    {
        if (_timer)
            _timer.StartTimer(1);
        if (_ps)
            _ps.Play();
    }
    
    public void StartEffect(Vector3[] points)
    {
        if (_timer)
            _timer.StartTimer(1);
        if (_lr)
        {
            _lr.positionCount = points.Length;
            _lr.SetPositions(points);
        }
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
