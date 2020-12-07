using System;
using System.Collections.Generic;
using Model;
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

        [ContextMenu("RenameGameObjects")]
        private void RenameGameObjects()
        {
            foreach (var button in _buttons)
            {
                button.gameObject.name = button.ButtonType.ToString() + " - Button";
            }
        }
        
        public void UpdateUI(Sketch.AppModel model)
        {
            var buttonStates = new Dictionary<Command, bool>
            {
                {Command.Help, false},
                {Command.Redo, false},
                {Command.Undo, false},
                {Command.Transform, model.Tool == Sketch.Tool.Transform},
                {
                    Command.DrawPoint,
                    model.Tool == Sketch.Tool.Drawing && model.CurrentGeometryType == GeometryType.Point
                },
                {
                    Command.DrawLine,
                    model.Tool == Sketch.Tool.Drawing && model.CurrentGeometryType == GeometryType.Line
                },
                {
                    Command.DrawRect,
                    model.Tool == Sketch.Tool.Drawing && model.CurrentGeometryType == GeometryType.Rectangle
                },
                {Command.ColorBlack, model.CurrentgeometryColor == GeometryStyleAsset.GeometryColor.Black},
                {Command.ColorGrey, model.CurrentgeometryColor == GeometryStyleAsset.GeometryColor.Grey},
                {Command.ColorWhite, model.CurrentgeometryColor == GeometryStyleAsset.GeometryColor.White}
            };

            foreach (var b in _buttons)
            {
                b.UpdateUI(buttonStates[b.ButtonType]);
            }
        }
    }
}