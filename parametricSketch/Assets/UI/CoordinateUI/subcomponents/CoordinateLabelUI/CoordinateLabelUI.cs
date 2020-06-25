using TMPro;
using UnityEngine;

public class CoordinateLabelUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _label = null;

    public void UpdateUI(string labelString, Vector3 labelPositionWorld,
        CoordinateUIStyle.LabelStyle style, SketchStyle.State state)
    {
        _label.text = labelString;
        _label.color = style.Color.GetForState(state);
        _label.fontSize = style.FontSize;
        
        var anchoredPosition =
            WorldScreenTransformationHelper.WorldToUISpace(GetComponentInParent<Canvas>(), labelPositionWorld);
        GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
    }
}