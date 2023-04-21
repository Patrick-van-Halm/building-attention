using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UtilityType", menuName = "Map/UtilityType")]
public class UtilityType : ScriptableObject
{
    [SerializeField] private string title;

    [SerializeField] private Sprite icon;

    public string Title { get { return title; } }

    public Sprite Icon { get { return icon; } }
}
