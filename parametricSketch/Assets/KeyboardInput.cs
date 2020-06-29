using System.Collections;
using System.Collections.Generic;
using Model;
using UI;
using UnityEngine;

public static class KeyboardInput
{
    public class Model
    {
        private GenericVector<int?> _inputInMM = new GenericVector<int?>();
//        private GenericVector<int?> _inputInMM = new GenericVector<int?>();

        public GenericVector<float?> InputInM =>
            new GenericVector<float?>()
            {
                X = 0.01f * (IsDirectionNegative.X ? -1f : 1f) * _inputInMM?.X,
                Y = 0.01f * (IsDirectionNegative.Y ? -1f : 1f) * _inputInMM?.Y,
                Z = 0.01f * (IsDirectionNegative.Z ? -1f : 1f) * _inputInMM?.Z,
            };

        public AxisID? ActiveAxis => _activeAxis;
        private AxisID? _activeAxis;

        public GenericVector<bool> IsDirectionNegative = new GenericVector<bool>(false);

        public int? ActiveInputInMM
        {
            get
            {
                if (_activeAxis == null)
                    return null;
                return _inputInMM[_activeAxis.Value];
            }
            set
            {
                if (_activeAxis == null)
                    _activeAxis = AxisID.X;
                _inputInMM[_activeAxis.Value] = value;
            }
        }

        public void SetNextAxis()
        {
            if (!_activeAxis.HasValue)
                _activeAxis = AxisID.X;
            else if (_activeAxis.Value == AxisID.X)
                _activeAxis = AxisID.Z;
            else // if ==Z
                _activeAxis = AxisID.X;
        }

        public void Reset()
        {
            _inputInMM.X = null;
            _inputInMM.Y = null;
            _inputInMM.Z = null;
            _activeAxis = null;
            IsDirectionNegative = new GenericVector<bool>()
            {
                X = false,
                Y = false,
                Z = false,
                
            };
        }
    }


    public static void UpdateKeyboardInput(ref Model model)
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
            AddDigit(model, 0);
        if (Input.GetKeyDown(KeyCode.Keypad1))
            AddDigit(model, 1);
        if (Input.GetKeyDown(KeyCode.Keypad2))
            AddDigit(model, 2);
        if (Input.GetKeyDown(KeyCode.Keypad3))
            AddDigit(model, 3);
        if (Input.GetKeyDown(KeyCode.Keypad4))
            AddDigit(model, 4);
        if (Input.GetKeyDown(KeyCode.Keypad5))
            AddDigit(model, 5);
        if (Input.GetKeyDown(KeyCode.Keypad6))
            AddDigit(model, 6);
        if (Input.GetKeyDown(KeyCode.Keypad7))
            AddDigit(model, 7);
        if (Input.GetKeyDown(KeyCode.Keypad8))
            AddDigit(model, 8);
        if (Input.GetKeyDown(KeyCode.Keypad9))
            AddDigit(model, 9);
        if (Input.GetKeyDown(KeyCode.Backspace))
            RemoveDigit(model);
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
            InvertDirection(model);
        if (Input.GetKeyDown(KeyCode.Tab))
            SetNextAxis(model);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            SetNextAxis(model);
    }

    private static void SetNextAxis(Model model)
    {
        model.SetNextAxis();
    }

    private static void InvertDirection(Model model)
    {
        if(model.ActiveAxis==null)
            return;
        
        model.IsDirectionNegative[model.ActiveAxis.Value] = !model.IsDirectionNegative[model.ActiveAxis.Value];
    }

    private static void RemoveDigit(Model model)
    {
        if (!model.ActiveInputInMM.HasValue)
            return;

        if (model.ActiveInputInMM.Value < 10)
        {
            model.ActiveInputInMM = null;
            return;
        }

        model.ActiveInputInMM = model.ActiveInputInMM.Value / 10;
    }

    private static void AddDigit(Model model, int digit)
    {
        if (!model.ActiveInputInMM.HasValue)
        {
            model.ActiveInputInMM = digit;
            return;
        }

        var currentValue = model.ActiveInputInMM.Value;
        model.ActiveInputInMM = currentValue * 10 + digit;
    }
}