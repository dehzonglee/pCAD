using TMPro;
using UnityEngine;

public class CoordinateLabelUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _label = null;

    public void UpdateUI(string labelString, Vector3 labelPositionWorld)
    {
        _label.text = labelString;
        var anchoredPosition =
            WorldScreenTransformationHelper.WorldToUISpace(GetComponentInParent<Canvas>(), labelPositionWorld);
        _label.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
    }
}