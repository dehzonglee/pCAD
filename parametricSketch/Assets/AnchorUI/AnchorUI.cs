using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorUI : MonoBehaviour
{
    public void Initalize(ParametricPosition anchorPosition)
    {
        _anchorPosition = anchorPosition;
        _anchorPosition.PositionChangedEvent += UpdateUI;
        UpdateUI();
    }

    public void UpdateUI()
    {
        transform.position = _anchorPosition.Value;
    }

    private ParametricPosition _anchorPosition;
}
