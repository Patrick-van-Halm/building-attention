using System.Collections;
using TMPro;
using UnityEngine;

public class RoomInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private TextMeshProUGUI status;

    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        Marker.OnMarkerClicked.AddListener(ShowRoomInfoPanel);
    }

    private void OnDisable()
    {
        Marker.OnMarkerClicked.RemoveListener(ShowRoomInfoPanel);
    }

    private void ShowRoomInfoPanel(Place place)
    {
        if (place is Room room)
        {
            roomName.text = room.Label;
            SetRoomStatus(room);
        }
        else if (place is Utility utility)
        {
            roomName.text = utility.Type.ToString();
        }

        animator.SetBool("RoomInfoShown", true);
    }

    private void SetRoomStatus(Room room)
    {
        status.text = room.Status();
        if (room.StatusValue())
        {
            status.color = Color.green;
        }
        else
        {
            status.color = Color.red;
        }
    }
}