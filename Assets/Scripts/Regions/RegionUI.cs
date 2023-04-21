using System;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class RegionUI : MonoBehaviour
{
    private RegionManager _regionManager;
    private void Awake()
    {
        _regionManager = FindObjectOfType<RegionManager>();

        _regionManager.SetColor.AddListener(SetColor);
        _regionManager.SetName.AddListener(SetName);
    }

    public void SetColor(Region region)
    {
        region.GetComponent<RawImage>().color = region.RegionInformation.Color;
    }

    public void SetName(Region region)
    {
        //region.GetComponentInChildren<Text>().text = region.RegionInformation.Type.ToString();


        string regionName = region.RegionInformation.Type.ToString();
        Type regionType = region.RegionInformation.Type.GetType();
        FieldInfo field = regionType.GetField(regionName);
        DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
        region.GetComponentInChildren<Text>().text = attribute.Description;
    }
}
