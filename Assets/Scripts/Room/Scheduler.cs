using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scheduler : SingletonMonoBehaviour<Scheduler>
{
    public Meeting CreateMeeting(int RoomID, string Name, DateTime Start, DateTime End)
    {
        return new Meeting(RoomID, Name, Start, End);
    }
}
