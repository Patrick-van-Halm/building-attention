using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MoveToPlayer : MonoBehaviour
{
    public Transform target;
    public float baseRotationY;
    private float signedDirOffset;

    private void Start()
    {
        if(!GPSManager.Instance || !GPSManager.Instance.IsGPSAllowed)
        {
            enabled = false;
            return;
        }
        Input.compass.enabled = true;
        signedDirOffset = baseRotationY;
    }

    public void RecalibrateRotation()
    {
        StartCoroutine(CoroCalibration());
    }

    private IEnumerator CoroCalibration()
    {
        float degrees = 0;
        int sampleSize = 50;
        for(int i = 0; i < sampleSize; i++)
        {
            print($"{i}: {Input.compass.headingAccuracy}: {Input.compass.trueHeading}");
            degrees += -Input.compass.trueHeading;
            yield return null;
        }
        degrees /= sampleSize;
        degrees %= 360;
        print($"Average: {degrees}");
        signedDirOffset = degrees - target.localEulerAngles.y;

        var offsetFromOrigin = target.localPosition - PositionConverter.Instance.GetPosition();
        transform.position = new(offsetFromOrigin.x, .5f, offsetFromOrigin.z);
        transform.eulerAngles = new(0, signedDirOffset, 0);
    }
}
