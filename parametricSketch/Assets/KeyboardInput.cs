using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Model;
using UI;
using UnityEngine;

public static class KeyboardInput
{
    public class Model
    {
        private GenericVector<int?> _inputInMM = new GenericVector<int?>();

        public GenericVector<MueParameter> ParameterReferences =
            new GenericVector<MueParameter>();

        public GenericVector<float?> InputInM =>
            new GenericVector<float?>()
            {
                X = 0.01f * (IsDirectionNegative.X ? -1f : 1f) * _inputInMM?.X,
                Y = 0.01f * (IsDirectionNegative.Y ? -1f : 1f) * _inputInMM?.Y,
                Z = 0.01f * (IsDirectionNegative.Z ? -1f : 1f) * _inputInMM?.Z,
            };

        public AxisID? ActiveAxis ;

        public GenericVector<bool> IsDirectionNegative = new GenericVector<bool>(false);

        public int? ActiveInputInMM
        {
            get => ActiveAxis == null ? null : _inputInMM[ActiveAxis.Value];
            set
            {
                if (ActiveAxis == null)
                    ActiveAxis = AxisID.X;
                _inputInMM[ActiveAxis.Value] = value;
            }
        }

        public void SetNextAxis()
        {
            if (!ActiveAxis.HasValue)
                ActiveAxis = AxisID.X;
            else if (ActiveAxis.Value == AxisID.X)
                ActiveAxis = AxisID.Z;
            else // if ==Z
                ActiveAxis = AxisID.X;
        }

        public void Reset()
        {
            _inputInMM = new GenericVector<int?>(null);
            ActiveAxis = null;
            IsDirectionNegative = new GenericVector<bool>()
            {
                X = false,
                Y = false,
                Z = false,
            };
        }
    }
    
    public static void UpdateKeyboardInput(ref Model model, List<MueParameter> availableParamters)
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
            SelectNextParameter(model, availableParamters);
    }

    private static void SelectNextParameter(Model model, List<MueParameter> availableParameters)
    {
        if(availableParameters.Count==0)
            return;
        
        if (!model.ActiveAxis.HasValue)
            model.ActiveAxis = AxisID.X;
        
        var currentlySelectedParameter = model.ParameterReferences[model.ActiveAxis.Value];
        if (currentlySelectedParameter == null)
        {
            model.ParameterReferences[model.ActiveAxis.Value] = availableParameters[0];
            return;
        }
        
        //get next in list
        var selectedIndex = availableParameters.IndexOf(currentlySelectedParameter);
        selectedIndex++;
        selectedIndex %= availableParameters.Count;

        model.ParameterReferences[model.ActiveAxis.Value] = availableParameters[selectedIndex];
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