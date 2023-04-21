using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint
{
    public GameObject WorldspaceWaypoint { get ; set; }
    public GameObject MapWaypoint { get; set; }

    public Vector3 MapToWorldspace()
    {
        var rectTransform = MapWaypoint.GetComponent<RectTransform>();
        return PositionConverter.MapToWorld(rectTransform);
    }

    public Vector2 WorldspaceToMap()
    {
        var transform = WorldspaceWaypoint.transform;
        return PositionConverter.WorldToMap(transform);
    }
}
