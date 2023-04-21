using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Meeting
{
    public int meetingID { get; private set; }
    public int meetingRoomID { get; private set; }
    public string meetingName { get; private set; }
    public DateTime meetingStart { get; private set; }
    public DateTime meetingEnd { get; private set; }

    private Random random = new Random();

    public Meeting(int RoomID, string Name, DateTime Start, DateTime End)
    {
        meetingID = random.Next(100, 1000);

        meetingRoomID = RoomID;
        meetingName = Name;
        meetingStart = Start;
        meetingEnd = End;

        if (meetingEnd.Hour - meetingStart.Hour < 0)
        {
            meetingEnd = new DateTime();
        }
    }
}
