using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImagePositioningManager : SingletonMonoBehaviour<ImagePositioningManager>
{
    [SerializeField] private ARSession _arSession;
    [SerializeField] private Transform _arSessionOrigin;
    [SerializeField] private Transform _camera;
    [SerializeField] private ARTrackedImageManager _imageTracker;
    [SerializeField] private List<ImagePositioningData> _imagePositions;

    protected override void Awake()
    {
        base.Awake();
        _imageTracker.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs e)
    {
        foreach(var tracked in e.added)
        {
            print(tracked.referenceImage.name);
            var positionData = _imagePositions.Find(i => i.Image == tracked.referenceImage.texture);
            if (positionData == null) continue;
            
            PositionAROrigin(positionData);
            break;
        }
    }

    private void PositionAROrigin(ImagePositioningData positionData)
    {
        _arSession.Reset();
        _arSessionOrigin.position = positionData.PositionTransform.position;
        _arSessionOrigin.rotation = positionData.PositionTransform.rotation;
    }
}

[Serializable]
public class ImagePositioningData
{
    public Texture2D Image;
    public Transform PositionTransform;
}
