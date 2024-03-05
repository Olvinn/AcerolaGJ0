using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public static Dictionary<PointType,List<Point>> _points;
    [SerializeField] private PointType _type;

    public static List<Point> GetPoint(PointType type)
    {
        if (_points == null || !_points.ContainsKey(type))
        {
            _points = new Dictionary<PointType, List<Point>>();
            Debug.LogError("No any points registered!");
            GameObject emptyHandler = new GameObject("Error Point");
            emptyHandler.AddComponent<Point>();
        }

        return _points[type];
    }
    
    private void Awake()
    {
        RegisterPoint();
    }

    private void RegisterPoint()
    {
        if (_points == null)
            _points = new Dictionary<PointType, List<Point>>();
        
        switch (_type)
        {
            case PointType.Spawn:
                if (!_points.ContainsKey(_type))
                    _points.Add(_type, new List<Point>());
                _points[_type].Add(this);
                break;
        }
    }
}

public enum PointType
{
    Spawn
}
