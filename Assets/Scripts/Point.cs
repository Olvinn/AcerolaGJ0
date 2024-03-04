using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public static Dictionary<PointType,List<Point>> points;
    [SerializeField] private PointType _type;

    private void Awake()
    {
        RegisterPoint();
    }

    private void RegisterPoint()
    {
        if (points == null)
            points = new Dictionary<PointType, List<Point>>();
        
        switch (_type)
        {
            case PointType.Spawn:
                if (!points.ContainsKey(_type))
                    points.Add(_type, new List<Point>());
                points[_type].Add(this);
                break;
        }
    }
}

public enum PointType
{
    Spawn
}
