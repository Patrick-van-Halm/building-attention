using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class FontysAPIManager : SingletonMonoBehaviour<FontysAPIManager>
{
    [SerializeField] private string apiUri = "https://api.fhict.nl";
    [SerializeField] private string overrideToken;
    [SerializeField] private float secondsPerLocationRequest;
    [SerializeField] private LocationDTO location = null;

    [SerializeField] private List<FloorDimensions> floors;
    public FloorDimensions GetFloorDimensions(string name) => floors.Find(f => f.floorLocationName == name);
    public FloorDimensions GetCurrentFloorDimensions() => GetFloorDimensions(location.locationMapHierarchy) ?? GetFloorDimensions("EHV>TQ>2e");
    public bool HasLocation => location != null && location.timestamp > 0;

    public LocationDTO.Locationcoordinate LocationCoordinate => location.locationCoordinate;
    private bool isGettingLocation;

    private void Update()
    {
        if (isGettingLocation) return;
        isGettingLocation = true;
        StartCoroutine(LocationUpdater());
    }

    private IEnumerator LocationUpdater()
    {
        yield return GetLocation();
        yield return new WaitForSeconds(secondsPerLocationRequest);
        isGettingLocation = false;
    }

    private IEnumerator GetLocation()
    {
        using UnityWebRequest req = UnityWebRequest.Get($"{apiUri}/location/current");
        req.SetRequestHeader("Authorization", $"Bearer {overrideToken}");
        req.timeout = 5;
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Request failed: {req.error}");
            yield break;
        }

        try
        {
            float prevX = location.locationCoordinate.x;
            float prevY = location.locationCoordinate.y;
            float prevTimestamp = location.timestamp;
            location = JsonUtility.FromJson<LocationDTO>(req.downloadHandler.text.TrimStart('[').TrimEnd(']'));


            if (prevTimestamp != location.timestamp)
                Debug.Log("New update received");

            if (prevX != location.locationCoordinate.x || prevY != location.locationCoordinate.y)
                Debug.Log("Location changed");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error when parsing JSON: {ex}");
        }
    }
}
