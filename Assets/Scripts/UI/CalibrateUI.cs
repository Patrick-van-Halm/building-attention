using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrateUI : MonoBehaviour
{
    [SerializeField] private MoveToPlayer moveToPlayer;

    public void Recalibrate()
    {
        moveToPlayer?.RecalibrateRotation();
    }
}
