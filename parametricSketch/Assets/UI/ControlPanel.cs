using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class ControlPanel : MonoBehaviour
    {
        [SerializeField] private List<ControlPanelButton> _buttons = new List<ControlPanelButton>();

        public void Initialize(Action<Command> buttonClickedCallback)
        {
            foreach (var b in _buttons)
            {
                b.Initialize(buttonClickedCallback);
            }
        }

        public void UpdateUI(Dictionary<Command, bool> states)
        {
            foreach (var b in _buttons)
            {
                b.UpdateUI(states[b.ButtonType]);
            }
        }
    }
}