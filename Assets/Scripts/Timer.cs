using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public UnityEvent onTime;
    private Coroutine _timer;

    public void StartTimer(float time)
    {
        if (_timer != null)
            StopCoroutine(_timer);
        _timer = StartCoroutine(Counting(time));
    }

    IEnumerator Counting(float time)
    {
        yield return new WaitForSeconds(time);
        onTime?.Invoke();
    }
}
