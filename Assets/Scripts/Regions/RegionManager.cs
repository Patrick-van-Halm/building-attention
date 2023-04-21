using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RegionManager : MonoBehaviour
{
    public List<Region> Regions = new();

    [SerializeField] private Transform _regionParent;

    public UnityEvent<Region> SetColor;
    public UnityEvent<Region> SetName;
    void Start()
    {
        foreach (Region region in Regions)
        {
            //RegionInformation r = region.RegionInformation;
            //region.GetComponent<RawImage>().color = r.Color;
            SetColor?.Invoke(region);

            Region InstantiatedRegion = Instantiate(region, Vector3.zero, Quaternion.identity);
            InstantiatedRegion.transform.SetParent(_regionParent.transform, false);
            InstantiatedRegion.GetComponent<RawImage>().rectTransform.anchoredPosition = Vector3.zero;

            RectTransform x = InstantiatedRegion.GetComponent<RectTransform>();
            x.transform.localPosition = InstantiatedRegion.RegionInformation.Position;
            x.sizeDelta = region.RegionInformation.Size;

            SetName?.Invoke(InstantiatedRegion);
        }

    }
}
