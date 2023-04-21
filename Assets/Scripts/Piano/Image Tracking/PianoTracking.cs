using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class PianoTracking : MonoBehaviour
{
    [SerializeField] private GameObject[] _placeableObjects;

    [SerializeField] private Dictionary<string, GameObject> _spawnedObjects = new();
    private ARTrackedImageManager _arTrackingManager;

    private void Awake()
    {
        _arTrackingManager = GetComponent<ARTrackedImageManager>();

        foreach (GameObject o in _placeableObjects)
        {
            GameObject newPrefab = Instantiate(o, Vector3.zero, Quaternion.identity);
            newPrefab.name = o.name;
            _spawnedObjects.Add(newPrefab.name, newPrefab);
            newPrefab.SetActive(false);
        }
    }

    void OnEnable() => _arTrackingManager.trackedImagesChanged += OnChanged;

    void OnDisable() => _arTrackingManager.trackedImagesChanged -= OnChanged;

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage image in eventArgs.added)
        {
            UpdateImage(image);
        }

        foreach (ARTrackedImage image in eventArgs.updated)
        {
            UpdateImage(image);
        }

        foreach (ARTrackedImage image in eventArgs.removed)
        {
            if (!_spawnedObjects.ContainsKey(image.referenceImage.name)) return;
            _spawnedObjects[image.referenceImage.name].SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage image)
    {
        string name = image.referenceImage.name;
        Vector3 position = image.transform.position;
        if (!_spawnedObjects.ContainsKey(name)) return;

        GameObject prefab = _spawnedObjects[name];
        prefab.transform.position = position;
        prefab.SetActive(true);

        foreach (GameObject o in _spawnedObjects.Values)
        {
            if (o.name != name)
            {
                o.SetActive(false);
            }
        }
    }

}
