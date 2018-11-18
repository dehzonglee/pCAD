using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorUI : MonoBehaviour
{
    [SerializeField]
    Transform _primaryAnchorUI;
    [SerializeField]
    Transform _secondaryAnchorUI;

    public void Initalize(Anchor anchor)
    {
        _anchor = anchor;
    }

    public void UpdateUI()
    {
        _primaryAnchorUI.position = _anchor.PrimaryPosition;
        _secondaryAnchorUI.position = _anchor.SecondaryPosition;
    }

    private Anchor _anchor;
}
