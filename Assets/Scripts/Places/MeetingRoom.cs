using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "MeetingRoom", menuName = "Map/Places/Meeting Room")]
public class MeetingRoom : Room
{
    [SerializeField] private int capacity;

    public bool IsAvailableNow()
    {
        return true;
    }

    public override string Status()
    {
        if (IsAvailableNow())
        {
            return "Free";
        }
        else
        {
            return "Occupied";
        }
    }

    public override bool StatusValue()
    {
        return IsAvailableNow();
    }
}
