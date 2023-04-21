using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomScheduleManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] dayNames;
    [SerializeField] Image currentLine;
    [SerializeField] GameObject meetingObject;
    [SerializeField] List<GameObject> meetingSpawns;

    private List<Meeting> meetings = new List<Meeting>();
    private List<Day> days = new List<Day>();

    private float dayStartHours = 480;
    private float dayEndHours = 1080;
    private float dayTotalHours = 600;
    private float UIYDistance = 1.72f;
    private float UIYNegativeScale = 0.086f;

    private void Start()
    {
        meetings.Add(new Meeting(
            69, "Sprint 2 Demo - XR  Building Attention",
            new DateTime(2023, 03, 24, 13, 0, 0),
            new DateTime(2023, 03, 24, 13, 30, 0)));

        meetings.Add(new Meeting(
            69, "Deeply diving time",
            new DateTime(2023, 03, 27, 9, 30, 0),
            new DateTime(2023, 03, 27, 16, 0, 0)));

        SetDays();
        SetCurrentLine();

        ShowAllMeetings();
    }

    private void SetDays()
    {
        for (int i = 0; i < 7; i++)
        {
            string day = DateTime.Now.AddDays(i).DayOfWeek.ToString();
            if (day == "Saturday" || day == "Sunday") continue;

            days.Add(new Day(DateTime.Now.AddDays(i)));
            dayNames[days.Count - 1].text = days[days.Count - 1].date.DayOfWeek.ToString();
        }
    }

    private void SetCurrentLine()
    {
        int currentMinutes = days[0].date.Hour * 60 + days[0].date.Minute;

        if (currentMinutes > dayStartHours && currentMinutes < dayEndHours)
        {
            Vector3 linePos = currentLine.transform.localPosition;
            linePos.y -= UIYDistance * ((currentMinutes - dayStartHours) / dayTotalHours);
            currentLine.transform.localPosition = linePos;
        }
    }

    private void ShowAllMeetings()
    {
        int i = 0;

        foreach (Day day in days)
        {
            foreach (Meeting meeting in meetings)
            {
                if (meeting.meetingStart.Day == day.date.Day && 
                    meeting.meetingStart.Hour > 7 && meeting.meetingStart.Hour < 17 &&
                    meeting.meetingEnd.Hour > 8 && meeting.meetingEnd.Hour < 19)
                {
                    if (meeting.meetingEnd.Hour == 18 && meeting.meetingEnd.Minute > 0) continue;

                    GameObject meetingObjectGO = Instantiate(meetingObject, meetingSpawns[i].transform);

                    int startTime = meeting.meetingStart.Hour * 60 + meeting.meetingStart.Minute;
                    int endTime = meeting.meetingEnd.Hour * 60 + meeting.meetingEnd.Minute;
                    float meetingScale = (endTime - startTime) / 60f;

                    Vector3 meetingPos = meetingObjectGO.transform.localPosition;
                    meetingPos.y -= UIYDistance * ((startTime - dayStartHours) / dayTotalHours);
                    meetingPos.y -= UIYNegativeScale * meetingScale;
                    meetingObjectGO.transform.localPosition = meetingPos;

                    meetingObjectGO.GetComponentInChildren<TextMeshProUGUI>().text = meeting.meetingName;

                    Image image = meetingObjectGO.GetComponentInChildren<Image>();
                    Vector3 meetingObjectScale = image.transform.localScale;
                    meetingObjectScale.y *= meetingScale;
                    image.transform.localScale = meetingObjectScale;
                }
            }

            if (i < 5) i++;
            else break;
        }
    }
}
