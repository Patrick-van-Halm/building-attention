using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class MapButton : MonoBehaviour
{
    [SerializeField] private Animator _mapAnim;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private Sprite _popupSprite;
    [SerializeField] private Sprite _popdownSprite;

    private bool _showMap = false;

    private void Start()
    {
        SwitchMapState();
    }

    public void SwitchMapState()
    {
        _showMap = !_showMap;

        _mapAnim.SetTrigger(_showMap ? "ShowMap" : "HideMap");
        _buttonImage.sprite = _showMap ? _popdownSprite : _popupSprite;
    }
}