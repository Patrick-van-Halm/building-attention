using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSManager : SingletonMonoBehaviour<GPSManager>
{
    [SerializeField] private Vector2 _originGPS;
    [SerializeField, Range(1, 10)] private float _accuracyMeters = 10;
    [SerializeField, Range(1, 10)] private float _updateMeters = 10;
	private float metersPerLat;
	private float metersPerLon;

	public bool IsGPSAllowed => Input.location.isEnabledByUser;

    public bool IsGPSRunning => Input.location.status == LocationServiceStatus.Running;

    public bool IsGPSStarting => Input.location.status == LocationServiceStatus.Initializing;

	public void StartGPS()
    {
        if (!IsGPSAllowed || IsGPSRunning || IsGPSStarting) return;
		StartCoroutine(CoroStartGPS());
    }

    public Vector2 GetGPSPosition()
    {
        if (!IsGPSRunning) return Vector3.zero;
        return new(Input.location.lastData.latitude, Input.location.lastData.longitude);
    }

    public Vector3 ConvertPosition(Vector2 gpsLocation)
    {
		FindMetersPerLat(_originGPS.x);
		float zPosition = metersPerLat * (gpsLocation.x - _originGPS.x); //Calc current lat
		float xPosition = metersPerLon * (gpsLocation.y - _originGPS.y); //Calc current lon
		return new Vector3((float)xPosition, 0, (float)zPosition);
	}

	private void FindMetersPerLat(float lat) // Compute lengths of degrees
	{
		float m1 = 111132.92f;    // latitude calculation term 1
		float m2 = -559.82f;        // latitude calculation term 2
		float m3 = 1.175f;      // latitude calculation term 3
		float m4 = -0.0023f;        // latitude calculation term 4
		float p1 = 111412.84f;    // longitude calculation term 1
		float p2 = -93.5f;      // longitude calculation term 2
		float p3 = 0.118f;      // longitude calculation term 3

		lat = lat * Mathf.Deg2Rad;

		// Calculate the length of a degree of latitude and longitude in meters
		metersPerLat = m1 + (m2 * Mathf.Cos(2 * (float)lat)) + (m3 * Mathf.Cos(4 * (float)lat)) + (m4 * Mathf.Cos(6 * (float)lat));
		metersPerLon = (p1 * Mathf.Cos((float)lat)) + (p2 * Mathf.Cos(3 * (float)lat)) + (p3 * Mathf.Cos(5 * (float)lat));
	}

	private IEnumerator CoroStartGPS()
    {
        // Starts the location service.
        Input.location.Start(_accuracyMeters, _updateMeters);

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            Debug.LogError("GPS Initialization: Timed out");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("GPS Initialization: Failed");
            yield break;
        }
    }

    private void OnDisable()
    {
        if (!IsGPSRunning) return;

        // Stops the location service if there is no need to query location updates continuously.
        Input.location.Stop();
    }
}
