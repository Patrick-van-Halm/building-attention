using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class RegionScanner : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private Camera ARCamera;

    [SerializeField] private Transform testObject;

    private float minHeight = 2;

    private void Update()
    {
        var screenCenter = ARCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        List<ARRaycastHit> raycastHits = new();
        raycastManager.Raycast(screenCenter, raycastHits, TrackableType.Planes);

        if (raycastHits.Count > 0)
        {
            float lastPlaneHeight = raycastHits[^1].pose.position.y;
            if (lastPlaneHeight < minHeight)
            {
                minHeight = lastPlaneHeight;
            }
            testObject.position = new Vector3(ARCamera.transform.position.x, minHeight, ARCamera.transform.position.z);
        }
    }
}
