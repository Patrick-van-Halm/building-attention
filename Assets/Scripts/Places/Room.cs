using System.Collections;
using UnityEngine;

public abstract class Room: Place
{
    [SerializeField] private string roomName;
    [SerializeField] private string roomNumber;
    [TextArea(2, 3)]
    [SerializeField] private string label;

    public string Label { get { return label; } }

    public abstract string Status();

    public abstract bool StatusValue();
}
