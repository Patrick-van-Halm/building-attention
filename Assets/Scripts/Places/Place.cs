using System.Collections;
using UnityEngine;

public abstract class Place : ScriptableObject
{
    [SerializeField] private Vector3 worldPosition;
    [SerializeField] private Vector2 mapPosition;
    [SerializeField] private int floor;

    public Vector3 WorldPosition { get { return worldPosition; } }
    public Vector2 MapPosition { get { return mapPosition; } }
    public int Floor { get { return floor;} }
}