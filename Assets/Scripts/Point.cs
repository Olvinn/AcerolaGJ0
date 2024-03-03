using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public static List<Point> points;

    private void Awake()
    {
        RegisterPoint();
    }

    private void RegisterPoint()
    {
        if (points == null)
            points = new List<Point>();
        points.Add(this);
    }
}
