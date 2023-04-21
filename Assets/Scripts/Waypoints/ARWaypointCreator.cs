using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARWaypointCreator : MonoBehaviour
{
    [SerializeField] private GameObject _arWaypointPrefab;
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private Camera cam;
    [SerializeField] private bool createWaypointsOnTap;
    private bool isTouching;

    private void Start()
    {
        if(!WaypointManager.Instance)
        {
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        if (!createWaypointsOnTap) return;
        if (Input.touchCount == 0)
        {
            isTouching = false;
            return;
        }
        if (isTouching) return;
        if (Input.touchCount > 1) return;
        isTouching = true;
        List<ARRaycastHit> raycastHits = new();
        Touch touch = Input.touches[0];
        if (raycastManager.Raycast(cam.ScreenPointToRay(touch.position), raycastHits)) {
            var hit = raycastHits.Find(r => r.trackable.gameObject.CompareTag("ARPlane"));
            if(hit == null) return;
            var obj = Instantiate(_arWaypointPrefab, hit.pose.position, Quaternion.identity, transform);
            WaypointManager.Instance.AddWaypoint(new() { WorldspaceWaypoint = obj });
        }
    }

    public void SpawnFromWaypoint(Waypoint waypoint)
    {
        if (waypoint == null) return;
        if (waypoint.MapWaypoint == null) return;
        var normalized = waypoint.MapToWorldspace();
        var ws = new Vector3(-normalized.x * 10, 0, -normalized.z * 10);
        var obj = Instantiate(_arWaypointPrefab, transform);
        obj.transform.localPosition = ws;
        waypoint.WorldspaceWaypoint = obj;
    }
}
