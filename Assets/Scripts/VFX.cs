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
    private Vector3[] _pointsDir;

    private void Update()
    {
        if (_lr)
        {
            for (int i = 0; i < _lr.positionCount; i++)
            {
                _lr.SetPosition(i, _lr.GetPosition(i) + _pointsDir[i] * Time.deltaTime);
            }

            _lr.widthMultiplier += Time.deltaTime;
            Color color = _lr.startColor;
            color.a -= Time.deltaTime;
            _lr.startColor = color;
            _lr.endColor = color;
        }
    }

    public void StartEffect()
    {
        if (_timer)
            _timer.StartTimer(1);
        if (_ps)
            _ps.Play();
    }
    
    public void StartEffect(Vector3 start, Vector3 end)
    {
        if (_timer)
            _timer.StartTimer(1);
        if (_lr)
        {
            _lr.widthMultiplier = 1;
            Color color = _lr.startColor;
            color.a = 1;
            _lr.startColor = color;
            
            float length = Vector3.Distance(start, end);
            int segments = (int)length + 1;
            Vector3[] newPoints = new Vector3[segments];
            _pointsDir = new Vector3[segments];
            for (int i = 0; i < segments; i++)
            {
                _pointsDir[i] = Random.insideUnitSphere;
                
                if (i == 0)
                {
                    newPoints[i] = start;
                    continue;
                }

                if (i == segments - 1)
                {
                    newPoints[i] = end;
                    continue;
                }
                    
                newPoints[i] = Vector3.Lerp(start, end, (float)i / segments);
            }
            _lr.positionCount = newPoints.Length;
            _lr.SetPositions(newPoints);
        }
    }

    public void Return()
    {
        if (returnOnDone)
            VFXController.Instance.Return(this);
    }

    private void OnParticleSystemStopped()
    {
        Return();
    }
}
