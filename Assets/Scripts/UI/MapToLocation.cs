using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapToLocation : MonoBehaviour
{
    [SerializeField] private RectTransform mapTransform;
    private RectTransform _transform;
    private void Start()
    {
        _transform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (!FontysAPIManager.Instance.HasLocation) return;
        FloorDimensions floorDimensions = FontysAPIManager.Instance.GetCurrentFloorDimensions();
        float x = FontysAPIManager.Instance.LocationCoordinate.x / floorDimensions.floorWidth * mapTransform.sizeDelta.x;
        float y = FontysAPIManager.Instance.LocationCoordinate.y / floorDimensions.floorLength * mapTransform.sizeDelta.y;
        _transform.localPosition = new(x, -y);
    }
}
