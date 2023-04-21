using UnityEngine;

public class RegionsToggler : MonoBehaviour
{
    [SerializeField] private GameObject regionsParent;

    public void ToggleRegions()
    {
        regionsParent.SetActive(!regionsParent.activeSelf);
    }
}
