using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARGroundDetector : MonoBehaviour
{
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private Camera ARCamera;

    private float minHeight = 2;

    private ARPlane groundPlane;

    void Update()
    {
        var screenCenter = ARCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        List<ARRaycastHit> raycastHits = new();
        raycastManager.Raycast(screenCenter, raycastHits, TrackableType.Planes);

        if (raycastHits.Count > 0)
        {
            // Get the last plane that was hit
            ARPlane lowestPlane = planeManager.GetPlane(raycastHits[^1].trackableId);

            float lastPlaneHeight = lowestPlane.center.y;
            if (lastPlaneHeight < minHeight)
            {
                minHeight = lastPlaneHeight;
                groundPlane = lowestPlane;
            }
        }
    }

    public ARPlane GetCurrentGroundPlane()
    {
        return groundPlane;
    }

    public float GetCurrentGroundYLevel()
    {
        return groundPlane.center.y;
    }
}
