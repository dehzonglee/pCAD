using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorUI : MonoBehaviour
{
    [SerializeField] CircleGizmo _primaryAnchorUI;
    [SerializeField] CircleGizmo _secondaryAnchorUI;

    public void UpdateUI(Anchor anchor, CoordinateUIStyle.AnchorStyle anchorStyle)
    {
        _primaryAnchorUI.UpdateUI(anchor.PrimaryPosition, anchorStyle.CircleStyle.Radius, anchorStyle.CircleStyle.Width,
            anchorStyle.CircleStyle.Color.DefaultStyle);
        _secondaryAnchorUI.UpdateUI(anchor.SecondaryPosition, anchorStyle.CircleStyle.Radius,
            anchorStyle.CircleStyle.Width, anchorStyle.CircleStyle.Color.DimmedStyle);
    }
}