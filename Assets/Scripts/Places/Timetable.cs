using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Timetable
{
    [SerializeField]
    private OpeningHours[] openingHours = new OpeningHours[5];

    public Timetable(OpeningHours[] openingHours)
    {
        this.openingHours = openingHours;
    }

    public bool IsCurrentlyOpen()
    {
        SetPlaceholderOpeningHours();
        DateTime currentDateTime = DateTime.Now;

        int currentDayOfTheWeekIndex = (int)currentDateTime.DayOfWeek - 1;
        if (currentDayOfTheWeekIndex < 0 || currentDayOfTheWeekIndex > 5)
        {
            return false;
        }

        return openingHours[currentDayOfTheWeekIndex].IsOpen(currentDateTime.TimeOfDay);
    }

    public OpeningHours[] OpeningHours { get { return openingHours; } }

    private void SetPlaceholderOpeningHours()
    {
        openingHours = new OpeningHours[5];
        openingHours[0] = new OpeningHours(new TimeSpan(9, 30, 00), new TimeSpan(19, 00, 00));
        openingHours[1] = new OpeningHours(new TimeSpan(9, 30, 00), new TimeSpan(19, 00, 00));
        openingHours[2] = new OpeningHours(new TimeSpan(9, 30, 00), new TimeSpan(13, 00, 00));
        openingHours[3] = new OpeningHours(new TimeSpan(9, 30, 00), new TimeSpan(19, 00, 00));
        openingHours[4] = new OpeningHours(new TimeSpan(9, 30, 00), new TimeSpan(17, 00, 00));
    }
}
