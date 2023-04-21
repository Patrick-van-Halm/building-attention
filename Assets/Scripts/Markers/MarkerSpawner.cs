using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerSpawner : MonoBehaviour
{
    [SerializeField] private List<Place> places;

    [SerializeField] private GameObject markerPrefab;
    [SerializeField] private GameObject markerParent;

    private void Start()
    {
        foreach (Place place in places)
        {
            Marker newMarker = Instantiate(markerPrefab).GetComponent<Marker>();
            newMarker.transform.SetParent(markerParent.transform);
            newMarker.GetComponent<RectTransform>().anchoredPosition = place.MapPosition;
            newMarker.transform.localScale = new Vector3(1f, 1f, 1f);
            newMarker.name = place.name;
            newMarker.SetPlace(place);

            WaypointManager.Instance.AddWaypoint(new() { MapWaypoint = newMarker.gameObject });
        }
    }
}