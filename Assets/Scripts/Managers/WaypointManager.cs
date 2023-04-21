using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaypointManager : SingletonMonoBehaviour<WaypointManager>
{
    [SerializeField] private ARWaypointCreator arWaypointCreator;
    [SerializeField] private GameObject waypointUIPrefab;

    private List<Waypoint> waypoints = new();

    public Waypoint GetWaypointByWorldspaceMarker(GameObject go) => waypoints.Find(w => w.WorldspaceWaypoint == go);
    public Waypoint GetWaypointByMapMarker(GameObject go) => waypoints.Find(w => w.MapWaypoint == go);

    public void AddWaypoint(Waypoint waypoint) 
    {
        if(waypoint.MapWaypoint != null)
            arWaypointCreator.SpawnFromWaypoint(waypoint);
        waypoints.Add(waypoint);

        waypoint.MapWaypoint.GetComponent<Button>().onClick.AddListener(delegate { WaypointSharer.Instance.WaypointSelected(waypoint.MapWaypoint.GetComponent<RectTransform>()); });
    }

    public void RemoveWaypoint(Waypoint waypoint)
    {
        if (!waypoints.Contains(waypoint)) return;
        Destroy(waypoint.MapWaypoint);
        Destroy(waypoint.WorldspaceWaypoint);
        waypoints.Remove(waypoint);
    }
}
