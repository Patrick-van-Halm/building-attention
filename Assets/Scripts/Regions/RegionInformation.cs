using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Region Information")]
public class RegionInformation : ScriptableObject
{
    [SerializeField] private RegionType _type;
    [SerializeField] private int _floor;
    [SerializeField] private Vector2 _position;
    [SerializeField] private Vector2 _size;
    [SerializeField] private Color32 _color;
    public RegionType Type { get { return _type; } }
    public int Floor { get { return _floor; } }
    public Color32 Color { get { return _color; } }

    public Vector2 Position { get { return _position; } }
    public Vector2 Size { get { return _size; } }

    public enum RegionType
    {
        [Description("Technology")]
        Technology,
        [Description("Game Design & Technology")]
        GameDesignAndTechnology,
        [Description("Infrastructure")]
        Infrastructure,
        [Description("Open Learning")]
        OpenLearning,
        [Description("Smart Mobile")]
        SmartMobile,
        [Description("XR")]
        XR,
    }
}
