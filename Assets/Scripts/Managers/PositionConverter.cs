using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionConverter : SingletonMonoBehaviour<PositionConverter>
{
    public enum Method
    {
        GPS,
        Wifi
    }
    [SerializeField] private Method method;

    private void Start()
    {
        if (method == Method.GPS && (!GPSManager.Instance || !GPSManager.Instance.IsGPSAllowed))
        {
            Debug.LogError("GPS is not allowed or enabled.");
            method = Method.Wifi;
        }

        if (method == Method.GPS) GPSManager.Instance.StartGPS();
    }

    public Vector3 GetPosition()
    {
        Vector3 position = Vector3.zero;
        if (method == Method.GPS)
        {
            var gpsPosition = GPSManager.Instance.GetGPSPosition();
            if(gpsPosition == Vector2.zero) return position;
            position = GPSManager.Instance.ConvertPosition(gpsPosition);
        }

        return position;
    }

    public static Vector3 MapToWorld(RectTransform transform)
    {
        var position = transform.anchoredPosition;
        var parentTransform = transform.parent.GetComponent<RectTransform>();

        return new(position.x / parentTransform.sizeDelta.x, 0, position.y / parentTransform.sizeDelta.y);
    }

    public static Vector2 WorldToMap(Transform transform)
    {
        var position = transform.localPosition;
        return WorldToMap(position);
    }

    public static Vector2 WorldToMap(Vector3 position)
    {
        var worldPosition3D = new Vector3(position.x / 10, 0, position.z / 10);
        return new Vector2(worldPosition3D.x, worldPosition3D.z);
    }
}
