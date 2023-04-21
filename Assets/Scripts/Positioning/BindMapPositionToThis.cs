using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindMapPositionToThis : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private GameObject _mapIcon;
    [SerializeField] private Waypoint _waypoint;

    private void Start()
    {
        _waypoint = new Waypoint();
        _waypoint.WorldspaceWaypoint = gameObject;
        _waypoint.MapWaypoint = _mapIcon;
    }

    private void Update()
    {
        this.transform.position = _camera.transform.position;
        var mapPoint = _waypoint.WorldspaceToMap();
        var mapRect = _waypoint.MapWaypoint.transform.parent.GetComponent<RectTransform>();
        var waypointRect = _waypoint.MapWaypoint.GetComponent<RectTransform>();

        waypointRect.anchoredPosition = new Vector2(-mapPoint.x * mapRect.sizeDelta.x, -mapPoint.y * mapRect.sizeDelta.y);
    }
}
