using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    [SerializeField] private Place place;

    [SerializeField] private TextMeshProUGUI nameText;

    [SerializeField] private Image icon;

    public static UnityEvent<Place> OnMarkerClicked = new UnityEvent<Place>();

    private void Start()
    {
        if (place is Room room)
        {
            nameText.text = room.Label;
            icon.gameObject.SetActive(false);
        }
        else if (place is Utility utility)
        {
            nameText.gameObject.SetActive(false);
            if (utility.Type.Icon != null)
            {
                icon.sprite = utility.Type.Icon;
            }
            else
            {
                icon.gameObject.SetActive(false);
            }
        }
    }

    public virtual void OnClick()
    {
        Debug.Log("Marker " +  place.name  + " clicked");
        OnMarkerClicked.Invoke(this.place);
    }

    public void SetPlace(Place place)
    {
        this.place = place;
    }

    public Place Place { get { return place; } }
}