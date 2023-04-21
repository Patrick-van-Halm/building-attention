using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapWaypointCreator : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _mapWaypointPrefab;
    private RectTransform _rectTransform;
    private void Start()
    {
        if(!WaypointManager.Instance)
        {
            enabled = false;
            return;
        }
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var obj = Instantiate(_mapWaypointPrefab, eventData.position, Quaternion.identity, _rectTransform);
        WaypointManager.Instance.AddWaypoint(new() { MapWaypoint = obj });
    }
}
