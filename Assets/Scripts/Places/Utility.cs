using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Utility", menuName = "Map/Places/Utility")]
public class Utility : Place
{
    [SerializeField] private UtilityType type;

    public UtilityType Type { get { return type; } }
}