using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ParameterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;

    public void UpdateUI(List<Parameter> parameters)
    {
        _label.text = parameters.Aggregate("", (current, parameter) => current + $"<b>{parameter.ID.Substring(0,5)}</b>:{parameter.Value}\n");
    }
}