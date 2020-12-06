using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class ControlPanelButton : MonoBehaviour
    {
        [SerializeField] private Image _selectedImage;
        [SerializeField] private Button _button;
        [FormerlySerializedAs("_type")] [SerializeField] public Command ButtonType;

        
        public void Initialize(Action<Command> ButtonClickCallback)
        {
            _button.onClick.AddListener(() => ButtonClickCallback(ButtonType));
        }

        public void UpdateUI(bool isSelected)
        {
            _selectedImage.enabled = isSelected;
        }
    }
}