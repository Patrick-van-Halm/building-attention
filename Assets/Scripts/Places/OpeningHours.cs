using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OpeningHours
{
    [SerializeField]
    private TimeSpan openingTime;
    [SerializeField]
    private TimeSpan closingTime;

    public OpeningHours(TimeSpan openingTime, TimeSpan closingTime)
    {
        if (openingTime > closingTime)
        {
            return;
        }

        this.openingTime = openingTime;
        this.closingTime = closingTime;
    }

    public bool IsOpen(TimeSpan time)
    {
        if (time > openingTime && time < closingTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public TimeSpan OpeningTime { get { return openingTime; } }
    public TimeSpan CloseingTime { get { return closingTime; } }
}
