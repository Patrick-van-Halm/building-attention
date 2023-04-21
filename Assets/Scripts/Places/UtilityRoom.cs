using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "UtilityRoom", menuName = "Map/Places/Utility Room")]
public class UtilityRoom : Room
{
    [SerializeField]
    private Timetable timetable;

    public UtilityRoom(Timetable timetable)
    {
        this.timetable = timetable;
    }

    public bool IsCurrentlyOpen()
    {
        return timetable.IsCurrentlyOpen();
    }

    public override string Status()
    {
        if (IsCurrentlyOpen())
        {
            return "Open";
        }
        else
        {
            return "Closed";
        }
    }

    public override bool StatusValue()
    {
        return IsCurrentlyOpen();
    }
}